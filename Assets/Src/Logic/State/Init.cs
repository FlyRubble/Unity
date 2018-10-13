using UnityEngine;
using System.IO;
using UnityAsset;
using System.Collections.Generic;

namespace Framework
{
    using UI;
    using IO;
    using Event;
    using JsonFx;
    /// <summary>
    /// 状态
    /// </summary>
    public class Init : State
    {
        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);

            // 选择沙盒路径还是流式路径
            bool sandbox = App.version.Equals(PlayerPrefs.GetString(Const.SANDBOX_VERSION));
            AssetManager.instance.url = sandbox ? App.persistentDataPath : App.streamingAssetsPath;
            // 加载Loading界面
            UIManager.instance.OpenUI(Const.UI_LOADING);

            // 检测网络是否开启
            if (App.internetReachability)
            {
                // 检测版本更新
                StateMachine.instance.OnEnter(new VersionUpdate());
            }
            else
            {
                // 网络不可达，退出
                Action action = () => { Helper.Msg("网络不可达，请打开网络再试，现在需要退出"); };
                UIManager.instance.OpenUI(Const.UI_NORMAL_TIPS_BOX, Param.Create(new object[] { UINormalTipsBox.SURE_ACTION, action, UINormalTipsBox.TEXT_CONTENT, "网络不可达，请打开网络再试，现在需要退出" }));
            }
        }
        #endregion
    }
}