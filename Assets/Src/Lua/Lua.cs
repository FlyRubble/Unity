using XLua;
using Framework.Event;
using Framework.Singleton;

public class Lua : Singleton<Lua>
{
    #region Variable
    /// <summary>
    /// Lua虚拟机
    /// </summary>
    private LuaEnv m_luaEnv = null;

    /// <summary>
    /// 表
    /// </summary>
    private LuaTable m_scriptEnv = null;

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
    #endregion

    #region Function
    /// <summary>
    /// 启动
    /// </summary>
    public void Init()
    {
        m_luaEnv = new LuaEnv();
        m_luaEnv.AddLoader(Loader);
        m_scriptEnv = m_luaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = m_luaEnv.NewTable();
        meta.Set("__index", m_luaEnv.Global);
        m_scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        m_scriptEnv.Set("self", this);
        
        m_luaEnv.DoString("require('main')", "Lua", m_scriptEnv);
        
        m_start = m_scriptEnv.Get<Action>("start");
        m_update = m_scriptEnv.Get<Action>("update");
        m_destroy = m_scriptEnv.Get<Action>("destroy");

        if (m_start != null)
        {
            m_start();
        }
    }

    byte[] Loader(ref string fileName)
    {
        byte[] result = new byte[0];
#if UNITY_EDITOR && !AB_MODE
        string path = "Assets/data/lua/" + fileName.Replace(".", "/").ToLower() + ".txt";
        string text = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)).ToString();
        result = System.Text.Encoding.Default.GetBytes(text);
#else
        string path = UnityAsset.AssetManager.instance.url + "data/lua/" + fileName.Replace(".", "/").ToLower() + ".txt";
        UnityAsset.AsyncAsset asyncAsset = UnityAsset.AssetManager.instance.AssetBundleLoad(path);
        result = asyncAsset.bytes;
#endif
        return result;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        m_luaEnv.Tick();
        m_update();
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
        m_luaEnv.Dispose();
    }
    #endregion
}
