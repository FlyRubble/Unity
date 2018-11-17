using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace UnityAsset
{
    public sealed class AssetsBundle : AsyncAsset
    {
        #region Variable
        /// <summary>
		/// WWW
		/// </summary>
        private WWW m_www = null;
        
        /// <summary>
        /// 被依赖表(依赖本对象的表)
        /// </summary>
        List<AssetsBundle> m_dependentSelf = null;

        /// <summary>
        /// 依赖表(被本对象依赖的表)
        /// </summary>
        List<AssetsBundle> m_selfDependent = null;
        #endregion

        #region Property
        /// <summary>
        /// 记载资源的类型
        /// </summary>
        public override LoadType loadType
        {
            get { return LoadType.AssetsBundle; }
        }

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

        /// <summary>
        /// 得到文本
        /// </summary>
        public override string text
        {
            get
            {
                return (mainAsset != null) ? mainAsset.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// 被依赖表(依赖本对象的表)
        /// </summary>
        public List<AssetsBundle> dependentSelf
        {
            get
            {
                return m_dependentSelf;
            }
        }

        /// <summary>
        /// 依赖表(被本对象依赖的表)
        /// </summary>
        public List<AssetsBundle> selfDependent
        {
            get
            {
                return m_selfDependent;
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssetsBundle(string url)
            : base(url)
        {
            m_dependentSelf = new List<AssetsBundle>();
            m_selfDependent = new List<AssetsBundle>();
        }

        /// <summary>
        /// 添加依赖本对象的资源
        /// </summary>
        /// <param name="assets"></param>
        public void AddDependentSelf(AssetsBundle assets)
        {
            m_dependentSelf.Add(assets);
        }
        
        /// <summary>
        /// 添加自己依赖的资源
        /// </summary>
        /// <param name="assets"></param>
        public void AddSelfDependent(AssetsBundle assets)
        {
            m_selfDependent.Add(assets);
        }

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
            if (m_www != null && m_www.assetBundle != null && !m_www.assetBundle.isStreamedSceneAssetBundle)
            {
                m_www.assetBundle.Unload(unloadAllLoadedObjects);
            }
            base.Unload(unloadAllLoadedObjects);
            m_www.Dispose();
            m_www = null;

            for (int i = 0; i < m_dependentSelf.Count; ++i)
            {
                m_dependentSelf[i].selfDependent.Remove(this);
            }
            for (int i = 0; i < m_selfDependent.Count; ++i)
            {
                m_selfDependent[i].dependentSelf.Remove(this);
            }
            m_dependentSelf.Clear();
            m_selfDependent.Clear();
        }
        #endregion
    }
}