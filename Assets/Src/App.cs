using System.Collections.Generic;
using UnityEngine;
using Framework.JsonFx;
using Framework.IO;

/// <summary>
/// App
/// </summary>
public sealed class App
{
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
    private static bool m_openGuide = true;

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    private static bool m_openUpdate = true;

    /// <summary>
    /// 是否功能全解锁
    /// </summary>
    private static bool m_unlockAllFunction = false;

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

    /// <summary>
    /// 新版本下载地址
    /// </summary>
    private static string m_newVersionDownloadUrl = string.Empty;

    /// <summary>
    /// 资源清单文件
    /// </summary>
    private static ManifestConfig m_manifest = new ManifestConfig();

    /// <summary>
    /// 远程版本
    /// </summary>
    private static Dictionary<string, object> m_remoteVersion = new Dictionary<string, object>();
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
    public static bool openGuide
    {
        get { return m_openGuide; }
    }

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    public static bool openUpdate
    {
        get { return m_openUpdate; }
    }

    /// <summary>
    /// 是否功能全解锁
    /// </summary>
    public static bool unlockAllFunction
    {
        get { return m_unlockAllFunction; }
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

    /// <summary>
    /// 新版本下载地址
    /// </summary>
    public static string newVersionDownloadUrl
    {
        get { return m_newVersionDownloadUrl; }
        set { m_newVersionDownloadUrl = value; }
    }

    /// <summary>
    /// 资源清单文件
    /// </summary>
    public static ManifestConfig manifest
    {
        get { return m_manifest; }
        set { m_manifest = value; }
    }

    /// <summary>
    /// 远程版本
    /// </summary>
    public static Dictionary<string, object> remoteVersion
    {
        get { return m_remoteVersion; }
        set { m_remoteVersion = value; }
    }

    /// <summary>
    /// assetPath
    /// </summary>
    public static string assetPath
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return Application.persistentDataPath + "/";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return Application.persistentDataPath + "/";
            }
            else
            {
                return Application.persistentDataPath + "/";
            }
        }
    }

    /// <summary>
    /// persistentDataPath
    /// </summary>
    public static string persistentDataPath
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return "jar:file://" + Application.persistentDataPath + "/";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return "file://" + Application.persistentDataPath + "/";
            }
            else
            {
                return "file:///" + Application.persistentDataPath + "/";
            }
        }
    }

    /// <summary>
    /// streamingAssetsPath
    /// </summary>
    public static string streamingAssetsPath
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return Application.streamingAssetsPath + "/";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return "file://" + Application.streamingAssetsPath + "/";
            }
            else
            {
                return "file://" + Application.streamingAssetsPath + "/";
            }
        }
    }

    /// <summary>
    /// 网络是否可达
    /// </summary>
    public static bool internetReachability
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    /// <summary>
    /// 是否是流量网络
    /// </summary>
    public static bool dataNetwork
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
        }
    }

    /// <summary>
    /// 是否是Wifi网络
    /// </summary>
    public static bool wifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
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

        Update(data);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="data"></param>
    public static void Update(Dictionary<string, object> data, List<string> filter = null)
    {
        // 产品名
        if (data.ContainsKey(Const.PRODUCT_NAME) && (filter == null || !filter.Contains(Const.PRODUCT_NAME)))
        {
            m_productName = data[Const.PRODUCT_NAME].ToString();
        }

        // 游戏版本[App版本、显示版本、资源版本一致]
        if (data.ContainsKey(Const.VERSION) && (filter == null || !filter.Contains(Const.VERSION)))
        {
            m_productName = data[Const.VERSION].ToString();
        }

        // 登陆地址
        if (data.ContainsKey(Const.LOGIN_URL) && (filter == null || !filter.Contains(Const.LOGIN_URL)))
        {
            m_loginUrl = data[Const.LOGIN_URL].ToString();
        }

        // Cdn
        if (data.ContainsKey(Const.CDN) && (filter == null || !filter.Contains(Const.CDN)))
        {
            m_cdn = data[Const.CDN].ToString();
        }

        // 是否开启引导
        if (data.ContainsKey(Const.OPEN_GUIDE) && (filter == null || !filter.Contains(Const.OPEN_GUIDE)))
        {
            m_openGuide = (bool)data[Const.OPEN_GUIDE];
        }

        // 是否开启更新功能
        if (data.ContainsKey(Const.OPEN_UPDATE) && (filter == null || !filter.Contains(Const.OPEN_UPDATE)))
        {
            m_openUpdate = (bool)data[Const.OPEN_UPDATE];
        }

        // 是否功能全解锁
        if (data.ContainsKey(Const.UNLOCK_ALL_FUNCTION) && (filter == null || !filter.Contains(Const.UNLOCK_ALL_FUNCTION)))
        {
            m_unlockAllFunction = (bool)data[Const.UNLOCK_ALL_FUNCTION];
        }

        // 是否开启日志
        if (data.ContainsKey(Const.LOG) && (filter == null || !filter.Contains(Const.LOG)))
        {
            m_log = (bool)data[Const.LOG];
        }

        // 是否开启Web日志
        if (data.ContainsKey(Const.WEB_LOG) && (filter == null || !filter.Contains(Const.WEB_LOG)))
        {
            m_webLog = (bool)data[Const.WEB_LOG];
        }

        // WebLog白名单
        if (data.ContainsKey(Const.WEB_LOG_IP) && (filter == null || !filter.Contains(Const.WEB_LOG_IP)))
        {
            m_webLogIp.Clear();
            string[] array = data[Const.WEB_LOG_IP].ToString().Split(',', ';', '|');
            foreach (var ip in array)
            {
                if (!string.IsNullOrEmpty(ip))
                {
                    m_webLogIp.Add(ip);
                }
            }
        }

        // [安卓]平台标签
        if (data.ContainsKey(Const.ANDROID_PLATFORM_NAME) && (filter == null || !filter.Contains(Const.ANDROID_PLATFORM_NAME)))
        {
            m_androidPlatformName = data[Const.ANDROID_PLATFORM_NAME].ToString();
        }

        // [苹果]平台标签
        if (data.ContainsKey(Const.IOS_PLATFORM_NAME) && (filter == null || !filter.Contains(Const.IOS_PLATFORM_NAME)))
        {
            m_iOSPlatformName = data[Const.IOS_PLATFORM_NAME].ToString();
        }

        // [桌面]平台标签
        if (data.ContainsKey(Const.DEFAULT_PLATFORM_NAME) && (filter == null || !filter.Contains(Const.DEFAULT_PLATFORM_NAME)))
        {
            m_defaultPlatformName = data[Const.DEFAULT_PLATFORM_NAME].ToString();
        }
    }
    #endregion
}
