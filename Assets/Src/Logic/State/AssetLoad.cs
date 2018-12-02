using UnityEngine;
using System.IO;
using UnityAsset;
using System.Collections.Generic;

namespace Framework
{
    using UI;
    using IO;
    using JsonFx;
    using Event;
    /// <summary>
    /// 状态
    /// </summary>
    public class AssetLoad : State
    {
        enum AssetState
        {
            Ready,
            Loading,
            Complete,
        }

        #region Variable
        /// <summary>
        /// 资源部署状态
        /// </summary>
        private AssetState m_state = AssetState.Ready;

        /// <summary>
        /// 要加载的资源大小
        /// </summary>
        private float m_size = 0;

        /// <summary>
        /// 当前已加载大小
        /// </summary>
        private float m_currentSize = 0;

        /// <summary>
        /// 当前已加载大小(真实)
        /// </summary>
        private float m_currentRealSize = 0F;

        /// <summary>
        /// 上一秒已加载大小(真实)
        /// </summary>
        private float m_lastRealSize = 0F;

        /// <summary>
        /// 时间，协助计算加载速度
        /// </summary>
        private float m_time = 0F;

        /// <summary>
        /// 速度
        /// </summary>
        private float m_speed = 0F;

        /// <summary>
        /// 加载资源记录
        /// </summary>
        private List<AsyncAsset> m_async = new List<AsyncAsset>();
        #endregion

        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);
            
            AssetManager.instance.UnloadAssets(true);
            // 获取清单文件
            AsyncAsset asset = AssetManager.instance.AssetBundleLoad(AssetManager.instance.url + Const.MANIFESTFILE);
            if (asset != null && string.IsNullOrEmpty(asset.error))
            {
                App.manifest = JsonReader.Deserialize<ManifestConfig>(asset.mainAsset.ToString());
                AssetManager.instance.setDependentAsset = App.manifest.GetDependencies;
                AssetManager.instance.UnloadAssets(asset, true);
            }
            
            // 加载Loading界面
            UIManager.instance.Clear();
            UIManager.instance.OpenUI(Const.UI_LOADING, Param.Create(new object[] {
                UILoading.TEXT_TIPS, ConfigManager.GetLang("Asset_Loading"), UILoading.SLIDER, 0F, UILoading.TEXT_DETAILS, string.Empty
            }), immediate: true);

            // 延迟启动Lua避免卡顿
            Schedule.instance.ScheduleOnce(0.03F, ()=> {
                Lua instance = Lua.instance;
            });
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public override void Update()
        {
            base.Update();

            switch (m_state)
            {
            case AssetState.Ready:
            {
                if (Lua.instance.init)
                {
                    StartAssetLoad();
                }
            } break;
            case AssetState.Loading:
            {
                m_currentRealSize = 0;
                int count = Mathf.Min(Const.MAX_LOADER, m_async.Count);
                for (int i = count - 1; i >= 0; --i)
                {
                    m_currentRealSize += (float)m_async[i].userData * m_async[i].progress;
                }
                m_currentRealSize += m_currentSize;

                if (Time.realtimeSinceStartup >= m_time + 1F)
                {
                    m_speed = m_currentRealSize - m_lastRealSize;
                    m_lastRealSize = m_currentRealSize;
                    m_time = Time.realtimeSinceStartup;
                }

                UIManager.instance.OpenUI(Const.UI_LOADING, Param.Create(new object[] {
                    UILoading.TEXT_TIPS, ConfigManager.GetLang("Asset_Loading"), UILoading.SLIDER, m_currentSize / m_size
                }));
                if (m_currentSize == m_size && Lua.instance.awake)
                {
                    m_state = AssetState.Complete;
                    AssetLoadComplete();
                }
            }
            break;
            }
        }

        /// <summary>
        /// 开始资源加载
        /// </summary>
        private void StartAssetLoad()
        {
            // 加载配置
            ConfigManager.instance.Init();
            foreach (var data in ConfigManager.instance.loadList)
            {
                AsyncAsset async = AssetManager.instance.AssetBundleAsyncLoad(AssetManager.instance.url + data.Key, (bResult, asset) =>
                {
                    if (bResult)
                    {
                        data.Value(asset.mainAsset.ToString());
                    }
                    else
                    {
                        Debugger.LogError(asset.error);
                    }
                    m_currentSize += (float)asset.userData;
                    m_async.Remove(asset);
                }, dependence: false);
                async.userData = 1F;
                m_size += (float)async.userData;
                m_async.Add(async);
            }

            if (m_async.Count > 0)
            {
                // 记录是否资源加载中
                m_state = AssetState.Loading;
                m_time = Time.realtimeSinceStartup;
            }
            else
            {
                m_state = AssetState.Complete;
                AssetLoadComplete();
            }
        }

        /// <summary>
        /// 资源加载完成
        /// </summary>
        private void AssetLoadComplete()
        {
            // 开始登陆
            StateMachine.instance.OnEnter(new StartLogin());
        }
        #endregion
    }
}