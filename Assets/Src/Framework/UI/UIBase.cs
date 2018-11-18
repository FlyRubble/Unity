using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Framework
{
    using Event;
    namespace UI
    {
        /// <summary>
        /// UIBase
        /// </summary>
        public class UIBase : MonoBehaviour, IObserver
        {
            #region Const
            /// <summary>
            /// 打开
            /// </summary>
            public const string OPEN = "open";

            /// <summary>
            /// 刷新
            /// </summary>
            public const string REFRESH = "refresh";

            /// <summary>
            /// 关闭
            /// </summary>
            public const string CLOSE = "close";

            const string AWAKE = "awake";
            const string ONENABLE = "onEnable";
            const string START = "start";
            const string FIXEDUPDATE = "fixedUpdate";
            const string UPDATE = "update";
            const string LATEUPDATE = "lateUpdate";
            const string ONDISABLE = "onDisable";
            const string ONDESTROY = "onDestroy";
            #endregion

            #region Variable
            /// <summary>
            /// 缓存
            /// </summary>
            [Tooltip("是否需要缓存")]
            [SerializeField] protected bool m_cache = false;

            /// <summary>
            /// 层选项
            /// </summary>
            [Tooltip("层选项")]
            [SerializeField] protected Sibling m_sibling = Sibling.First;

            /// <summary>
            /// 层级顺序
            /// </summary>
            [Tooltip("显示层级顺序")]
            [SerializeField] protected int m_sortOrder = 0;

            /// <summary>
            /// 需要操作的GameObject列表
            /// </summary>
            [Header("需要操作的GameObject列表")]
            [SerializeField] public List<GameObject> m_list = new List<GameObject>();

            /// <summary>
            /// Canvas设置层级后跟显示相关
            /// </summary>
            protected Canvas m_canvas = null;

            /// <summary>
            /// GraphicRaycaster图像射线碰撞相关
            /// </summary>
            protected GraphicRaycaster m_graphicRaycaster = null;

            /// <summary>
            /// 观察自己的对象表
            /// </summary>
            private List<string> m_observerSelf = new List<string>() { };

            /// <summary>
            /// 自己观察的对象表
            /// </summary>
            private List<string> m_selfObserver = new List<string>() { };

            /// <summary>
            /// 普通参数
            /// </summary>
            protected Param m_param = null;

            /// <summary>
            /// 组件事件
            /// </summary>
            private Dictionary<string, Action> m_event = null;

            /// <summary>
            /// 是否显示可见
            /// </summary>
            protected bool m_show = false;
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
            /// 观察自己的对象表
            /// </summary>
            public virtual List<string> observerSelf
            {
                get { return m_observerSelf; }
            }

            /// <summary> 
            /// 自己观察的对象表
            /// </summary>
            public virtual List<string> selfObserver
            {
                get { return m_selfObserver; }
            }
            
            /// <summary>
            /// 是否需要缓存
            /// </summary>
            public bool cache
            {
                get { return m_cache; }
            }

            /// <summary>
            /// 对象顺序索引
            /// </summary>
            public int siblingIndex
            {
                get { return transform.GetSiblingIndex(); }
            }

            /// <summary>
            /// 当前是否显示可见
            /// </summary>
            public bool show
            {
                get { return m_show; }
            }
            #endregion

            #region Function
            #region Component
            /// <summary>
            /// 启动
            /// </summary>
            protected virtual void Awake()
            {
                gameObject.SetActive(true);
                m_canvas = gameObject.AddComponent<Canvas>();
                m_graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();

                Observer.instance.Register(this);

                if (m_event.ContainsKey(AWAKE) && null != m_event[AWAKE])
                {
                    m_event[AWAKE]();
                }
            }

            /// <summary>
            /// 激活
            /// </summary>
            protected virtual void OnEnable()
            {
                if (m_event.ContainsKey(ONENABLE) && null != m_event[ONENABLE])
                {
                    m_event[ONENABLE]();
                }
            }

            /// <summary>
            /// 开始
            /// </summary>
            protected virtual void Start()
            {
                if (m_event.ContainsKey(ONENABLE) && null != m_event[ONENABLE])
                {
                    m_event[ONENABLE]();
                }
            }

            /// <summary>
            /// 物理更新
            /// </summary>
            protected virtual void FixedUpdate()
            {
                if (m_event.ContainsKey(FIXEDUPDATE) && null != m_event[FIXEDUPDATE])
                {
                    m_event[FIXEDUPDATE]();
                }
            }

            /// <summary>
            /// 更新
            /// </summary>
            protected virtual void Update()
            {
                if (m_event.ContainsKey(UPDATE) && null != m_event[UPDATE])
                {
                    m_event[UPDATE]();
                }
            }

            /// <summary>
            /// 延迟更新
            /// </summary>
            protected virtual void LateUpdate()
            {
                if (m_event.ContainsKey(LATEUPDATE) && null != m_event[LATEUPDATE])
                {
                    m_event[LATEUPDATE]();
                }
            }

            /// <summary>
            /// 隐藏
            /// </summary>
            protected virtual void OnDisable()
            {
                if (m_event.ContainsKey(ONDISABLE) && null != m_event[ONDISABLE])
                {
                    m_event[ONDISABLE]();
                }
            }

            /// <summary>
            /// 销毁
            /// </summary>
            protected virtual void OnDestroy()
            {
                Param.Destroy(m_param);
                if (m_event.ContainsKey(ONDESTROY) && null != m_event[ONDESTROY])
                {
                    m_event[ONDESTROY]();
                }
                m_event.Clear();
                Observer.instance.UnRegister(this);
            }
            #endregion

            /// <summary>
            /// 显示
            /// </summary>
            void Show()
            {
                m_show = true;
                SetLayer(transform, (int)Layers.UI);
                m_graphicRaycaster.enabled = true;
                switch (m_sibling)
                {
                case Sibling.First:
                {
                    transform.SetAsFirstSibling();
                }
                break;
                case Sibling.Last:
                {
                    transform.SetAsLastSibling();
                }
                break;
                case Sibling.Custom:
                {
                    transform.SetSiblingIndex(m_sortOrder);
                }
                break;
                }
            }

            /// <summary>
            /// 隐藏
            /// </summary>
            void Hide()
            {
                m_show = false;
                SetLayer(transform, (int)Layers.HIDE);
                m_graphicRaycaster.enabled = false;
            }

            /// <summary>
            /// 设置层
            /// </summary>
            /// <param name="tf"></param>
            /// <param name="layer"></param>
            void SetLayer(Transform tf, int layer)
            {
                tf.gameObject.layer = layer;
            }

            /// <summary>
            /// 设置同对象中的顺序
            /// </summary>
            /// <param name="index"></param>
            public void SetSiblingIndex(int index)
            {
                transform.SetSiblingIndex(m_sortOrder);
            }

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
            public virtual void OnNotification(Param param = null)
            {
                Param.Destroy(m_param);
                m_param = param;
                
                if (m_param.Contain(REFRESH))
                {
                    Action update = m_param[REFRESH] as Action;
                    if (null != update)
                    {
                        update();
                    }
                }
                else if (m_param.Contain(CLOSE))
                {
                    Action close = m_param[CLOSE] as Action;
                    if (null != close)
                    {
                        close();
                    }
                }
                else if (m_param.Contain(OPEN))
                {
                    Action open = m_param[OPEN] as Action;
                    if (null != open)
                    {
                        open();
                    }
                }
            }


            /// <summary>
            /// 打开
            /// </summary>
            /// <param name="param"></param>
            public void Open(Param param = null)
            {
                if (null == m_event)
                {
                    m_event = new Dictionary<string, Action>();
                    if (param.Contain(AWAKE)) m_event.Add(AWAKE, param[AWAKE] as Action);
                    if (param.Contain(START)) m_event.Add(START, param[START] as Action);
                    if (param.Contain(ONENABLE)) m_event.Add(ONENABLE, param[ONENABLE] as Action);
                    if (param.Contain(FIXEDUPDATE)) m_event.Add(FIXEDUPDATE, param[FIXEDUPDATE] as Action);
                    if (param.Contain(UPDATE)) m_event.Add(UPDATE, param[UPDATE] as Action);
                    if (param.Contain(LATEUPDATE)) m_event.Add(LATEUPDATE, param[LATEUPDATE] as Action);
                    if (param.Contain(ONDISABLE)) m_event.Add(ONDISABLE, param[ONDISABLE] as Action);
                    if (param.Contain(ONDESTROY)) m_event.Add(ONDESTROY, param[ONDESTROY] as Action);
                }
                if (null == param)
                {
                    param = Param.Create();
                }
                Action action = param[OPEN] as Action;
                Action open = () => {
                    this.Show();
                    if (null != action)
                    {
                        action();
                    }
                };
                param.Add(OPEN, open);
                param.Remove(REFRESH);
                param.Remove(CLOSE);
                OnNotification(param);
            }

            /// <summary>
            /// 更新
            /// </summary>
            /// <param name="param"></param>
            public void Update(Param param = null)
            {
                if (null == param)
                {
                    param = Param.Create();
                }
                param.TryAdd(REFRESH, null);
                param.Remove(OPEN);
                param.Remove(CLOSE);
                OnNotification(param);
            }

            /// <summary>
            /// 关闭
            /// </summary>
            /// <param name="param"></param>
            public void Close(Param param = null)
            {
                if (null == param)
                {
                    param = Param.Create();
                }
                Action action = param[CLOSE] as Action;
                Action close = () => {
                    this.Hide();
                    if (null != action)
                    {
                        action();
                    }
                };
                param.Add(CLOSE, close);
                param.Remove(OPEN);
                param.Remove(REFRESH);
                OnNotification(param);
            }
            #endregion
        }
    }
}