using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.IO;

namespace Framework
{
    namespace IO
    {
        /// <summary>
        /// 配置
        /// </summary>
        public class ManifestConfig
        {
            #region Variable
            /// <summary>
            /// 数据
            /// </summary>
            Dictionary<string, Manifest> m_data = new Dictionary<string, Manifest>(20480);
            #endregion

            #region Property
            /// <summary>
            /// 得到数据
            /// </summary>
            /// <value>The data.</value>
            public Dictionary<string, Manifest> data
            {
                get { return m_data; }
                set { m_data = value; }
            }
            #endregion

            #region Function
            /// <summary>
            /// 添加
            /// </summary>
            /// <param name="t">T.</param>
            public void Add(Manifest manifest)
            {
                if (m_data.ContainsKey(manifest.name))
                {
                    m_data[manifest.name] = manifest;
                }
                else
                {
                    m_data.Add(manifest.name, manifest);
                }
            }

            /// <summary>
            /// 得到一份清单
            /// </summary>
            /// <param name="name">Name.</param>
            public Manifest Get(string name)
            {
                return m_data.ContainsKey(name) ? m_data[name] : null;
            }

            /// <summary>
            /// 得到依赖资源表
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public List<string> GetDependencies(string name)
            {
                Manifest manifest = Get(name);
                return manifest != null ? manifest.dependencies : new List<string>();
            }

            /// <summary>
            /// 是否包含值
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool Contains(string name)
            {
                return m_data.ContainsKey(name);
            }
            #endregion
        }
    }
}