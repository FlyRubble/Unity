using UnityEngine;
using System;
using System.Collections.Generic;

namespace Framework
{
    using Singleton;
    namespace UI
    {
        /// <summary>
        /// UI管理器
        /// </summary>
        public sealed class UIManager : Singleton<UIManager>
        {
            #region Variable
            /// <summary>
            /// 界面集合
            /// </summary>
            Dictionary<string, UIBase> m_data = null;
            #endregion

            #region Function
            /// <summary>
            /// 构造
            /// </summary>
            public UIManager()
            {
                m_data = new Dictionary<string, UIBase>();
            }

            /// <summary>
            /// 是否包含界面
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool Contain(string name)
            {
                return m_data.ContainsKey(name);
            }

            /// <summary>
            /// 得到界面
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="name"></param>
            /// <returns></returns>
            public T GetUI<T>(string name) where T : UIBase
            {
                UIBase t = null;
                m_data.TryGetValue(name, out t);
                return (T)t;
            }

            /// <summary>
            /// 得到界面
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public T GetUI<T>() where T : UIBase
            {
                T t = Activator.CreateInstance(typeof(T)) as T;
                return null != t ? GetUI<T>(t.getName) : null;
            }

            /// <summary>
            /// 打开界面
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="param"></param>
            /// <returns></returns>
            public T OpenUI<T>(Param param) where T : UIBase
            {
                T t = Activator.CreateInstance(typeof(T)) as T;
                if (null == t) return null;

                if (m_data.ContainsKey(t.getName))
                {

                }
                else
                {

                }
                return null;
            }

            public UIBase OpenUI<T>(string name, Param param) where T : UIBase
            {
                return null;
            }

            public UIBase OpenUI(string name, Param param)
            {
                return null;
            }
            #endregion
        }
    }
}
