using UnityEngine;
using System.IO;
using UnityAsset;
using System.Collections.Generic;

namespace Framework
{
    using UI;
    using IO;
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

            // 是否需要解压
            bool bExists = File.Exists(App.assetPath + Const.MANIFESTFILE);
            if (bExists)
            {
                StateMachine.instance.OnEnter(new CheckUpdate());
            }
            else
            {
                StateMachine.instance.OnEnter(new Unzip());
            }
        }
        #endregion
    }
}