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
            // 获取清单文件
            AsyncAsset asset = AssetManager.instance.AssetBundleLoad(AssetManager.instance.url + Const.MANIFESTFILE);
            if (asset != null && string.IsNullOrEmpty(asset.error))
            {
                App.manifest = JsonReader.Deserialize<ManifestConfig>(asset.mainAsset.ToString());
                AssetManager.instance.UnloadAssets(asset, true);
            }
            // 加载Loading界面
            UIManager.instance.OpenUI(Const.UI_LOADING, Param.Create(new object[] { UILoading.TEXT_TIPS, Const.ID_GETING, UILoading.SLIDER, 0F, UILoading.TEXT_DETAILS, string.Empty }));
            // 检测网络是否开启
            InternetReachability();
        }

        /// <summary>
        /// 检测网络是否开启
        /// </summary>
        private void InternetReachability()
        {
            // 检测网络是否开启
            if (App.internetReachability)
            {
                // 检测版本更新
                StateMachine.instance.OnEnter(new VersionUpdate());
            }
            else
            {
                // 网络不可达，退出
                Action sure = () => {
                    UIManager.instance.CloseUI(Const.UI_NORMAL_TIPS_BOX);
                    Application.Quit();
                };
                Action close = () => {
                    InternetReachability();
                };
                UIManager.instance.OpenUI(Const.UI_NORMAL_TIPS_BOX, Param.Create(new object[] { UINormalTipsBox.ACTION_SURE, sure, UINormalTipsBox.TEXT_CONTENT, Const.ID_NETWORK_INVALID, UINormalTipsBox.ACTION_CLOSE, close }));
            }
        }
        #endregion
    }
}