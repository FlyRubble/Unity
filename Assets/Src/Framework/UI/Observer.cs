using System.Collections.Generic;

namespace Framework
{
    using Singleton;
    /// <summary>
    /// 观察者对象
    /// </summary>
    public sealed class Observer : Singleton<Observer>
    {
        #region Variable
        /// <summary>
        /// 对象表
        /// </summary>
        private Dictionary<string, IObserver> m_data = null;
        #endregion

        #region Function
        /// <summary>
        /// 构造
        /// </summary>
        public Observer()
        {
            m_data = new Dictionary<string, IObserver>();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="obs"></param>
        public void Register(IObserver obs)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (m_data.ContainsKey(obs.getName)) return;

            m_data.Add(obs.getName, obs);
            foreach (var name in obs.observerSelf)
            {
                var data = m_data.ContainsKey(name) ? m_data[name] : null;
                if (null != data && !data.selfObserver.Contains(obs.getName))
                {
                    data.selfObserver.Add(obs.getName);
                }
            }
            foreach (var name in obs.selfObserver)
            {
                var data = m_data.ContainsKey(name) ? m_data[name] : null;
                if (null != data && !data.observerSelf.Contains(obs.getName))
                {
                    data.observerSelf.Add(obs.getName);
                }
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="obs"></param>
        public void UnRegister(IObserver obs)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;

            m_data.Remove(obs.getName);
            foreach (var name in obs.observerSelf)
            {
                var data = m_data.ContainsKey(name) ? m_data[name] : null;
                if (null != data && data.selfObserver.Contains(obs.getName))
                {
                    data.selfObserver.Remove(obs.getName);
                }
            }
            foreach (var name in obs.selfObserver)
            {
                var data = m_data.ContainsKey(name) ? m_data[name] : null;
                if (null != data && data.observerSelf.Contains(obs.getName))
                {
                    data.observerSelf.Remove(obs.getName);
                }
            }
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        public void AddNotification(IObserver obs, string name)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (string.IsNullOrEmpty(name)) return;

            if (m_data.ContainsKey(obs.getName) && m_data.ContainsKey(name))
            {
                if (!obs.selfObserver.Contains(name))
                {
                    obs.selfObserver.Add(name);
                }
                var data = m_data[name];
                if (!data.observerSelf.Contains(obs.getName))
                {
                    data.observerSelf.Add(obs.getName);
                }
            }
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="names"></param>
        public void AddNotification(IObserver obs, string[] names)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (null == names) return;

            for (int i = 0; i < names.Length; ++i)
            {
                string name = names[i];
                if (string.IsNullOrEmpty(name)) continue;
                if (m_data.ContainsKey(obs.getName) && m_data.ContainsKey(name))
                {
                    if (!obs.selfObserver.Contains(name))
                    {
                        obs.selfObserver.Add(name);
                    }
                    var data = m_data[name];
                    if (!data.observerSelf.Contains(obs.getName))
                    {
                        data.observerSelf.Add(obs.getName);
                    }
                }
            }
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="names"></param>
        public void AddNotification(IObserver obs, List<string> names)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (null == names) return;

            for (int i = 0; i < names.Count; ++i)
            {
                string name = names[i];
                if (string.IsNullOrEmpty(name)) continue;
                if (m_data.ContainsKey(obs.getName) && m_data.ContainsKey(name))
                {
                    if (!obs.selfObserver.Contains(name))
                    {
                        obs.selfObserver.Add(name);
                    }
                    var data = m_data[name];
                    if (!data.observerSelf.Contains(obs.getName))
                    {
                        data.observerSelf.Add(obs.getName);
                    }
                }
            }
        }

        /// <summary>
        /// 插入通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        public void InsertNotification(IObserver obs, string name, int index = 0)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (string.IsNullOrEmpty(name)) return;

            if (m_data.ContainsKey(obs.getName) && m_data.ContainsKey(name))
            {
                if (!obs.selfObserver.Contains(name))
                {
                    obs.selfObserver.Insert(index, name);
                }
                var data = m_data[name];
                if (!data.observerSelf.Contains(obs.getName))
                {
                    data.observerSelf.Add(obs.getName);
                }
            }
        }

        /// <summary>
        /// 插入通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="names"></param>
        /// <param name="index"></param>
        public void InsertNotification(IObserver obs, string[] names, int index = 0)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (null == names) return;

            for (int i = 0; i < names.Length; ++i)
            {
                string name = names[i];
                if (string.IsNullOrEmpty(name)) continue;
                if (m_data.ContainsKey(obs.getName) && m_data.ContainsKey(name))
                {
                    if (!obs.selfObserver.Contains(name))
                    {
                        obs.selfObserver.Insert(index, name);
                    }
                    var data = m_data[name];
                    if (!data.observerSelf.Contains(obs.getName))
                    {
                        data.observerSelf.Add(obs.getName);
                    }
                }
            }
        }

        /// <summary>
        /// 插入通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="names"></param>
        /// <param name="index"></param>
        public void InsertNotification(IObserver obs, List<string> names, int index = 0)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (null == names) return;

            for (int i = 0; i < names.Count; ++i)
            {
                string name = names[i];
                if (string.IsNullOrEmpty(name)) continue;
                if (m_data.ContainsKey(obs.getName) && m_data.ContainsKey(name))
                {
                    if (!obs.selfObserver.Contains(name))
                    {
                        obs.selfObserver.Insert(index, name);
                    }
                    var data = m_data[name];
                    if (!data.observerSelf.Contains(obs.getName))
                    {
                        data.observerSelf.Add(obs.getName);
                    }
                }
            }
        }

        /// <summary>
        /// 移除通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        public void RemoveNotification(IObserver obs, string name)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (null == name) return;

            if (m_data.ContainsKey(obs.getName) && m_data.ContainsKey(name))
            {
                if (obs.selfObserver.Contains(name))
                {
                    obs.selfObserver.Remove(name);
                }
                var data = m_data[name];
                if (data.observerSelf.Contains(obs.getName))
                {
                    data.observerSelf.Remove(obs.getName);
                }
            }
        }


        /// <summary>
        /// 移除通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="names"></param>
        public void RemoveNotification(IObserver obs, string[] names)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (null == names) return;

            for (int i = 0; i < names.Length; ++i)
            {
                string name = names[i];
                if (m_data.ContainsKey(obs.getName) && m_data.ContainsKey(name))
                {
                    if (obs.selfObserver.Contains(name))
                    {
                        obs.selfObserver.Remove(name);
                    }
                    var data = m_data[name];
                    if (data.observerSelf.Contains(obs.getName))
                    {
                        data.observerSelf.Remove(obs.getName);
                    }
                }
            }
        }

        /// <summary>
        /// 移除通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="names"></param>
        public void RemoveNotification(IObserver obs, List<string> names)
        {
            if (null == obs) return;
            if (string.IsNullOrEmpty(obs.getName)) return;
            if (null == names) return;

            for (int i = 0; i < names.Count; ++i)
            {
                string name = names[i];
                if (m_data.ContainsKey(obs.getName) && m_data.ContainsKey(name))
                {
                    if (obs.selfObserver.Contains(name))
                    {
                        obs.selfObserver.Remove(name);
                    }
                    var data = m_data[name];
                    if (data.observerSelf.Contains(obs.getName))
                    {
                        data.observerSelf.Remove(obs.getName);
                    }
                }
            }
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="name"></param>
        /// <param name="param"></param>
        public void Notification(string name, Param param)
        {
            if (string.IsNullOrEmpty(name)) return;

            if (m_data.ContainsKey(name))
            {
                m_data[name].OnNotification(param);
            }
        }

        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="name"></param>
        /// <param name="param"></param>
        public void Broadcast(string name, Param param)
        {
            if (string.IsNullOrEmpty(name)) return;

            if (m_data.ContainsKey(name))
            {
                var obs = m_data[name];
                for (int i = 0; i < obs.selfObserver.Count; ++i)
                {
                    if (m_data.ContainsKey(obs.selfObserver[i]))
                    {
                        m_data[obs.selfObserver[i]].OnNotification(param);
                    }
                }
            }
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            m_data.Clear();
        }
        #endregion
    }
}