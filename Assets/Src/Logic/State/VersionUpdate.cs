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
        #region Variable
        /// <summary>
        /// WWW资源请求
        /// </summary>
        WWW m_www = null;
        #endregion

        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);
            
            // 获取远程版本文件
            string remoteUrl = Path.Combine(App.cdn + App.platform, string.Format(Const.REMOTE_VERSION, App.version));
            m_www = new WWW(remoteUrl);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (m_www != null && m_www.isDone)
            {
                bool forceUpdate = false;
                if (string.IsNullOrEmpty(m_www.error))
                {
                    Dictionary<string, object> remoteVersion = JsonReader.Deserialize<Dictionary<string, object>>(m_www.text);
                    if (remoteVersion != null)
                    {
                        App.Update(remoteVersion, new List<string>() { Const.VERSION });
                        Debugger.logEnabled = App.log;
                        Debugger.webLogEnabled = App.webLog;
                        string[] local = App.version.Split('.');
                        string[] remote = remoteVersion.ContainsKey(Const.VERSION) ? remoteVersion[Const.VERSION].ToString().Split('.') : new string[0];
                        if (local.Length == remote.Length)
                        {
                            forceUpdate = !(int.Parse(local[0]) >= int.Parse(remote[0]) && int.Parse(local[1]) >= int.Parse(remote[1]));
                        }
                    }

                }
                
                m_www.Dispose();
                m_www = null;

                if (forceUpdate)
                {
                    // 需要更新版本
                    Action sure = () => {
                        Application.OpenURL(App.newVersionDownloadUrl);
                    };
                    Action close = () => {
                        Application.Quit();
                    };
                    UIManager.instance.OpenUI(Const.UI_NORMAL_TIPS_BOX, Param.Create(new object[] {
                        UINormalTipsBox.ACTION_SURE, sure, UINormalTipsBox.TEXT_CONTENT, ConfigManager.GetLang("Version_Low"), UINormalTipsBox.ACTION_CLOSE, close }));
                }
                else
                {
                    // 检测沙盒资源解压
                    StateMachine.instance.OnEnter(new AssetDecompressing());
                }
            }
        }
        #endregion
    }
}