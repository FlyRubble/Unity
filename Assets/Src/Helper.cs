using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityAsset;
using Framework.Event;

public class Helper
{
    /// <summary>
    /// 写入字节到文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="bytes"></param>
    public static void WriteAllBytes(string path, byte[] bytes)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        File.WriteAllBytes(path, bytes);
    }

    /// <summary>
    /// 得到依赖资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="dic"></param>
    /// <param name="action"></param>
    public static void GetDependentAsset(string path, ref Dictionary<string, AsyncAsset> dic, Action<bool, AsyncAsset> action)
    {
        var data = App.manifest.Get(path);
        if (data != null)
        {
            foreach (var value in data.directDependencies)
            {
                GetDependentAsset(value, ref dic, action);
            }
            dic.Add(path, AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + path, action));
        }
    }

    /// <summary>
    /// 加载资源(自动加载依赖资源)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    public static void Load(string path, Action<bool, Object> action)
    {
        if (action != null)
        {
#if UNITY_EDITOR && !AB_MODE
            AssetManager.instance.Load(path, action);
#else
            var data = App.manifest.Get(path);
            if (data != null)
            {
                Dictionary<string, AsyncAsset> dic = new Dictionary<string, AsyncAsset>();
                int number = 0;
                Action<bool, AsyncAsset> complete = (bResult, asyncAsset) =>
                {
                    number++;
                    if (dic.Count == number)
                    {
                        asyncAsset = dic[path];
                        action(asyncAsset.mainAsset != null, asyncAsset.mainAsset);
                    }
                };
                GetDependentAsset(data.name, ref dic, complete);
            }
            else
            {
                action(false, null);
            }
#endif
        }
    }

    public static void Msg(string msg, bool quit = false)
    {
        Debug.LogError(msg);
        if (quit)
        {
            Application.Quit();
        }
    }
}
