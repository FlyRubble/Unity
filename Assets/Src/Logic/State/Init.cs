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
            bool sandbox = App.version.Equals(Util.GetString(Const.SANDBOX_VERSION));
            AssetManager.instance.url = sandbox ? App.persistentDataPath : App.streamingAssetsPath;
            // 获取清单文件
            AsyncAsset asset = AssetManager.instance.AssetBundleLoad(AssetManager.instance.url + Const.MANIFESTFILE);
            if (asset != null && string.IsNullOrEmpty(asset.error))
            {
                App.manifest = JsonReader.Deserialize<ManifestConfig>(asset.mainAsset.ToString());
                AssetManager.instance.setDependentAsset = App.manifest.GetDependencies;
                AssetManager.instance.UnloadAssets(asset, true);
            }
            // 语言配置
            asset = AssetManager.instance.Load(Const.LANG_FILE, (bResult, obj) => {
                if (bResult && obj != null)
                {
                    ConfigManager.instance.langConfig.Init(obj.ToString());
                    AssetManager.instance.UnloadAssets(asset, true);
                }
            }, async: false);
            // 加载Loading界面
            UIManager.instance.OpenUI(Const.UI_LOADING, Param.Create(new object[] {
                UILoading.TEXT_TIPS, ConfigManager.GetLang("Asset_Request"), UILoading.SLIDER, 0F, UILoading.TEXT_DETAILS, string.Empty
            }));
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
                    Application.Quit();
                };
                Action close = () => {
                    InternetReachability();
                };
                UIManager.instance.OpenUI(Const.UI_NORMAL_TIPS_BOX, Param.Create(new object[] {
                    UINormalTipsBox.ACTION_SURE, sure, UINormalTipsBox.TEXT_CONTENT, ConfigManager.GetLang("Network_Invalid"), UINormalTipsBox.ACTION_CLOSE, close
                }));
            }
        }
        #endregion
    }
}