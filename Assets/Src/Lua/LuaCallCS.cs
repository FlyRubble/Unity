using System.Collections.Generic;
using UnityEngine;
using Framework;
using Framework.Event;
using Framework.UI;
using UnityAsset;

public class LuaCallCS
{
    #region Asset
    /// <summary>
    /// 卸载所有资源
    /// </summary>
    /// <param name="unloadAllLoadedObjects"></param>
    public static void AssetManager_UnloadAssets(bool unloadAllLoadedObjects)
    {
        AssetManager.instance.UnloadAssets(unloadAllLoadedObjects);
    }
    #endregion

    #region UIManager
    /// <summary>
    /// 打开UI
    /// </summary>
    /// <param name="name"></param>
    /// <param name="param"></param>
    public static void UIManager_OpenUI(string name, Param param = null)
    {
        UIManager.instance.OpenUI(name, param);
    }
    
    /// <summary>
    /// 清理UI
    /// </summary>
    public static void UIManager_Clear()
    {
        UIManager.instance.Clear();
    }

    /// <summary>
    /// 立即清理UI
    /// </summary>
    public static void UIManager_ClearImmediate()
    {
        UIManager.instance.ClearImmediate();
    }
    #endregion
}
