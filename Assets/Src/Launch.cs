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
    }

    /// <summary>
    /// 开始
    /// </summary>
    void Start()
	{
        StateMachine.instance.OnEnter(new Init());
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        AssetManager.instance.Update();
        Schedule.instance.Update(Time.deltaTime);
        StateMachine.instance.Update();
    }
}