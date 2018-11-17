using UnityEngine;
using Framework;
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
        // 选择沙盒路径还是流式路径
        bool sandbox = App.version.Equals(Util.GetString(Const.SANDBOX_VERSION));
        AssetManager.instance.url = sandbox ? App.persistentDataPath : App.streamingAssetsPath;
        // 配置参数设置
        Debugger.logEnabled = App.log;
        Debugger.webLogEnabled = App.webLog;
        AssetManager.instance.maxLoader = Const.MAX_LOADER;
        // 启动定时器
        Schedule.instance.Start();
    }

    /// <summary>
    /// 开始
    /// </summary>
    void Start()
    {
        Lua.instance.Init();
        //StateMachine.instance.OnEnter(new Init());
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        Lua.instance.Update();
        AssetManager.instance.Update();
        Schedule.instance.Update(Time.deltaTime);
        //StateMachine.instance.Update();
    }

    /// <summary>
    /// 销毁
    /// </summary>
    private void OnDestroy()
    {
        Lua.instance.Destroy();
    }
}