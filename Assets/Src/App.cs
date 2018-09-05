﻿using System.Collections.Generic;
using UnityEngine;
using Framework.Singleton;
using Framework.JsonFx;

/// <summary>
/// App
/// </summary>
public sealed class App
{
    #region Variable
    /// <summary>
    /// 游戏版本[App版本、显示版本、资源版本一致]
    /// </summary>
    private string m_version = "1.0.0";

    /// <summary>
    /// 登陆地址
    /// </summary>
    private string m_loginUrl = string.Empty;

    /// <summary>
    /// 首选cdn
    /// </summary>
    private string m_cdn = string.Empty;

    /// <summary>
    /// 备用cdn
    /// </summary>
    private string m_spareCDN = string.Empty;

    /// <summary>
    /// 是否开启引导
    /// </summary>
    private bool m_isOpenGuide = true;

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    private bool m_updateMode = true;

    /// <summary>
    /// 是否功能全解锁
    /// </summary>
    private bool m_functionUnlock = false;

    /// <summary>
    /// 是否开启日志
    /// </summary>
    private bool m_log = false;

    /// <summary>
    /// 是否开启Web日志
    /// </summary>
    private bool m_webLog = false;

    /// <summary>
    /// WebLog白名单
    /// </summary>
    private List<string> m_webLogIp = new List<string>();

    /// <summary>
    /// 平台标签
    /// </summary>
    private string m_platform = "Android";
    #endregion

    #region Static Variable
    /// <summary>
    /// 全局
    /// </summary>
    private static App m_instance = null;
    #endregion

    #region Property
    /// <summary>
    /// 游戏版本[App版本、显示版本、资源版本一致]
    /// </summary>
    public string version
    {
        get { return m_version; }
        set { m_version = value; }
    }

    /// <summary>
    /// 登陆地址
    /// </summary>
    /// <value>The login URL.</value>
    public string loginUrl
    {
        get { return m_loginUrl; }
        set { m_loginUrl = value; }
    }

    /// <summary>
    /// 首选CDN
    /// </summary>
    public string cdn
    {
        get { return m_cdn; }
        set { m_cdn = value; }
    }

    /// <summary>
    /// 备用CDN
    /// </summary>
    public string spareCDN
    {
        get { return m_spareCDN; }
        set { m_spareCDN = value; }
    }

    /// <summary>
    /// 是否开启引导
    /// </summary>
    public bool isOpenGuide
    {
        get { return m_isOpenGuide; }
        set { m_isOpenGuide = value; }
    }

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    public bool updateMode
    {
        get { return m_updateMode; }
        set { m_updateMode = value; }
    }

    /// <summary>
    /// 是否功能全解锁
    /// </summary>
    public bool functionUnlock
    {
        get { return m_functionUnlock; }
        set { m_functionUnlock = value; }
    }

    /// <summary>
    /// 是否开启日志
    /// </summary>
    public bool log
    {
        get { return m_log; }
        set { m_log = value; }
    }

    /// <summary>
    /// 是否开启Web日志
    /// </summary>
    public bool webLog
    {
        get { return m_webLog; }
        set { m_webLog = value; }
    }

    /// <summary>
    /// WebLog白名单
    /// </summary>
    public List<string> webLogIp
    {
        get { return m_webLogIp; }
        set { m_webLogIp = value; }
    }

    /// <summary>
    /// 平台标签
    /// </summary>
    public string platform
    {
        get { return m_platform; }
        set { m_platform = value; }
    }
    #endregion

    #region Static Property
    /// <summary>
    /// 游戏版本[App版本、显示版本、资源版本一致]
    /// </summary>
    public static string getVersion
    {
        get { return m_instance.version; }
        set { m_instance.version = value; }
    }

    /// <summary>
    /// 登陆地址
    /// </summary>
    /// <value>The login URL.</value>
    public static string getLoginUrl
    {
        get { return m_instance.loginUrl; }
    }

    /// <summary>
    /// 首选CDN
    /// </summary>
    public static string getCDN
    {
        get { return m_instance.cdn; }
    }

    /// <summary>
    /// 备用CDN
    /// </summary>
    public static string getSpareCDN
    {
        get { return m_instance.spareCDN; }
    }

    /// <summary>
    /// 是否开启引导
    /// </summary>
    public static bool getIsOpenGuide
    {
        get { return m_instance.isOpenGuide; }
    }

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    public static bool getUpdateMode
    {
        get { return m_instance.updateMode; }
    }

    /// <summary>
    /// 是否功能全解锁
    /// </summary>
    public static bool getFunctionUnlock
    {
        get { return m_instance.functionUnlock; }
    }

    /// <summary>
    /// 是否开启日志
    /// </summary>
    public static bool getLog
    {
        get { return m_instance.log; }
    }

    /// <summary>
    /// 是否开启Web日志
    /// </summary>
    public static bool getWebLog
    {
        get { return m_instance.webLog; }
    }

    /// <summary>
    /// WebLog白名单
    /// </summary>
    public static List<string> getWebLogIp
    {
        get { return m_instance.webLogIp; }
    }
    
    /// <summary>
    /// 平台标签
    /// </summary>
    public static string getPlatform
    {
        get
        {
            return m_instance.platform;
        }
    }
    #endregion

    #region Function
    /// <summary>
    /// 初始化
    /// </summary>
    public static App Init()
    {
        TextAsset t = Resources.Load("app") as TextAsset;
        m_instance = JsonReader.Deserialize<App>(t.text);
        return m_instance;
    }
    #endregion
}