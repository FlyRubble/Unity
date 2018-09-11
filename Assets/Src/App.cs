using System.Collections.Generic;
using UnityEngine;
using Framework.JsonFx;

/// <summary>
/// App
/// </summary>
public sealed class App
{
    public const string Name = "name";
    public const string ProductName = "productName";
    public const string BundleIdentifier = "bundleIdentifier";
    public const string Version = "version";
    public const string BundleVersionCode = "bundleVersionCode";
    public const string ScriptingDefineSymbols = "scriptingDefineSymbols";
    public const string LoginUrl = "loginUrl";
    public const string Cdn = "cdn";
    public const string IsOpenGuide = "isOpenGuide";
    public const string IsOpenUpdate = "isOpenUpdate";
    public const string IsUnlockAllFunction = "isUnlockAllFunction";
    public const string Log = "log";
    public const string WebLog = "webLog";
    public const string WebLogIp = "webLogIp";
    public const string AndroidPlatformName = "androidPlatformName";
    public const string IOSPlatformName = "iOSPlatformName";
    public const string DefaultPlatformName = "defaultPlatformName";

    #region Variable
    /// <summary>
    /// 产品名
    /// </summary>
    private static string m_productName = string.Empty;

    /// <summary>
    /// 游戏版本[App版本、显示版本、资源版本一致]
    /// </summary>
    private static string m_version = "1.0.0";

    /// <summary>
    /// 登陆地址
    /// </summary>
    private static string m_loginUrl = string.Empty;

    /// <summary>
    /// Cdn
    /// </summary>
    private static string m_cdn = string.Empty;

    /// <summary>
    /// 是否开启引导
    /// </summary>
    private static bool m_isOpenGuide = true;

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    private static bool m_isOpenUpdate = true;

    /// <summary>
    /// 是否功能全解锁
    /// </summary>
    private static bool m_isUnlockAllFunction = false;

    /// <summary>
    /// 是否开启日志
    /// </summary>
    private static bool m_log = false;

    /// <summary>
    /// 是否开启Web日志
    /// </summary>
    private static bool m_webLog = false;

    /// <summary>
    /// WebLog白名单
    /// </summary>
    private static List<string> m_webLogIp = new List<string>();

    /// <summary>
    /// [安卓]平台标签
    /// </summary>
    private static string m_androidPlatformName = string.Empty;

    /// <summary>
    /// [苹果]平台标签
    /// </summary>
    private static string m_iOSPlatformName = string.Empty;

    /// <summary>
    /// [桌面]平台标签
    /// </summary>
    private static string m_defaultPlatformName = string.Empty;
    #endregion

    #region Property
    /// <summary>
    /// 产品名
    /// </summary>
    public static string productName
    {
        get { return m_productName; }
    }

    /// <summary>
    /// 游戏版本[App版本、显示版本、资源版本一致]
    /// </summary>
    public static string version
    {
        get { return m_version; }
    }

    /// <summary>
    /// 登陆地址
    /// </summary>
    /// <value>The login URL.</value>
    public static string loginUrl
    {
        get { return m_loginUrl; }
    }

    /// <summary>
    /// CDN
    /// </summary>
    public static string cdn
    {
        get { return m_cdn; }
    }

    /// <summary>
    /// 是否开启引导
    /// </summary>
    public static bool isOpenGuide
    {
        get { return m_isOpenGuide; }
    }

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    public static bool isOpenUpdate
    {
        get { return m_isOpenUpdate; }
    }

    /// <summary>
    /// 是否功能全解锁
    /// </summary>
    public static bool isUnlockAllFunction
    {
        get { return m_isUnlockAllFunction; }
    }

    /// <summary>
    /// 是否开启日志
    /// </summary>
    public static bool log
    {
        get { return m_log; }
    }

    /// <summary>
    /// 是否开启Web日志
    /// </summary>
    public static bool webLog
    {
        get { return m_webLog; }
    }

    /// <summary>
    /// WebLog白名单
    /// </summary>
    public static List<string> webLogIp
    {
        get { return m_webLogIp; }
    }

    /// <summary>
    /// 平台标签
    /// </summary>
    public static string platform
    {
        get
        {
#if UNITY_ANDROID
            return m_androidPlatformName;
#elif UNITY_IOS
            return m_iOSPlatformName;
#else
            return m_defaultPlatformName;
#endif
        }
    }
    #endregion

    #region Function
    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        TextAsset t = Resources.Load("version") as TextAsset;
        Dictionary<string, object> data = JsonReader.Deserialize<Dictionary<string, object>>(t.text);

        // 产品名
        if (data.ContainsKey(ProductName))
        {
            m_productName = data[ProductName].ToString();
        }

        // 游戏版本[App版本、显示版本、资源版本一致]
        if (data.ContainsKey(Version))
        {
            m_productName = data[Version].ToString();
        }

        // 登陆地址
        if (data.ContainsKey(LoginUrl))
        {
            m_loginUrl = data[LoginUrl].ToString();
        }

        // Cdn
        if (data.ContainsKey(Cdn))
        {
            m_cdn = data[Cdn].ToString();
        }

        // 是否开启引导
        if (data.ContainsKey(IsOpenGuide))
        {
            m_isOpenGuide = (bool)data[IsOpenGuide];
        }

        // 是否开启更新功能
        if (data.ContainsKey(IsOpenUpdate))
        {
            m_isOpenUpdate = (bool)data[IsOpenUpdate];
        }

        // 是否功能全解锁
        if (data.ContainsKey(IsUnlockAllFunction))
        {
            m_isUnlockAllFunction = (bool)data[IsUnlockAllFunction];
        }

        // 是否开启日志
        if (data.ContainsKey(Log))
        {
            m_log = (bool)data[Log];
        }

        // 是否开启Web日志
        if (data.ContainsKey(WebLog))
        {
            m_webLog = (bool)data[WebLog];
        }

        // WebLog白名单
        if (data.ContainsKey(WebLogIp))
        {
            m_webLogIp.Clear();
            string[] array = data[WebLogIp].ToString().Split(',', ';', '|');
            foreach (var ip in array)
            {
                if (!string.IsNullOrEmpty(ip))
                {
                    m_webLogIp.Add(ip);
                }
            }
        }

        // [安卓]平台标签
        if (data.ContainsKey(AndroidPlatformName))
        {
            m_androidPlatformName = data[AndroidPlatformName].ToString();
        }

        // [苹果]平台标签
        if (data.ContainsKey(IOSPlatformName))
        {
            m_iOSPlatformName = data[IOSPlatformName].ToString();
        }

        // [桌面]平台标签
        if (data.ContainsKey(DefaultPlatformName))
        {
            m_defaultPlatformName = data[DefaultPlatformName].ToString();
        }
    }
    #endregion
}
