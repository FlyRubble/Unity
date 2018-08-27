using UnityEngine;
using System.Collections.Generic;

namespace Framework
{
    namespace UI
    {
        /// <summary>
        /// UIBase
        /// </summary>
        public class UIBase : MonoBehaviour, IObserver
        {
            #region Variable
            /// <summary>
            /// 缓存
            /// </summary>
            [Tooltip("是否需要缓存")]
            [SerializeField] protected bool m_cache = false;

            /// <summary>
            /// 层级顺序
            /// </summary>
            [Tooltip("显示层级顺序")]
            [SerializeField] protected int m_sortOrder = 0;

            /// <summary>
            /// 默认是否激活
            /// </summary>
            [Tooltip("默认是否激活")]
            [SerializeField] protected bool m_defaultActive = true;

            /// <summary>
            /// 需要操作的GameObject列表
            /// </summary>
            [Header("需要操作的GameObject列表")]
            [SerializeField] public List<GameObject> m_list;

            /// <summary>
            /// 通知表(监听对象表)
            /// </summary>
            List<string> m_nName = new List<string>() { };
            #endregion

            #region Property
            /// <summary>
            /// 当前对象名字
            /// </summary>
            public virtual string getName
            {
                get { return this.name; }
            }

            /// <summary>
            /// 通知表
            /// </summary>
            public List<string> nName
            {
                get { return m_nName; }
            }
            #endregion

            #region Function
            /// <summary>
            /// 启动
            /// </summary>
            protected virtual void Awake()
            {
                Observer.instance.Register(this);
            }

            /// <summary>
            /// 激活
            /// </summary>
            protected virtual void OnEnable()
            {
                
            }

            /// <summary>
            /// 开始
            /// </summary>
            protected virtual void Start()
            {
                
            }

            /// <summary>
            /// 物理更新
            /// </summary>
            protected virtual void FixedUpdate()
            {
                
            }

            /// <summary>
            /// 更新
            /// </summary>
            protected virtual void Update()
            {
                
            }

            /// <summary>
            /// 延迟更新
            /// </summary>
            protected virtual void LateUpdate()
            {
                
            }

            /// <summary>
            /// 隐藏
            /// </summary>
            protected virtual void OnDisable()
            {
                
            }

            /// <summary>
            /// 销毁
            /// </summary>
            protected virtual void OnDestroy()
            {
                Observer.instance.UnRegister(this);
            }
            #region Component

            #endregion
            /// <summary>
            /// 通知
            /// </summary>
            /// <param name="name"></param>
            /// <param name="param"></param>
            public void Notification(string name, Param param)
            {
                Observer.instance.Notification(name, param);
            }

            /// <summary>
            /// 广播
            /// </summary>
            /// <param name="param"></param>
            public void Broadcast(Param param)
            {
                Observer.instance.Broadcast(this.getName, param);
            }

            /// <summary>
            /// 执行通知
            /// </summary>
            /// <param name="param"></param>
            public virtual void OnNotification(Param param)
            {
                
            }
            #endregion
        }
    }
}