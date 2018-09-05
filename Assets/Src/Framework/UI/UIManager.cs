using UnityEngine;
using System.Collections.Generic;

namespace Framework
{
    using Singleton;
    using Event;
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
            private Dictionary<string, UIBase> m_data = null;

            /// <summary>
            /// 参数表
            /// </summary>
            private Dictionary<string, Param> m_param = null;

            /// <summary>
            /// root
            /// </summary>
            private Transform m_root = null;
            #endregion

            #region Function
            /// <summary>
            /// 构造
            /// </summary>
            public UIManager()
            {
                m_data = new Dictionary<string, UIBase>();
                m_param = new Dictionary<string, Param>();

                GameObject go = GameObject.Find("Canvas/Center");
                if (null != go)
                {
                    GameObject.DontDestroyOnLoad(go.transform.parent.gameObject);
                    m_root = go.transform;
                }
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
            /// 得到UI
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public T GetUI<T>() where T : UIBase
            {
                string name = typeof(T).ToString();
                return GetUI<T>(name);
            }

            /// <summary>
            /// 得到UI
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="name"></param>
            /// <returns></returns>
            public T GetUI<T>(string name) where T : UIBase
            {
                return (T)GetUI(name);
            }

            /// <summary>
            /// 得到UI
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public UIBase GetUI(string name)
            {
                UIBase t = null;
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogErrorFormat("UI: name '{0}' is null or empty!", name);
                }
                else
                {
                    m_data.TryGetValue(name, out t);
                }
                return t;
            }

            /// <summary>
            /// 打开UI
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="param"></param>
            /// <returns></returns>
            public void OpenUI<T>(Param param = null) where T : UIBase
            {
                string name = typeof(T).ToString();

                OpenUI(name, param);
            }

            /// <summary>
            /// 打开UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="param"></param>
            public void OpenUI(string name, Param param = null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogErrorFormat("UI: name '{0}' is null or empty!", name);
                    return;
                }

                if (m_data.ContainsKey(name))
                {
                    UIBase t = m_data[name];
                    if (null != t)
                    {
                        if (t.show)
                        {
                            t.Update(param);
                        }
                        else
                        {
                            t.Open(param);
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("UI: '{0}' is not exist!", name);
                    }
                }
                else
                {
                    if (m_param.ContainsKey(name))
                    {
                        m_param[name] = param;
                    }
                    else
                    {
                        m_param.Add(name, param);
                        UnityEngine.Object o = Resources.Load(name);

                        {
                            GameObject go = GameObject.Instantiate(o) as GameObject;
                            go.name = go.name.Replace("(Clone)", "");
                            go.transform.parent = m_root;
                            go.transform.localPosition = Vector3.zero;
                            go.transform.localScale = Vector3.one;
                            go.transform.localRotation = Quaternion.identity;
                            UIBase t = go.GetComponent<UIBase>();
                            if (null != t)
                            {
                                m_data.Add(name, t);
                                t.Open(m_param.ContainsKey(name) ? m_param[name] : null);
                            }
                            else
                            {
                                Debug.LogErrorFormat("UI: '{0}' is not find!", name);
                            }
                            m_param.Remove(name);
                        }
                    }
                }
            }

            /// <summary>
            /// 仅打开一个UI,并且隐藏其它显示的UI
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="param"></param>
            public void OpenOneUI<T>(Param param = null) where T : UIBase
            {
                string name = typeof(T).ToString();

                OpenOneUI(name, param);
            }

            /// <summary>
            /// 仅打开一个UI,并且隐藏其它显示的UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="param"></param>
            public void OpenOneUI(string name, Param param = null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogErrorFormat("UI: name '{0}' is null or empty!", name);
                    return;
                }

                Action<Param> action = (o) => {
                    List<string> list = o["show"] as List<string>;
                    if (!list.Contains(name))
                    {
                        this.OpenUI(name, param);
                    }
                };

                Param p = Param.Create();
                p.Add("closeall", action);
                p.Add("filter", new List<string>() { name });
                CloseAllUI(p);
            }

            /// <summary>
            /// 更新UI
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="param"></param>
            public void UpdateUI<T>(Param param = null) where T : UIBase
            {
                string name = typeof(T).ToString();

                UpdateUI(name, param);
            }

            /// <summary>
            /// 更新UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="param"></param>
            public void UpdateUI(string name, Param param = null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogErrorFormat("UI: name '{0}' is null or empty!", name);
                    return;
                }

                if (m_data.ContainsKey(name))
                {
                    UIBase t = m_data[name];
                    if (null != t)
                    {
                        t.Update(param);
                    }
                    else
                    {
                        Debug.LogErrorFormat("UI: '{0}' is not exist!", name);
                    }
                }
            }

            /// <summary>
            /// 关闭UI
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="param"></param>
            public void CloseUI<T>(Param param = null) where T : UIBase
            {
                string name = typeof(T).ToString();

                CloseUI(name, param);
            }

            /// <summary>
            /// 关闭UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="param"></param>
            public void CloseUI(string name, Param param = null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogErrorFormat("UI: name '{0}' is null or empty!", name);
                    return;
                }

                if (m_data.ContainsKey(name))
                {
                    UIBase t = m_data[name];
                    if (null != t)
                    {
                        if (t.show)
                        {
                            t.Close(param);
                            if (!t.cache)
                            {
                                GameObject.Destroy(t.gameObject);
                                m_data.Remove(name);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("UI: '{0}' is not exist!", name);
                    }
                }
            }

            /// <summary>
            /// 关闭所有显示的UI
            /// </summary>
            /// <param name="param"></param>
            public void CloseAllUI(Param param = null)
            {
                Action closeAll = param["closeall"] as Action;
                Action<Param> closeAllWithParam = param["closeall"] as Action<Param>;
                List<string> filter = param["filter"] as List<string>;
                Param.Destroy(param);

                int showNum = 0;
                List<UIBase> list = new List<UIBase>();
                List<string> filterList = new List<string>();
                foreach (var t in m_data.Values)
                {
                    if (t.show)
                    {
                        if (filter.Contains(t.getName))
                        {
                            filterList.Add(t.getName);
                            continue;
                        }
                        else
                        {
                            list.Add(t);
                        }
                    }
                }
                showNum = list.Count;

                Param p = Param.Create();
                Action close = () => {
                    --showNum;
                    if (showNum == 0)
                    {
                        if (null != closeAll)
                        {
                            closeAll();
                        }
                        else if (null != closeAllWithParam)
                        {
                            p = Param.Create();
                            p.Add("show", filterList);
                            closeAllWithParam(p);
                        }
                    }
                };
                p.Add("close", close);
                for (int i = 0; i < list.Count; ++i)
                {
                    list[i].Close(p);
                    if (!list[i].cache)
                    {
                        GameObject.Destroy(list[i].gameObject);
                        m_data.Remove(list[i].getName);
                    }
                }
            }

            /// <summary>
            /// 设置同对象中的顺序
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="index"></param>
            public void SetSiblingIndex<T>(int index) where T : UIBase
            {
                string name = typeof(T).ToString();

                SetSiblingIndex(name, index);
            }

            /// <summary>
            /// 设置同对象中的顺序
            /// </summary>
            /// <param name="name"></param>
            /// <param name="index"></param>
            public void SetSiblingIndex(string name, int index)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogErrorFormat("UI: name '{0}' is null or empty!", name);
                    return;
                }

                if (m_data.ContainsKey(name))
                {
                    UIBase t = m_data[name];
                    if (null != t)
                    {
                        t.SetSiblingIndex(index);
                    }
                    else
                    {
                        Debug.LogErrorFormat("UI: '{0}' is not exist!", name);
                    }
                }
            }

            /// <summary>
            /// 设置UI在同父对象下顺序
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="names">排序在姐妹对象前面</param>
            public void SetSiblingIndex<T>(params string[] names) where T : UIBase
            {
                string name = typeof(T).ToString();

                SetSiblingIndex(name, names);
            }

            /// <summary>
            /// 设置UI在同父对象下顺序
            /// </summary>
            /// <param name="name"></param>
            /// <param name="names">排序在姐妹对象前面</param>
            public void SetSiblingIndex(string name, params string[] names)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogErrorFormat("UI: name '{0}' is null or empty!", name);
                    return;
                }

                int index = 0;
                UIBase t = GetUI(name);
                if (null != t)
                {
                    index = t.siblingIndex;
                }
                else
                {
                    Debug.LogErrorFormat("UI: '{0}' is not exist!", name);
                    return;
                }


                for (int i = 0; i < names.Length; ++i)
                {
                    t = GetUI(names[i]);
                    if (null != t && index > t.siblingIndex)
                    {
                        index = t.siblingIndex;
                    }
                }

                m_data.TryGetValue(name, out t);
                t.SetSiblingIndex(--index);
            }

            /// <summary>
            /// 清理数据
            /// </summary>
            public override void Clear()
            {
                base.Clear();

                foreach (var t in m_data)
                {
                    if (null != t.Value)
                    {
                        t.Value.Close();
                        GameObject.DestroyImmediate(t.Value.gameObject);
                    }
                }
                m_data.Clear();
                m_param.Clear();
            }
            #endregion
        }
    }
}
