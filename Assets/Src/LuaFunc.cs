using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Event;
using UnityAsset;
using XLua;

[LuaCallCSharp]
public class LuaFunc
{

    public static float Test()
    {
        return Time.realtimeSinceStartup;
    }


    public static AsyncAsset AssetBundleAsyncLoad(string path, Action<bool, AsyncAsset> action, Dictionary<string, AsyncAsset> dic = null)
    {
        return AssetManager.instance.AssetBundleAsyncLoad(path, action, dic);
    }
}
