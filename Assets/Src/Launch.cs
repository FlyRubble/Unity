using System.Collections.Generic;
using UnityEngine;
using Framework.IO;

public class Launch : MonoBehaviour
{
    public Dictionary<string, object> a;

    /// <summary>
    /// 启动
    /// </summary>
    void Awake()
    {
        App.Init();

        Debug.Log(App.productName);
        Debug.Log(App.version);
        Debug.Log(App.isOpenGuide);
        Debug.Log(App.webLogIp.Count);
        Debug.Log(App.platform);
    }

    /// <summary>
    /// 开始
    /// </summary>
    void Start()
	{
        TextAsset asset = Resources.Load("manifest.txt") as TextAsset;
        if (asset != null)
        {
            ManifestConfig manifest = new ManifestConfig();
        }
	}
}