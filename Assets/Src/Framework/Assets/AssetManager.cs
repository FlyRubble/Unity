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

        /// <summary>
        /// 得到依赖资源
        /// </summary>
        /// <returns></returns>
        public delegate List<string> DependentAsset(string path);
        private DependentAsset m_getDependentAsset = null;
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

        /// <summary>
        /// 设置依赖资源
        /// </summary>
        public DependentAsset setDependentAsset
        {
            set
            {
                if (value != null)
                {
                    m_getDependentAsset = value;
                }
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssetManager()
        {
            m_complete = new Dictionary<string, AsyncAsset>(128);
            m_loading = new Dictionary<string, AsyncAsset>(16);
            m_queue = new List<AsyncAssetBundle>(64);
            m_remove = new List<AsyncAsset>(m_loading.Count);
            m_getDependentAsset = (path) => { return new List<string>(); };
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
        public AsyncAsset ResourceAsyncLoad(string path, Action<bool, AsyncAsset> action)
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
        public AsyncAsset AssetBundleLoad(string path)
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
                    async.AsyncLoad();
                }
                while (!async.isDone) { }
                async.Complete();
                m_complete.Add(async.url, async);
            }

            return async;
        }
        
        /// <summary>
        /// AssetBundle资源加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public AsyncAsset AssetBundleAsyncLoad(string path, Action<bool, AsyncAsset> action, Dictionary<string, AsyncAsset> dic = null)
        {
            return DependentAssetLoad(path, action, dic);
        }

        /// <summary>
        /// AssetBundle资源加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private AsyncAsset AssetBundleAsyncLoadWithoutDependent(string path, Action<bool, AsyncAsset> action)
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
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <param name="dic"></param>
        private AsyncAsset DependentAssetLoad(string path, Action<bool, AsyncAsset> action, Dictionary<string, AsyncAsset> dic)
        {
            var data = m_getDependentAsset(path);
            AssetsBundle[] dependent = new AssetsBundle[data.Count];
            for (int i = 0;i < data.Count; ++i)
            {
                dependent[i] = (AssetsBundle)DependentAssetLoad(data[i], action, dic);
            }

            AssetsBundle asset = (AssetsBundle)AssetBundleAsyncLoadWithoutDependent(path, action);
            if (null != dic)
            {
                dic.Add(path, asset);
            }

            for (int i = 0; i < dependent.Length; ++i)
            {
                dependent[i].AddDependentSelf(asset);
                asset.AddSelfDependent(dependent[i]);
            }

            return asset;
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
                    m_queue.Remove(asyncBundle);
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
        /// <param name="unloadAllLoadedObjects"></param>
        public void UnloadAssets(bool unloadAllLoadedObjects)
        {
            foreach (var asyn in m_complete.Values)
            {
                asyn.Unload(unloadAllLoadedObjects);
            }
            foreach (var asyn in m_loading.Values)
            {
                asyn.Unload(unloadAllLoadedObjects);
            }

            m_complete.Clear();
            m_loading.Clear();
            m_queue.Clear();
        }

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="unloadAllLoadedObjects"></param>
        public void UnloadAssets(AsyncAsset asset, bool unloadAllLoadedObjects)
        {
            if (null == asset) return;

            if (m_complete.ContainsKey(asset.url) && asset == m_complete[asset.url])
            {
                asset.Unload(unloadAllLoadedObjects);
                m_complete.Remove(asset.url);
            }
            else if (m_loading.ContainsKey(asset.url) && asset == m_loading[asset.url])
            {
                asset.Unload(unloadAllLoadedObjects);
                m_loading.Remove(asset.url);
            }
            for (int i = 0; i < m_queue.Count; ++i)
            {
                if (m_queue[i].asyncAsset == asset)
                {
                    m_queue.RemoveAt(i);
                    break;
                }
            }
        }
#endregion
    }
}
