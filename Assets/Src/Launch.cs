using UnityEngine;
using Framework.IO;

public class Launch : MonoBehaviour {
	/// <summary>
	/// 开始
	/// </summary>
	void Start()
	{
		// 初始化AppConfig
		AppConfig.Init();
        // 
        TextAsset asset = Resources.Load("manifest.txt") as TextAsset;
        if (asset != null)
        {
            ManifestConfig manifest = new ManifestConfig();
            manifest.Read(asset.text);
        }
	}
}