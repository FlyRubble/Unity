using UnityEngine;
using System.IO;
using UnityAsset;
using System.Collections.Generic;

namespace Framework
{
    using UI;
    using Event;
    /// <summary>
    /// 状态
    /// </summary>
    public class AssetDecompressing : State
    {
        #region Variable
        /// <summary>
        /// 沙盒资源是否需要解压
        /// </summary>
        private bool m_assetDecompressing = false;

        /// <summary>
        /// 要解压的资源大小
        /// </summary>
        private float m_size = 0F;

        /// <summary>
        /// 当前已解压大小(已解压完整文件大小)
        /// </summary>
        private float m_currentSize = 0F;

        /// <summary>
        /// 当前已解压大小(真实)
        /// </summary>
        private float m_currentRealSize = 0F;

        /// <summary>
        /// 上一秒已解压大小(真实)
        /// </summary>
        private float m_lastRealSize = 0F;

        /// <summary>
        /// 时间，协助计算解压速度
        /// </summary>
        private float m_time = 0F;

        /// <summary>
        /// 速度
        /// </summary>
        private float m_speed = 0F;

        /// <summary>
        /// 解压资源记录
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
            
            // 是否需要解压资源到沙河路径
            m_assetDecompressing = !App.version.Equals(Util.GetString(Const.SANDBOX_VERSION));
            if (m_assetDecompressing)
            {
                // 清空文件夹，以便重新解压
                if (Directory.Exists(Application.persistentDataPath))
                {
                    Directory.Delete(Application.persistentDataPath, true);
                }
                // 解压准备
                m_async.Clear();
                AsyncAsset async = AssetManager.instance.AssetBundleAsyncLoad(AssetManager.instance.url + Const.MANIFESTFILE, (bResult, asset) =>
                {
                    if (bResult)
                    {
                        Util.WriteAllBytes(App.assetPath + Const.MANIFESTFILE, asset.bytes);
                    }
                    else
                    {
                        Debugger.LogError(asset.error);
                    }
                }, dependence: false);
                async.userData = 0.5F;
                m_size += (float)async.userData;
                m_async.Add(async);
                foreach (var data in App.manifest.data.Values)
                {
                    async = AssetManager.instance.AssetBundleAsyncLoad(AssetManager.instance.url + data.name, (bResult, asset) =>
                    {
                        if (bResult)
                        {
                            Util.WriteAllBytes(App.assetPath + data.name, asset.bytes);
                        }
                        else
                        {
                            Debugger.LogError(asset.error);
                        }
                    }, dependence: false);
                    async.userData = data.size / 1024F;
                    m_size += (float)async.userData;
                    m_async.Add(async);
                }
                async = AssetManager.instance.AssetBundleAsyncLoad(AssetManager.instance.url + Const.UPDATE_FILE, (bResult, asset) =>
                {
                    if (bResult)
                    {
                        Util.WriteAllBytes(App.assetPath + Const.UPDATE_FILE, asset.bytes);
                    }
                    else
                    {
                        Debugger.LogError(asset.error);
                    }
                }, dependence: false);
                async.userData = 0.1F;
                m_size += (float)async.userData;
                m_async.Add(async);

                m_time = Time.realtimeSinceStartup;
            }
            else
            {
                StateMachine.instance.OnEnter(new AssetUpdate());
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (m_assetDecompressing)
            {
                m_currentRealSize = 0;
                int count = Mathf.Min(Const.MAX_LOADER, m_async.Count);
                for (int i = count - 1; i >= 0; --i)
                {
                    if (m_async[i].progress == 1F)
                    {
                        m_currentSize += (float)m_async[i].userData;
                        m_async.RemoveAt(i);
                        continue;
                    }
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
                    UILoading.TEXT_TIPS, ConfigManager.GetLang("Asset_Decompressing"), UILoading.SLIDER, m_currentSize / m_size
                }));
                if (m_currentSize == m_size)
                {
                    m_assetDecompressing = false;
                    PlayerPrefs.SetString(Const.SANDBOX_VERSION, App.version);
                    Schedule.instance.ScheduleOnce(0.18F, () =>
                    {
                        StateMachine.instance.OnEnter(new AssetUpdate());
                    });
                }
            }
        }
        #endregion
    }
}