using UnityEngine;
using System.IO;
using UnityAsset;
using System.Collections.Generic;

namespace Framework
{
    using UI;
    using IO;
    using JsonFx;
    /// <summary>
    /// 状态
    /// </summary>
    public class AssetUpdate : State
    {
        #region Variable
        /// <summary>
        /// WWW资源请求
        /// </summary>
        private WWW m_www = null;

        /// <summary>
        /// 本地更新清单文件
        /// </summary>
        private ManifestConfig m_localUpdateManifest = null;

        /// <summary>
        /// 远程更新清单文件
        /// </summary>
        private ManifestConfig m_remoteUpdateManifest = null;

        /// <summary>
        /// 是否有资源更新
        /// </summary>
        private bool m_hasAssetUpdate = false;

        /// <summary>
        /// 时间，用于计算解压速度
        /// </summary>
        private float m_time = 0;

        /// <summary>
        /// 要解压的资源大小
        /// </summary>
        private float m_size = 0;

        /// <summary>
        /// 当前已解压大小
        /// </summary>
        private float m_currentSize = 0;

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

            AssetManager.instance.url = App.persistentDataPath;
            // 清理资源，并且重新打开Loading界面
            AssetManager.instance.UnloadAssets(true);
            // 获取本地更新清单文件
            AsyncAsset async = AssetManager.instance.AssetBundleLoad(AssetManager.instance.url + Const.UPDATE_FILE);
            if (async != null && string.IsNullOrEmpty(async.error))
            {
                m_localUpdateManifest = JsonReader.Deserialize<ManifestConfig>(async.mainAsset.ToString());
                AssetManager.instance.UnloadAssets(async, true);
            }

            // 获取远程更新清单文件
            m_www = new WWW(Path.Combine(Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTE_DIRECTORY, App.version)), Const.UPDATE_FILE));
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public override void Update()
        {
            base.Update();

            // 远程更新清单文件加载
            if (m_www != null && m_www.isDone)
            {
                if (string.IsNullOrEmpty(m_www.error))
                {
                    m_remoteUpdateManifest = JsonReader.Deserialize<ManifestConfig>(m_www.assetBundle.LoadAsset<TextAsset>(Path.GetFileNameWithoutExtension(m_www.url)).text);
                    if (m_www.assetBundle != null)
                    {
                        m_www.assetBundle.Unload(true);
                    }
                }
                else
                {
                    m_remoteUpdateManifest = new ManifestConfig();
                }
                StartAssetUpdate();
                m_www = null;
            }

            // 更新中...
            if (m_hasAssetUpdate)
            {
                float speed = m_currentSize;
                if (Time.realtimeSinceStartup >= m_time + 1F)
                {
                    m_time = Time.realtimeSinceStartup;

                    m_currentSize = 0;
                    foreach (var data in m_async)
                    {
                        m_currentSize += (float)data.userData * data.progress;
                        Debugger.Log(data.url);
                    }
                    speed = m_currentSize - speed;
                }
                UIManager.instance.OpenUI(Const.UI_LOADING, Param.Create(new object[] { UILoading.SLIDER, m_currentSize / m_size }));
                if (m_currentSize == m_size)
                {
                    m_hasAssetUpdate = false;
                    AssetUpdateComplete();
                }
            }
        }

        /// <summary>
        /// 开始资源更新
        /// </summary>
        private void StartAssetUpdate()
        {
            // 解压
            m_async.Clear();
            if (m_remoteUpdateManifest.data.Count > 0)
            {
                AsyncAsset async = AssetManager.instance.AssetBundleAsyncLoad(Path.Combine(Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTE_DIRECTORY, App.version)), Const.UPDATE_FILE), (bResult, asset) =>
                {
                    Util.WriteAllBytes(App.assetPath + Const.UPDATE_FILE, asset.bytes);
                });
                async.userData = 0.1F;
                m_size += (float)async.userData;
                m_async.Add(async);
                foreach (var data in m_remoteUpdateManifest.data.Values)
                {
                    if (m_localUpdateManifest.Contains(data.name) && m_localUpdateManifest.Get(data.name).MD5.Equals(data.MD5))
                    {
                        continue;
                    }
                    async = AssetManager.instance.AssetBundleAsyncLoad(Path.Combine(Path.Combine(App.cdn + App.platform , string.Format(Const.REMOTE_DIRECTORY, App.version)), data.name), (bResult, asset) =>
                    {
                        Util.WriteAllBytes(App.assetPath + data.name, asset.bytes);
                    });
                    async.userData = data.size / 1024F;
                    m_size += (float)async.userData;
                    m_async.Add(async);
                }
                async = AssetManager.instance.AssetBundleAsyncLoad(Path.Combine(Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTE_DIRECTORY, App.version)), Const.MANIFESTFILE), (bResult, asset) =>
                {
                    Util.WriteAllBytes(App.assetPath + Const.MANIFESTFILE, asset.bytes);
                });
                async.userData = 0.5F;
                m_size += (float)async.userData;
                m_async.Add(async);
                // 设置有资源更新标志 至少有update文件和version文件，所以至少要大于2
                m_hasAssetUpdate = m_async.Count > 2;
            }
            else
            {
                AssetUpdateComplete();
            }
        }

        /// <summary>
        /// 资源更新完成
        /// </summary>
        private void AssetUpdateComplete()
        {
            // 清理资源，打开登录
            AssetManager.instance.UnloadAssets(true);
            UIManager.instance.Clear();
            UIManager.instance.OpenUI(Const.UI_LOADING);
            Debugger.Log("开始登陆");
        }
        #endregion
    }
}