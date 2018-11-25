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
        /// 本地更新清单文件MD5
        /// </summary>
        private string m_localUpdateManifestMD5 = string.Empty;

        /// <summary>
        /// 远程更新清单文件
        /// </summary>
        private ManifestConfig m_remoteUpdateManifest = null;

        /// <summary>
        /// 远程更新清单文件MD5
        /// </summary>
        private string m_remoteUpdateManifestMD5 = string.Empty;

        /// <summary>
        /// 是否资源更新中
        /// </summary>
        private bool m_assetUpdating = false;

        /// <summary>
        /// 要解压的资源大小
        /// </summary>
        private float m_size = 0;

        /// <summary>
        /// 当前已解压大小
        /// </summary>
        private float m_currentSize = 0;

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
            
            AssetManager.instance.UnloadAssets(false);
            AssetManager.instance.url = App.persistentDataPath;
            // 获取本地更新清单文件
            AsyncAsset async = AssetManager.instance.AssetBundleLoad(AssetManager.instance.url + Const.UPDATE_FILE);
            if (async != null && string.IsNullOrEmpty(async.error))
            {
                m_localUpdateManifest = JsonReader.Deserialize<ManifestConfig>(async.mainAsset.ToString());
                m_localUpdateManifestMD5 = Util.GetMD5(async.bytes);
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
                    m_remoteUpdateManifestMD5 = Util.GetMD5(m_www.bytes);
                    m_remoteUpdateManifest = JsonReader.Deserialize<ManifestConfig>(m_www.assetBundle.LoadAsset<TextAsset>(Path.GetFileNameWithoutExtension(m_www.url)).text);
                    if (m_www.assetBundle != null)
                    {
                        m_www.assetBundle.Unload(false);
                    }
                }
                else
                {
                    m_remoteUpdateManifest = new ManifestConfig();
                }
                StartAssetUpdate();

                m_www.Dispose();
                m_www = null;
            }

            // 更新中...
            if (m_assetUpdating)
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
                    UILoading.TEXT_TIPS, ConfigManager.GetLang("Asset_Updating"), UILoading.SLIDER, m_currentSize / m_size
                }));
                if (m_currentSize == m_size)
                {
                    m_assetUpdating = false;
                    AssetUpdateComplete();
                }
            }
        }

        /// <summary>
        /// 开始资源更新
        /// </summary>
        private void StartAssetUpdate()
        {
            bool hasAssetUpdate = false;
            // 解压
            m_async.Clear();
            if (m_remoteUpdateManifest.data.Count > 0)
            {
                // 最后一个更新清单文件是否下载
                hasAssetUpdate = !m_localUpdateManifestMD5.Equals(m_remoteUpdateManifestMD5);
                if (hasAssetUpdate)
                {
                    Action sure = () => {
                        // 更新清单文件
                        AsyncAsset async = AssetManager.instance.AssetBundleAsyncLoad(Path.Combine(Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTE_DIRECTORY, App.version)), Const.MANIFESTFILE), (bResult, asset) =>
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
                        async.userData = 0.5F;
                        m_size += (float)async.userData;
                        m_async.Add(async);
                        // 更新其它文件
                        foreach (var data in m_remoteUpdateManifest.data.Values)
                        {
                            if (m_localUpdateManifest.Contains(data.name))
                            {
                                if (m_localUpdateManifest.Get(data.name).MD5.Equals(data.MD5))
                                {
                                    continue;
                                }
                                else if (data.MD5.Equals(Util.GetMD5(App.assetPath + data.name)))
                                {
                                    continue;
                                }
                            }
                            async = AssetManager.instance.AssetBundleAsyncLoad(Path.Combine(Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTE_DIRECTORY, App.version)), data.name), (bResult, asset) =>
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
                        // 更新更新文件
                        async = AssetManager.instance.AssetBundleAsyncLoad(Path.Combine(Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTE_DIRECTORY, App.version)), Const.UPDATE_FILE), (bResult, asset) =>
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
                        async.userData = 0.1F;
                        m_size += (float)async.userData;
                        m_async.Add(async);

                        // 记录是否资源更新中
                        m_assetUpdating = true;
                        m_time = Time.realtimeSinceStartup;
                    };
                    Action close = () => {
                        Application.Quit();
                    };
                    UIManager.instance.OpenUI(Const.UI_NORMAL_TIPS_BOX, Param.Create(new object[] {
                        UINormalTipsBox.ACTION_SURE, sure, UINormalTipsBox.TEXT_CONTENT, ConfigManager.GetLang("Network_Invalid"), UINormalTipsBox.ACTION_CLOSE, close
                    }));
                }
            }
            
            // 如果没有资源更新，就直接认为更新完成
            if (!hasAssetUpdate)
            {
                AssetUpdateComplete();
            }
        }

        /// <summary>
        /// 资源更新完成
        /// </summary>
        private void AssetUpdateComplete()
        {
            // 清理资源
            AssetManager.instance.UnloadAssets(true);
            UIManager.instance.Clear();
            // 加载资源
            UIManager.instance.OpenUI(Const.UI_LOADING, immediate: true);
        }
        #endregion
    }
}