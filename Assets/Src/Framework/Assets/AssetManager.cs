using System.Collections.Generic;
using System.Linq;
using Framework.Singleton;
using UnityEngine;
using Framework.Event;

namespace UnityAsset
{
    public class AssetManager : Singleton<AssetManager>
    {
        #region Variable
        /// <summary>
        /// 记录已加载完成的所有异步资源
        /// </summary>
        Dictionary<string, AsyncAsset> m_complete = null;

        /// <summary>
        /// 记录正在加载中的异步资源
        /// </summary>
        Dictionary<string, AsyncAsset> m_loading = null;

        /// <summary>
        /// 等待加载的异步资源
        /// </summary>
        List<AsyncAssetBundle> m_queue = null;

        /// <summary>
        /// 待移除表
        /// </summary>
        List<AsyncAsset> m_remove = null;

        /// <summary>
        /// 是否允许加载
        /// </summary>
        bool m_isAllowLoad = true;

        /// <summary>
        /// 最大同时加载个数
        /// </summary>
        int m_maxLoader = 1;

        /// <summary>
        /// 当前最大加载数
        /// </summary>
        int m_currentMaxLoader = 0;

        /// <summary>
        /// URL
        /// </summary>
        string m_url = string.Empty;
        #endregion

        #region Property
        /// <summary>
        /// 得到或设置是否允许加载
        /// </summary>
        public bool isAllowload
        {
            get { return m_isAllowLoad; }
            set { m_isAllowLoad = value; }
        }

        /// <summary>
        /// 得到或设置最大同时加载数
        /// </summary>
        /// <value>The max loader.</value>
        public int maxLoader
        {
            get { return m_maxLoader; }
            set { m_maxLoader = value; }
        }

        /// <summary>
        /// URL
        /// </summary>
        public string url
        {
            get { return m_url; }
            set { m_url = value; }
        }
        #endregion

        #region Function
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssetManager()
        {
            m_complete = new Dictionary<string, AsyncAsset>(128);
            m_loading = new Dictionary<string, AsyncAsset>(64);
            m_queue = new List<AsyncAssetBundle>(64);
            m_remove = new List<AsyncAsset>(8);
        }

        /// <summary>
        /// Resources加载
        /// </summary>
        /// <param name="path">Path.</param>
        public Object ResourceLoad(string path)
        {
            return Resources.Load(path);
        }

        /// <summary>
        /// Resources加载
        /// </summary>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T ResourceLoad<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// Resources加载
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="type">Type.</param>
        public Object ResourceLoad(string path, System.Type type)
        {
            return Resources.Load(path, type);
        }

        /// <summary>
        /// Resource异步加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public AsyncAsset ResourceLoadAsync(string path, Action<bool, AsyncAsset> action)
        {
            AsyncAsset async = null;
            if (m_complete.ContainsKey(path))
            {
                async = m_complete[path];
            }
            else
            {
                if (m_loading.ContainsKey(path))
                {
                    async = m_loading[path];
                }
                else
                {
                    async = new Resource(path);
                    m_loading.Add(async.url, async);
                }
            }
            m_queue.Add(new AsyncAssetBundle(async, action));

            return async;
        }

        /// <summary>
        /// AssetBundle加载
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Object AssetBundleLoad(string url)
        {
            AsyncAsset async = null;
            if (m_complete.ContainsKey(url))
            {
                async = m_complete[url];
            }
            else
            {
                if (m_loading.ContainsKey(url))
                {
                    async = m_loading[url];
                }
                else
                {
                    async = new AssetsBundle(url);
                    async.AsyncLoad();
                    m_complete.Add(async.url, async);
                }
                while (!async.isDone) { }
            }

            return async.mainAsset;
        }

        /// <summary>
        /// AssetBundle资源加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public AsyncAsset AssetBundleLoadAsync(string path, Action<bool, AsyncAsset> action)
        {
            AsyncAsset async = null;
            if (m_complete.ContainsKey(path))
            {
                async = m_complete[path];
            }
            else
            {
                if (m_loading.ContainsKey(path))
                {
                    async = m_loading[path];
                }
                else
                {
                    async = new AssetsBundle(path);
                    m_loading.Add(async.url, async);
                }
            }
            m_queue.Add(new AsyncAssetBundle(async, action));

            return async;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        public void Load(string path, Action<bool, Object> action)
        {
#if UNITY_EDITOR && !AB_MODE
            if (action != null)
            {
                path = "Assets/" + path;
                action(true, UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
            }
#else
            path = m_url + path;
            if (action != null)
            {
                AssetBundleLoadAsync(path, (bResult, asset) => {
                    action(bResult, asset.mainAsset);
                });
            }
#endif
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (m_isAllowLoad)
            {
                // 正在加载中的处理
                m_currentMaxLoader = Mathf.Min(m_maxLoader, m_loading.Count);
                if (m_currentMaxLoader > 0)
                {
                    m_remove.Clear();
                    foreach (var kvp in m_loading)
                    {
                        switch (kvp.Value.loadState)
                        {
                            case AsyncAsset.LoadState.Wait:
                                {
                                    kvp.Value.AsyncLoad();
                                }
                                break;
                            case AsyncAsset.LoadState.Loading:
                                {
                                    if (kvp.Value.isDone)
                                    {
                                        kvp.Value.Complete();
                                        m_remove.Add(kvp.Value);
                                    }
                                }
                                break;
                        }
                        if (--m_currentMaxLoader == 0)
                        {
                            break;
                        }
                    }
                    foreach (var asyn in m_remove)
                    {
                        m_complete.Add(asyn.url, asyn);
                        m_loading.Remove(asyn.url);
                    }
                }
                // 等待加载中的处理
                if (m_queue.Count > 0 && m_queue[0].isDone)
                {
                    AsyncAssetBundle asyncBundle = m_queue[0];
                    m_queue.RemoveAt(0);
                    asyncBundle.Complete();
                }
            }
        }

        /// <summary>
        /// 完成列表是否包含
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool CompleteContains(string key)
        {
            return m_complete.ContainsKey(key);
        }

        /// <summary>
        /// 正在加载列表是否包含
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool LoadingContains(string key)
        {
            return m_loading.ContainsKey(key);
        }

        /// <summary>
        /// 卸载所有资源
        /// </summary>
        public void UnloadAssets()
        {
            foreach (var v in m_loading.Values)
            {
                v.Unload(true);
            }
            foreach (var v in m_complete.Values)
            {
                v.Unload(true);
            }
            Resources.UnloadUnusedAssets();

            m_complete.Clear();
            m_loading.Clear();
            m_queue.Clear();
        }

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="asset">Asset.</param>
        public void UnloadAssets(AsyncAsset asset)
        {
            if (null == asset) return;

            UnloadAssets(asset, true);
        }

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="asset">Asset.</param>
        public void UnloadAssets(AsyncAsset[] assets)
        {
            if (null == assets) return;

            foreach (var asset in assets)
            {
                UnloadAssets(asset, false);
            }
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="unloadUnusedAssets"></param>
        private void UnloadAssets(AsyncAsset asset, bool unloadUnusedAssets)
        {
            if (m_loading.ContainsKey(asset.url))
            {
                m_loading[asset.url].Unload(true);
                m_loading.Remove(asset.url);
            }
            if (m_complete.ContainsKey(asset.url))
            {
                m_complete[asset.url].Unload(true);
                m_complete.Remove(asset.url);
            }
            for (int i = m_queue.Count - 1; i >= 0; --i)
            {
                if (m_queue[i].asyncAsset == asset)
                {
                    m_queue.RemoveAt(i);
                }
            }
            if (unloadUnusedAssets)
            {
                Resources.UnloadUnusedAssets();
            }
        }
#endregion
    }
}
