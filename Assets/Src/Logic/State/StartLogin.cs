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
    public class StartLogin : State
    {
        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);
            
            Lua.instance.OnStart();
        }
        #endregion
    }
}