using UnityEngine;

namespace UnityAsset
{
    public abstract class AsyncAsset : AsyncOperation
    {
        /// <summary>
        /// 状态
        /// </summary>
        public enum LoadState
        {
            None = 0,
            Wait = 1,
            Loading = 2,
            Complete = 3,
            Unload = 4,
        }

        /// <summary>
        /// 加载资源类型
        /// </summary>
        public enum LoadType
        {
            None = 0,
            Resource,
            AssetsBundle,
        }

        #region Variable
        /// <summary>
        /// url
        /// </summary>
        protected string m_url = string.Empty;

        /// <summary>
        /// 主资源
        /// </summary>
        protected Object m_mainAsset = null;

        /// <summary>
        /// 加载状态
        /// </summary>
        protected LoadState m_loadState = LoadState.None;

        /// <summary>
        /// 用户数据
        /// </summary>
        protected object m_userData = null;
        #endregion

        #region Property
        /// <summary>
        /// 记载资源的类型
        /// </summary>
        public virtual LoadType loadType
        {
            get { return LoadType.None; }
        }

        /// <summary>
        /// 得到url
        /// </summary>
        /// <value>The URL Path.</value>
        public string url
        {
            get { return m_url; }
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        /// <value><c>true</c> if is done; otherwise, <c>false</c>.</value>
        public virtual bool isDone
        {
            get { return false; }
        }

        /// <summary>
        /// 进度
        /// </summary>
        /// <value>The progress.</value>
        public virtual float progress
        {
            get { return 0f; }
        }

        /// <summary>
        /// 主要资源
        /// </summary>
        /// <value>The main asset.</value>
        public virtual Object mainAsset
        {
            get { return m_mainAsset; }
        }

        /// <summary>
        /// 字节
        /// </summary>
        public virtual byte[] bytes
        {
            get { return new byte[0]; }
        }

        /// <summary>
        /// 错误
        /// </summary>
        public virtual string error
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 加载状态
        /// </summary>
        /// <value>The state of the load.</value>
        public LoadState loadState
        {
            get { return m_loadState; }
        }

        /// <summary>
        /// 用户数据
        /// </summary>
        public object userData
        {
            get { return m_userData; }
            set { m_userData = value; }
        }
        #endregion

        #region Function
        /// <summary>
        /// 构造函数
        /// </summary>
        public AsyncAsset(string url)
        {
            m_url = url;
            m_loadState = LoadState.Wait;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public virtual void AsyncLoad()
        {
            m_loadState = LoadState.Loading;
        }

        /// <summary>
        /// 异步加载完成
        /// </summary>
        public virtual void Complete()
        {
            m_loadState = LoadState.Complete;
            m_mainAsset = mainAsset;
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="unloadAllLoadedObjects">If set to <c>true</c> unload all loaded objects.</param>
        public virtual void Unload(bool unloadAllLoadedObjects)
        {
            m_loadState = LoadState.Unload;
            m_mainAsset = null;
        }
        #endregion
    }
}