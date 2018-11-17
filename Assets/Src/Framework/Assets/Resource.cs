using UnityEngine;

namespace UnityAsset
{
    public sealed class Resource : AsyncAsset
    {
        #region Variable
        /// <summary>
		/// Resource资源
		/// </summary>
        private ResourceRequest m_resourceRequest = null;
        #endregion

        #region Property
        /// <summary>
        /// 记载资源的类型
        /// </summary>
        public override LoadType loadType
        {
            get { return LoadType.Resource; }
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        /// <value><c>true</c> if is done; otherwise, <c>false</c>.</value>
        public override bool isDone
        {
            get { return m_resourceRequest != null ? m_resourceRequest.isDone : false; }
        }

        /// <summary>
        /// 进度
        /// </summary>
        /// <value>The progress.</value>
        public override float progress
        {
            get { return m_resourceRequest != null ? m_resourceRequest.progress : 0f; }
        }

        /// <summary>
        /// 主要资源
        /// </summary>
        public override Object mainAsset
        {
            get
            {
                if (m_mainAsset == null && m_resourceRequest != null)
                {
                    m_mainAsset = m_resourceRequest.asset;
                }
                return m_mainAsset;
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        public override string error
        {
            get
            {
                return m_resourceRequest != null ? "" : "unknown error";
            }
        }

        /// <summary>
        /// 得到文本
        /// </summary>
        public override string text
        {
            get
            {
                return mainAsset != null ? mainAsset.ToString() : string.Empty;
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 构造函数
        /// </summary>
        public Resource(string url)
            : base(url)
        {  }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public override void AsyncLoad()
        {
            m_resourceRequest = Resources.LoadAsync(m_url);
            base.AsyncLoad();
        }

        /// <summary>
        /// 卸载
        /// </summary>
        /// <param name="unloadAllLoadedObjects"></param>
        public override void Unload(bool unloadAllLoadedObjects)
        {
            Resources.UnloadAsset(m_mainAsset);
            base.Unload(unloadAllLoadedObjects);
            m_resourceRequest = null;
        }
        #endregion
    }
}