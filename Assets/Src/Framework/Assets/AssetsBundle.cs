using UnityEngine;
using System.IO;

namespace UnityAsset
{
    public sealed class AssetsBundle : AsyncAsset
    {
        #region Variable
        /// <summary>
		/// WWW
		/// </summary>
        private WWW m_www = null;
        #endregion

        #region Property
        /// <summary>
        /// 是否完成
        /// </summary>
        /// <value><c>true</c> if is done; otherwise, <c>false</c>.</value>
        public override bool isDone
        {
            get { return m_www != null ? m_www.isDone : false; }
        }

        /// <summary>
        /// 进度
        /// </summary>
        /// <value>The progress.</value>
        public override float progress
        {
            get { return m_www != null ? m_www.progress : 0f; }
        }

        /// <summary>
        /// 主要资源
        /// </summary>
        /// <value>The main asset.</value>
        public override Object mainAsset
        {
            get
            {
                if (m_mainAsset == null && m_www != null && m_www.assetBundle != null && !m_www.assetBundle.isStreamedSceneAssetBundle)
                {
                    m_mainAsset = m_www.assetBundle.LoadAsset(Path.GetFileNameWithoutExtension(m_url));
                }
                return m_mainAsset;
            }
        }

        /// <summary>
        /// 字节
        /// </summary>
        public override byte[] bytes
        {
            get
            {
                return m_www != null ? m_www.bytes : base.bytes;
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        public override string error
        {
            get
            {
                return m_www != null ? m_www.error : "unknown error";
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssetsBundle(string url)
            : base(url)
        { }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public override void AsyncLoad()
        {
            m_www = new WWW(m_url);
            base.AsyncLoad();
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="unloadAllLoadedObjects"></param>
        public override void Unload(bool unloadAllLoadedObjects)
        {
            base.Unload(unloadAllLoadedObjects);
            if (m_www != null && m_www.assetBundle != null && !m_www.assetBundle.isStreamedSceneAssetBundle)
            {
                m_www.assetBundle.Unload(unloadAllLoadedObjects);
            }
        }
        #endregion
    }
}