using System.Collections.Generic;
using UnityEngine;
using Framework.Event;

namespace UnityAsset
{
    public static class AssetManagerExtensions
    {
        /// <summary>
        /// 资源加载(编辑器非AB模式AssetDatabase同步加载，否则AB异步加载)
        /// </summary>
        /// <returns>asyncAsset</returns>
        public static AsyncAsset Load(this AssetManager self, string path, Action<bool, Object> complete, Action<bool, AsyncAsset> action = null, bool async = true)
        {
            AsyncAsset asyncAsset = null;
#if UNITY_EDITOR && !AB_MODE
        if (action != null)
        {
            action(true, UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/" + path, typeof(Object)));
        }
#else
            if (async)
            {
                asyncAsset = self.AssetBundleAsyncLoad(self.url + path, (bResult, asset) =>
                {
                    if (null != complete && bResult && null != asset)
                    {
                        complete(bResult, asset.mainAsset);
                    }
                }, action);
            }
            else
            {
                asyncAsset = self.AssetBundleLoad(self.url + path);
                if (null != complete && null != asyncAsset)
                {
                    complete(string.IsNullOrEmpty(asyncAsset.error), asyncAsset.mainAsset);
                }
            }
#endif
            return asyncAsset;
        }
    }
}
