using System.Collections.Generic;
using UnityEngine;
using Framework.Event;

namespace UnityAsset
{
    public static class AssetManagerExtensions
    {
        /// <summary>
        /// 加载依赖资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dic"></param>
        /// <param name="action"></param>
        public static void LoadDependentAsset(this AssetManager assetManager, string path, ref Dictionary<string, AsyncAsset> dic, Action<bool, AsyncAsset> action)
        {
            var data = App.manifest.Get(path);
            if (data != null)
            {
                foreach (var value in data.directDependencies)
                {
                    assetManager.LoadDependentAsset(value, ref dic, action);
                }
                dic.Add(path, assetManager.AssetBundleLoadAsync(assetManager.url + path, action));
            }
        }

        /// <summary>
        /// 加载资源(自动加载依赖资源)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        public static void Load(this AssetManager assetManager, string path, Action<bool, Object> action)
        {
            if (action != null)
            {
#if UNITY_EDITOR && !AB_MODE
                if (action != null)
                {
                    path = "Assets/" + path;
                    action(true, UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
                }
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
                    assetManager.LoadDependentAsset(data.name, ref dic, complete);
                }
                else
                {
                    action(false, null);
                }
#endif
            }
        }
    }
}
