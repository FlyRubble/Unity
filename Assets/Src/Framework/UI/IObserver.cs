using System.Collections.Generic;

namespace Framework
{
    public interface IObserver
    {
        /// <summary> 
        /// 名字
        /// </summary>
        string getName
        {
            get;
        }

        /// <summary>
        /// 观察自己的对象表
        /// </summary>
        List<string> observerSelf
        {
            get;
        }

        /// <summary> 
        /// 自己观察的对象表
        /// </summary>
        List<string> selfObserver
        {
            get;
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="name"></param>
        /// <param name="param"></param>
        void Notification(string name, Param param);

        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="param"></param>
        void Broadcast(Param param);

        /// <summary> 
        /// 处理通知
        /// </summary>
        void OnNotification(Param param);
    }
}