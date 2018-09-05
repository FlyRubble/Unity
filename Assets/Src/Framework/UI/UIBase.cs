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
            [SerializeField] public List<GameObject> m_list;

            /// <summary>
            /// Canvas设置层级后跟显示相关
            /// </summary>
            protected Canvas m_canvas = null;

            /// <summary>
            /// GraphicRaycaster图像射线碰撞相关
            /// </summary>
            protected GraphicRaycaster m_graphicRaycaster = null;

            /// <summary>
            /// 通知表(监听对象表)
            /// </summary>
            List<string> m_nName = new List<string>() { };

            /// <summary>
            /// 参数
            /// </summary>
            protected Param m_param = null;

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
            /// 通知表
            /// </summary>
            public List<string> nName
            {
                get { return m_nName; }
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
            /// <summary>
            /// 启动
            /// </summary>
            protected virtual void Awake()
            {
                gameObject.SetActive(true);
                m_canvas = gameObject.AddComponent<Canvas>();
                m_graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();

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
            /// 打开
            /// </summary>
            /// <param name="param"></param>
            public void Open(Param param = null)
            {
                if (null == param)
                {
                    param = Param.Create();
                }
                Action action = param["open"] as Action;
                Action open = () => {
                    this.Show();
                    if (null != action)
                    {
                        action();
                    }
                };
                param.Add("open", open);
                param.Remove("update");
                param.Remove("close");
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
                param.TryAdd("update", null);
                param.Remove("open");
                param.Remove("close");
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
                Action action = param["close"] as Action;
                Action close = () => {
                    this.Hide();
                    if (null != action)
                    {
                        action();
                    }
                };
                param.Add("close", close);
                param.Remove("open");
                param.Remove("update");
                OnNotification(param);
            }

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
                } break;
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

                if (m_param.Contain("close"))
                {
                    Action close = m_param["close"] as Action;
                    if (null != close)
                    {
                        close();
                    }
                }
                else if (m_param.Contain("open"))
                {
                    Action open = m_param["open"] as Action;
                    if (null != open)
                    {
                        open();
                    }
                }
            }
            #endregion
        }
    }
}