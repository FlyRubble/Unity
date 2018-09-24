using UnityEngine;
using Framework;
using Framework.IO;
using UnityAsset;
using Framework.Event;

public class Launch : MonoBehaviour
{
    /// <summary>
    /// 启动
    /// </summary>
    void Awake()
    {
        App.Init();
        AssetManager.instance.maxLoader = Const.MAX_LOADER;
        Schedule.instance.Start();
        StateMachine.instance.OnEnter(new Unzip());

        //Object o = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets", typeof(Object))
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

    private void Update()
    {
        AssetManager.instance.Update();
        Schedule.instance.Update(Time.deltaTime);
        StateMachine.instance.Update();
    }
}