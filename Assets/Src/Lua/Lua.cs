using XLua;
using System.Collections.Generic;
using Framework.UI;
using Framework.Event;
using Framework.Singleton;

public class Lua : MonoBehaviourSingleton<Lua>
{
    #region Variable
    /// <summary>
    /// Lua虚拟机
    /// </summary>
    private LuaEnv m_luaEnv = null;

    /// <summary>
    /// 表
    /// </summary>
    private LuaTable m_luaTable = null;

    /// <summary>
    /// 开始
    /// </summary>
    private Action m_start = null;

    /// <summary>
    /// 更新
    /// </summary>
    private Action m_update = null;

    /// <summary>
    /// 销毁
    /// </summary>
    private Action m_destroy = null;

    /// <summary>
    /// 初始化是否完成
    /// </summary>
    private bool m_init = false;

    /// <summary>
    /// 启动是否完成
    /// </summary>
    private bool m_awake = false;

    /// <summary>
    /// 获取配置列表
    /// </summary>
    public delegate void ConfigList(Dictionary<string, Action<string>> config);

    /// <summary>
    /// 加载资源表
    /// </summary>
    private Dictionary<string, Action<string>> m_config = null;

    /// <summary>
    /// 加载资源数据表
    /// </summary>
    private Dictionary<string, string> m_data = null;

    /// <summary>
    /// 资源路径
    /// </summary>
    private string m_assetPath = string.Empty;

    /// <summary>
    /// 定时器
    /// </summary>
    private ScheduleEvent m_scheduleEvent = null;

    /// <summary>
    /// 字节
    /// </summary>
    private byte[] m_byte = null;

    /// <summary>
    /// 锁
    /// </summary>
    private object m_lock = new object();
    #endregion

    #region Property
    /// <summary>
    /// 初始化是否完成
    /// </summary>
    public bool init
    {
        get
        {
            lock (m_lock)
            {
                return m_init;
            }
        }
        set
        {
            lock (m_lock)
            {
                m_init = value;
            }
        }
    }

    /// <summary>
    /// 启动
    /// </summary>
    public bool awake
    {
        get
        {
            lock (m_lock)
            {
                return m_awake;
            }
        }
        set
        {
            lock (m_lock)
            {
                m_awake = value;
            }
        }
    }

    /// <summary>
    /// 加载资源数据表
    /// </summary>
    private Dictionary<string, string> data
    {
        get
        {
            lock (m_lock)
            {
                return m_data;
            }
        }
        set
        {
            lock (m_lock)
            {
                m_data = value;
            }
        }
    }

    /// <summary>
    /// 资源路径
    /// </summary>
    private string assetPath
    {
        get
        {
            lock (m_lock)
            {
                return m_assetPath;
            }
        }
        set
        {
            lock (m_lock)
            {
                m_assetPath = value;
            }
        }
    }

    /// <summary>
    /// 定时器
    /// </summary>
    private ScheduleEvent scheduleEvent
    {
        get
        {
            lock (m_lock)
            {
                return m_scheduleEvent;
            }
        }
        set
        {
            lock (m_lock)
            {
                m_scheduleEvent = value;
            }
        }
    }

    /// <summary>
    /// 字节
    /// </summary>
    private byte[] bytes
    {
        get
        {
            lock (m_lock)
            {
                return m_byte;
            }
        }
        set
        {
            lock (m_lock)
            {
                m_byte = value;
            }
        }
    }
    #endregion

    #region Function
    /// <summary>
    /// 开始
    /// </summary>
    private void Start()
    {
        m_config = new Dictionary<string, Action<string>>();
        m_data = new Dictionary<string, string>();

        scheduleEvent = Schedule.instance.ScheduleUpdate(() => {
            if (!string.IsNullOrEmpty(assetPath))
            {
                UnityAsset.AssetManager.instance.AssetBundleAsyncLoad(assetPath, (bResult, asset) =>
                {
                    if (bResult)
                    {
                        bytes = System.Text.Encoding.Default.GetBytes(asset.mainAsset.ToString());
                    }
                    UnityAsset.AssetManager.instance.UnloadAssets(asset, true);
                }, dependence: false);
                assetPath = string.Empty;
            }
        }, 0.0001F, float.MaxValue);
        
        System.Threading.Thread thread = new System.Threading.Thread(Run);
        thread.Start();
    }
    
    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        if (m_awake)
        {
            m_luaEnv.Tick();
            m_update();
        }
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Destroy()
    {
        if (m_destroy != null)
        {
            m_destroy();
        }
        m_start = null;
        m_update = null;
        m_destroy = null;

        m_config.Clear();
        UIManager.instance.ClearImmediate();
        m_luaEnv.Dispose();
    }

    /// <summary>
    /// 多线程运行
    /// </summary>
    private void Run()
    {
        Init();
        init = true;
        OnAwake();

        while (m_config.Count > 0)
        {
            System.Threading.Thread.Sleep(50);
            
            foreach (var kvp in data)
            {
                Action<string> action = m_config[kvp.Key];
                data.Remove(kvp.Key);
                m_config.Remove(kvp.Key);
                try
                {
                    action(kvp.Value);
                }
                catch (System.Exception e)
                {

                }
                break;
            }
        }

        awake = true;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        m_luaEnv = new LuaEnv();
        m_luaEnv.AddLoader(Loader);
        m_luaTable = m_luaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = m_luaEnv.NewTable();
        meta.Set("__index", m_luaEnv.Global);
        m_luaTable.SetMetaTable(meta);
        meta.Dispose();

        m_luaTable.Set("self", this);
        m_luaEnv.DoString("require('main')", "Lua", m_luaTable);

        // 获取要加载的配置
        ConfigList func = m_luaTable.Get<ConfigList>("init");
        if (func != null)
        {
            func(m_config);
            func = null;

            foreach (var kvp in m_config)
            {
                ConfigManager.instance.Add(kvp.Key, (text)=> {
                    data.Add(kvp.Key, text);
                });
            }
        }
    }

    /// <summary>
    /// 启动Lua
    /// </summary>
    private void OnAwake()
    {
        Action awake = m_luaTable.Get<Action>("awake");
        if (awake != null)
        {
            awake();
            awake = null;
        }

        m_start = m_luaTable.Get<Action>("start");
        m_update = m_luaTable.Get<Action>("update");
        m_destroy = m_luaTable.Get<Action>("destroy");
    }

    /// <summary>
    /// 开始Lua
    /// </summary>
    public void OnStart()
    {
        Schedule.instance.UnSchedule(scheduleEvent);
        scheduleEvent = null;
        if (m_start != null)
        {
            m_start();
        }
    }

    /// <summary>
    /// Lua加载器
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    byte[] Loader(ref string fileName)
    {
        byte[] result = new byte[0];

        if (awake)
        {
#if UNITY_EDITOR && !AB_MODE
            string path = System.IO.Directory.GetCurrentDirectory() + "/Assets/data/lua/" + fileName.Replace(".", "/").ToLower() + ".txt";
            result = System.IO.File.ReadAllBytes(path);
#else
            string path = UnityAsset.AssetManager.instance.url + "data/lua/" + fileName.Replace(".", "/").ToLower() + ".txt";
            UnityAsset.AsyncAsset asyncAsset = UnityAsset.AssetManager.instance.AssetBundleLoad(path);
            result = System.Text.Encoding.Default.GetBytes(asyncAsset.mainAsset.ToString());
            UnityAsset.AssetManager.instance.UnloadAssets(asyncAsset, true);
#endif
        }
        else
        {
#if UNITY_EDITOR && !AB_MODE
            string path = System.IO.Directory.GetCurrentDirectory() + "/Assets/data/lua/" + fileName.Replace(".", "/").ToLower() + ".txt";
            result = System.IO.File.ReadAllBytes(path);
#else
            if (scheduleEvent == null)
            {
                string path = UnityAsset.AssetManager.instance.url + "data/lua/" + fileName.Replace(".", "/").ToLower() + ".txt";
                UnityAsset.AsyncAsset asyncAsset = UnityAsset.AssetManager.instance.AssetBundleLoad(path);
                result = System.Text.Encoding.Default.GetBytes(asyncAsset.mainAsset.ToString());
                UnityAsset.AssetManager.instance.UnloadAssets(asyncAsset, true);
            }
            else
            {
                assetPath = UnityAsset.AssetManager.instance.url + "data/lua/" + fileName.Replace(".", "/").ToLower() + ".txt";
                while (true)
                {
                    System.Threading.Thread.Sleep(30);
                    if (bytes != null)
                    {
                        break;
                    }
                }
                result = bytes;
                bytes = null;
            }
#endif
        }
        return result;
    }
    #endregion
}
