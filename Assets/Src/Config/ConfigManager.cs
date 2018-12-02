using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Singleton;
using Framework.Event;

/// <summary>
/// 配置管理
/// </summary>
public class ConfigManager : Singleton<ConfigManager>
{
    #region Variable
    /// <summary>
    /// 统一配置表在加载之前使用(唯一)
    /// </summary>
    private LangConfig m_langConfig = null;

    /// <summary>
    /// 需要加载的配置表
    /// </summary>
    private Dictionary<string, Action<string>> m_loadList = null;
    #endregion

    #region Property
    /// <summary>
    /// 统一配置表在加载之前使用(唯一)
    /// </summary>
    public LangConfig langConfig
    {
        get
        {
            return m_langConfig;
        }
    }

    /// <summary>
    /// 获取加载列表
    /// </summary>
    public Dictionary<string, Action<string>> loadList
    {
        get
        {
            return m_loadList;
        }
    }
    #endregion

    #region Function
    /// <summary>
    /// 构造函数
    /// </summary>
    public ConfigManager()
    {
        m_langConfig = new LangConfig();
        m_loadList = new Dictionary<string, Action<string>>();
    }


    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        //m_loadList.Add("data/conf/lang.json", m_langConfig.Init);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Add(string path, Action<string> action)
    {
        m_loadList.Add(path, action);
    }

    /// <summary>
    /// 得到语言配置文本
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="defaultText"></param>
    /// <returns></returns>
    public static string GetLang(string uuid, string defaultText = null)
    {
        defaultText = string.Format("{0} = {1}", uuid, defaultText);
        if (instance.langConfig.TryGetLang(uuid, out uuid))
        {
            defaultText = uuid;
        }
        return defaultText;
    }

    /// <summary>
    /// 得到语言配置文本
    /// </summary>
    /// <param name="id"></param>
    /// <param name="defaultText"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string GetLangFormat(string uuid, string defaultText, params object[] args)
    {
        return string.Format(GetLang(uuid, defaultText), args);
    }
    #endregion
}
