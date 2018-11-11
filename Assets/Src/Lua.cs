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
    #endregion

    #region Function
    public void Init()
    {
        m_luaEnv = new LuaEnv();
        m_scriptEnv = m_luaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = m_luaEnv.NewTable();
        meta.Set("__index", m_luaEnv.Global);
        m_scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        m_scriptEnv.Set("self", this);

        //TextAsset ta = Resources.Load("lua") as TextAsset;

        m_luaEnv.DoString("require('lua')", "Lua", m_scriptEnv);
        var action = m_scriptEnv.Get<Action>("awake");
        action();
    }
    #endregion
}
