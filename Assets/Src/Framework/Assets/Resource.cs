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
        /// 是否完成
        /// </summary>
        /// <value><c>true</c> if is done; otherwise, <c>false</c>.</value>
        public new bool isDone
        {
            get { return m_resourceRequest != null ? m_resourceRequest.isDone : false; }
        }

        /// <summary>
        /// 进度
        /// </summary>
        /// <value>The progress.</value>
        public new float progress
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
        #endregion
    }
}