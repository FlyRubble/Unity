using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 启动配置
/// </summary>
public class LaunchConfig
{
    #region Variable
    /// <summary>
    /// 标签
    /// </summary>
    private string m_tag = string.Empty;

    /// <summary>
    /// 游戏名字
    /// </summary>
    private string m_appName = string.Empty;

    /// <summary>
    /// 游戏包名
    /// </summary>
    private string m_bundleIdentifier = string.Empty;

    /// <summary>
    /// 游戏版本[App版本、显示版本、资源版本一致]
    /// </summary>
    private string m_version = "1.0.0";

    /// <summary>
    /// 构建版本
    /// </summary>
    private int m_bundleVersionCode = 0;

    /// <summary>
    /// 宏定义
    /// </summary>
    private string m_scriptingDefineSymbols = string.Empty;

    /// <summary>
    /// 登陆地址
    /// </summary>
    private string m_loginUrl = string.Empty;

    /// <summary>
    /// 首选CDN
    /// </summary>
    private string m_cdn = string.Empty;

    /// <summary>
    /// 备用CDN
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
    private List<string> m_webLogIp = new List<string>(2);

    /// <summary>
    /// 安卓平台标签
    /// </summary>
    private string m_androidPlatformName = "Android";

    /// <summary>
    /// IOS平台标签
    /// </summary>
    private string m_iOSPlatformName = "IOS";

    /// <summary>
    /// 默认平台标签
    /// </summary>
    private string m_defaultPlatformName = "Windows";
    #endregion

    #region Property
    /// <summary>
    /// 标签
    /// </summary>
    public string tag
    {
        get { return m_tag; }
        set { m_tag = value; }
    }

    /// <summary>
    /// 游戏名字
    /// </summary>
    public string appName
    {
        get { return m_appName; }
        set { m_appName = value; }
    }

    /// <summary>
    /// 游戏包名
    /// </summary>
    public string bundleIdentifier
    {
        get { return m_bundleIdentifier; }
        set { m_bundleIdentifier = value; }
    }

    /// <summary>
    /// 游戏版本[App版本、显示版本、资源版本一致]
    /// </summary>
    public string version
    {
        get { return m_version; }
        set { m_version = value; }
    }

    /// <summary>
    /// 构建版本
    /// </summary>
    public int bundleVersionCode
    {
        get { return m_bundleVersionCode; }
        set { m_bundleVersionCode = value; }
    }

    /// <summary>
    /// 宏定义
    /// </summary>
    /// <value>The scripting define symbols.</value>
    public string scriptingDefineSymbols
    {
        get { return m_scriptingDefineSymbols; }
        set { m_scriptingDefineSymbols = value; }
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
    /// 安卓平台标签
    /// </summary>
    public string androidPlatformName
    {
        get { return m_androidPlatformName; }
        set { m_androidPlatformName = value; }
    }

    /// <summary>
    /// IOS平台标签
    /// </summary>
    public string iOSPlatformName
    {
        get { return m_iOSPlatformName; }
        set { m_iOSPlatformName = value; }
    }

    /// <summary>
    /// 默认平台标签
    /// </summary>
    public string defaultPlatformName
    {
        get { return m_defaultPlatformName; }
        set { m_defaultPlatformName = value; }
    }
    #endregion
}