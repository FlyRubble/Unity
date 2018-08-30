using UnityEngine;
using Framework.IO;

public class Launch : MonoBehaviour
{
    /// <summary>
    /// 启动
    /// </summary>
    void Awake()
    {
        App.Init();
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