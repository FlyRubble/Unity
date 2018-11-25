using System.Collections.Generic;
using UnityEngine;
using Framework;
using Framework.Event;
using Framework.UI;
using UnityAsset;

public delegate void EventA(bool bResult, Object obj);

public class LuaCallCS
{
    #region UIManager
    /// <summary>
    /// 打开UI
    /// </summary>
    /// <param name="name"></param>
    /// <param name="param"></param>
    public static void OpenUI(string name, Param param = null)
    {
        UIManager.instance.OpenUI(name, param);
    }
    #endregion
}
