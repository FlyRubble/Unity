using System.Collections.Generic;
using UnityEngine;
using Framework.Event;
using UnityAsset;

public class LuaCallCS
{
    /// <summary>
    /// 初始化全局时间
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, object> Init()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("AssetManager", AssetManager.instance);

        return data;
    }

    /// <summary>
    /// AssetBundle资源加载
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static AsyncAsset AssetBundleLoad(string path)
    {
        return AssetManager.instance.AssetBundleLoad(path);
    }

    /// <summary>
    /// AssetBundle异步加载
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static AsyncAsset AssetBundleAsyncLoad(string path, Action<bool, AsyncAsset> action, Dictionary<string, AsyncAsset> dic = null)
    {
        return AssetManager.instance.AssetBundleAsyncLoad(path, action, dic);
    }
}
