using UnityEngine;
using System.IO;
using UnityAsset;
using System.Collections.Generic;

namespace Framework
{
    using UI;
    using Event;
    using JsonFx;
    /// <summary>
    /// 状态
    /// </summary>
    public class VersionUpdate : State
    {
        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);

#if UNITY_EDITOR
            // 获取远程版本文件
            string remoteUrl = App.cdn + App.platform + string.Format(Const.REMOTE_VERSION, App.version);
            WWW www = new WWW(remoteUrl);
            while (!www.isDone) ;
            if (!string.IsNullOrEmpty(www.error))
            {
                // 获取版本信息失败
                Action action = () => { Helper.Msg("获取版本信息失败"); };
                UIManager.instance.OpenUI(Const.UI_NORMAL_TIPS_BOX, Param.Create(new object[] { UINormalTipsBox.SURE_ACTION, action, UINormalTipsBox.TEXT_CONTENT, "获取版本信息失败" }));
                return;
            }
            Dictionary<string, object> remoteVersion = JsonReader.Deserialize<Dictionary<string, object>>(www.text);
            string[] local = App.version.Split('.');
            string[] remote = remoteVersion[Const.VERSION].ToString().Split('.');
            bool forceUpdate = true;
            if (local.Length == remote.Length)
            {
                forceUpdate = !(int.Parse(local[0]) >= int.Parse(remote[0]) && int.Parse(local[1]) >= int.Parse(remote[1]));
            }
            if (forceUpdate)
            {
                // 需要更新版本
                Action action = () => { Helper.Msg("需要更新版本"); Application.OpenURL("http://baidu.com"); };
                UIManager.instance.OpenUI(Const.UI_NORMAL_TIPS_BOX, Param.Create(new object[] { UINormalTipsBox.SURE_ACTION, action, UINormalTipsBox.TEXT_CONTENT, "需要更新版本" }));
                return;
            }

            // 检测沙盒资源更新
            StateMachine.instance.OnEnter(new SandboxAssetUpdate());
#else

#endif
        }
        #endregion
    }
}