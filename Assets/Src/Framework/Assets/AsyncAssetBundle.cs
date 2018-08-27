using Framework.Event;

namespace UnityAsset
{
    /// <summary>
    /// 异步资源包
    /// </summary>
    public struct AsyncAssetBundle
    {
        #region Variable
        /// <summary>
        /// 异步资源
        /// </summary>
        AsyncAsset m_asyncAsset;
        
        /// <summary>
        /// 完成事件
        /// </summary>
        Action<bool, AsyncAsset> m_action;
        #endregion

        #region Property
        /// <summary>
        /// 异步资源
        /// </summary>
        public AsyncAsset asyncAsset
        {
            get { return m_asyncAsset; }
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isDone
        {
            get { return m_asyncAsset != null ? m_asyncAsset.isDone : false; }
        }

        /// <summary>
        /// 错误
        /// </summary>
        string error
        {
            get
            {
                return m_asyncAsset != null ? m_asyncAsset.error : "unknown error";
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="asyncAsset"></param>
        /// <param name="action"></param>
        public AsyncAssetBundle(AsyncAsset asyncAsset, Action<bool, AsyncAsset> action)
        {
            m_asyncAsset = asyncAsset;
            m_action = action;
        }

        /// <summary>
        /// 异步加载完成
        /// </summary>
        public void Complete()
        {
            if (m_action != null)
            {
                m_action(string.IsNullOrEmpty(error), m_asyncAsset);
            }
        }
        #endregion
    }
}