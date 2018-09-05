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
        private Dictionary<string, IObserver> m_oData = null;

        /// <summary>
        /// 对象通知表
        /// </summary>
        private Dictionary<string, List<IObserver>> m_nData = null;
        #endregion

        #region Function
        /// <summary>
        /// 构造
        /// </summary>
        public Observer()
        {
            m_oData = new Dictionary<string, IObserver>();
            m_nData = new Dictionary<string, List<IObserver>>();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="obs"></param>
        public void Register(IObserver obs)
        {
            if (null == obs) return;

            if (string.IsNullOrEmpty(obs.getName)) return;
            if (m_oData.ContainsKey(obs.getName))
            {
                m_oData[obs.getName] = obs;
            }
            else
            {
                m_oData.Add(obs.getName, obs);
            }
            
            for (int i = 0; i < obs.nName.Count; ++i)
            {
                if (string.IsNullOrEmpty(obs.nName[i])) continue;
                if (!m_nData.ContainsKey(obs.nName[i]))
                {
                    m_nData.Add(obs.nName[i], new List<IObserver>());
                }
                m_nData[obs.nName[i]].Add(obs);
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="obs"></param>
        public void UnRegister(IObserver obs)
        {
            if (null == obs) return;
            m_oData.Remove(obs.getName);
            
            for (int i = 0; i < obs.nName.Count; ++i)
            {
                if (m_nData.ContainsKey(obs.nName[i]))
                {
                    m_nData[obs.nName[i]].Remove(obs);
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

            if (string.IsNullOrEmpty(name)) return;
            if (!m_nData.ContainsKey(name))
            {
                m_nData.Add(name, new List<IObserver>());
            }
            m_nData[name].Add(obs);
            
            if (!obs.nName.Contains(name))
            {
                obs.nName.Add(name);
            }
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        public void AddNotification(IObserver obs, string[] name)
        {
            if (null == obs) return;

            if (null == name) return;
            for (int i = 0; i < name.Length; ++i)
            {
                if (string.IsNullOrEmpty(name[i])) continue;
                if (!m_nData.ContainsKey(name[i]))
                {
                    m_nData.Add(name[i], new List<IObserver>());
                }
                m_nData[name[i]].Add(obs);

                if (!obs.nName.Contains(name[i]))
                {
                    obs.nName.Add(name[i]);
                }
            }
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        public void AddNotification(IObserver obs, List<string> name)
        {
            if (null == obs) return;

            if (null == name) return;
            for (int i = 0; i < name.Count; ++i)
            {
                if (string.IsNullOrEmpty(name[i])) continue;
                if (!m_nData.ContainsKey(name[i]))
                {
                    m_nData.Add(name[i], new List<IObserver>());
                }
                m_nData[name[i]].Add(obs);

                if (!obs.nName.Contains(name[i]))
                {
                    obs.nName.Add(name[i]);
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

            if (string.IsNullOrEmpty(name)) return;
            if (!m_nData.ContainsKey(name))
            {
                m_nData.Add(name, new List<IObserver>());
            }
            m_nData[name].Insert(index, obs);

            if (!obs.nName.Contains(name))
            {
                obs.nName.Insert(index, name);
            }
        }

        /// <summary>
        /// 插入通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        public void InsertNotification(IObserver obs, string[] name, int index = 0)
        {
            if (null == obs) return;

            if (null == name) return;
            for (int i = name.Length - 1; i >= 0; --i)
            {
                if (string.IsNullOrEmpty(name[i])) continue;
                if (!m_nData.ContainsKey(name[i]))
                {
                    m_nData.Add(name[i], new List<IObserver>());
                }
                m_nData[name[i]].Insert(index, obs);

                if (!obs.nName.Contains(name[i]))
                {
                    obs.nName.Insert(index, name[i]);
                }
            }
        }

        /// <summary>
        /// 插入通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        public void InsertNotification(IObserver obs, List<string> name, int index = 0)
        {
            if (null == obs) return;

            if (null == name) return;
            for(int i = name.Count - 1; i >= 0; --i)
            {
                if (string.IsNullOrEmpty(name[i])) continue;
                if (!m_nData.ContainsKey(name[i]))
                {
                    m_nData.Add(name[i], new List<IObserver>());
                }
                m_nData[name[i]].Insert(index, obs);

                if (!obs.nName.Contains(name[i]))
                {
                    obs.nName.Insert(index, name[i]);
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

            if (string.IsNullOrEmpty(name)) return;
            if (m_nData.ContainsKey(name))
            {
                m_nData[name].Remove(obs);
            }
            
            obs.nName.Remove(name);
        }


        /// <summary>
        /// 移除通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        public void RemoveNotification(IObserver obs, string[] name)
        {
            if (null == obs) return;

            if (null == name) return;
            for (int i = 0; i < name.Length; ++i)
            {
                if (string.IsNullOrEmpty(name[i])) continue;
                if (m_nData.ContainsKey(name[i]))
                {
                    m_nData[name[i]].Remove(obs);
                }
                obs.nName.Remove(name[i]);
            }
        }

        /// <summary>
        /// 移除通知
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="name"></param>
        public void RemoveNotification(IObserver obs, List<string> name)
        {
            if (null == obs) return;

            if (null == name) return;
            for (int i = 0; i < name.Count; ++i)
            {
                if (string.IsNullOrEmpty(name[i])) continue;
                if (m_nData.ContainsKey(name[i]))
                {
                    m_nData[name[i]].Remove(obs);
                }
                obs.nName.Remove(name[i]);
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

            if (m_oData.ContainsKey(name))
            {
                m_oData[name].OnNotification(param);
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

            if (m_nData.ContainsKey(name))
            {
                var obs = m_nData[name];
                for (int i = 0; i < obs.Count; ++i)
                {
                    if (null == obs[i]) continue;
                    obs[i].OnNotification(param);
                }
            }
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public override void Clear()
        {
            base.Clear();

            m_oData.Clear();
            m_nData.Clear();
        }
        #endregion
    }
}