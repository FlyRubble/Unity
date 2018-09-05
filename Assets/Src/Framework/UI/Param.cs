using UnityEngine;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 参数
    /// </summary>
    public sealed class Param
    {
        #region Variable
        /// <summary>
        /// 参数表
        /// </summary>
        private Dictionary<object, object> m_param;

        /// <summary>
        /// 参数队列
        /// </summary>
        private static Queue<Param> m_data = new Queue<Param>();
        #endregion

        #region Property
        /// <summary>
        /// 参数大小
        /// </summary>
        public int Count
        {
            get
            {
                return m_param.Count;
            }
        }

        /// <summary>
        /// 得到参数Key
        /// </summary>
        public List<object> keys
        {
            get
            {
                return new List<object>(m_param.Keys);
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <returns></returns>
        public static Param Create()
        {
            Param p = null;
            if (m_data.Count > 0)
            {
                p = m_data.Dequeue();
                p.Clear();
            }
            else
            {
                p = new Param();
            }
            return p;
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Param Create(object[] param)
        {
            Param p = null;
            if (m_data.Count > 0)
            {
                p = m_data.Dequeue();
                p.Clear();
                p.Add(param);
            }
            else
            {
                p = new Param(param);
            }
            return p;
        }

        /// <summary>
        /// 销毁参数
        /// </summary>
        /// <param name="param"></param>
        public static void Destroy(Param param)
        {
            if (null == param) return;
            m_data.Enqueue(param);
        }

        /// <summary>
        /// 构造
        /// </summary>
        Param()
        {
            m_param = new Dictionary<object, object>();
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="param"></param>
        Param(object[] param)
        {
            m_param = new Dictionary<object, object>();
            Add(param);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(object name, object value)
        {
            if (m_param.ContainsKey(name))
            {
                m_param[name] = value;
            }
            else
            {
                m_param.Add(name, value);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        public void Add(object[] param)
        {
            for (int i = 1; i < param.Length; ++i, ++i)
            {
                Add(param[i - 1], param[i]);
            }
        }

        /// <summary>
        /// 尝试添加
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void TryAdd(object name, object value)
        {
            if (!m_param.ContainsKey(name))
            {
                m_param.Add(name, value);
            }
        }

        /// <summary>
        /// 尝试添加
        /// </summary>
        /// <param name="param"></param>
        public void TryAdd(object[] param)
        {
            for (int i = 1; i < param.Length; ++i, ++i)
            {
                TryAdd(param[i - 1], param[i]);
            }
        }

        /// <summary>
        /// 移除参数
        /// </summary>
        /// <param name="name"></param>
        public void Remove(object name)
        {
            if (m_param.ContainsKey(name))
            {
                m_param.Remove(name);
            }
        }

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contain(object name)
        {
            return m_param.ContainsKey(name);
        }

        /// <summary>
        /// 得到参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[object name]
        {
            get
            {
                return m_param.ContainsKey(name) ? m_param[name] : null;
            }
        }

        /// <summary>
        /// 清理参数
        /// </summary>
        public void Clear()
        {
            m_param.Clear();
        }
        #endregion
    }

}