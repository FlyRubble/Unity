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
            Stop = 4,
        }

        #region Variable
        /// <summary>
        /// url
        /// </summary>
        protected string m_url = string.Empty;

        /// <summary>
        /// 是否停止
        /// </summary>
        protected bool m_stop = false;

        /// <summary>
        /// 主资源
        /// </summary>
        protected Object m_mainAsset = null;

        /// <summary>
        /// 加载状态
        /// </summary>
        protected LoadState m_loadState = LoadState.None;
        #endregion

        #region Property
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
        #endregion

        #region Function
        /// <summary>
        /// 构造函数
        /// </summary>
        public AsyncAsset(string url)
        {
            m_url = url;
            m_stop = false;
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
        /// 停止
        /// </summary>
        public void Stop()
        {
            m_stop = true;
            m_loadState = LoadState.Stop;
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="unloadAllLoadedObjects">If set to <c>true</c> unload all loaded objects.</param>
        public virtual void Unload(bool unloadAllLoadedObjects)
        {
            Stop();
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public void Unload()
        {
            Unload(false);
        }
        #endregion
    }
}