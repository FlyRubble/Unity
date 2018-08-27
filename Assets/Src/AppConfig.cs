using System.Collections.Generic;
using UnityEngine;
using Framework.IO;

/// <summary>
/// App配置
/// </summary>
public class AppConfig
{
    #region Variable
	/// <summary>
	/// 游戏名字
	/// </summary>
	private static string m_appName = string.Empty;

	/// <summary>
	/// 游戏包名
	/// </summary>
	private static string m_bundleIdentifier = string.Empty;

	/// <summary>
	/// 游戏版本[App版本、显示版本、资源版本一致]
	/// </summary>
	private static string m_version = "1.0.0";

	/// <summary>
	/// 构建版本
	/// </summary>
	private static int m_bundleVersionCode = 0;

	/// <summary>
	/// 宏定义
	/// </summary>
	private static string m_scriptingDefineSymbols = string.Empty;

	/// <summary>
	/// 登陆地址
	/// </summary>
	private static string m_loginUrl = string.Empty;

	/// <summary>
	/// 首选URL
	/// </summary>
	private static string m_url = string.Empty;

	/// <summary>
	/// 备用URL
	/// </summary>
	private static string m_spareUrl = string.Empty;

	/// <summary>
	/// 是否开启引导
	/// </summary>
	private static bool m_isOpenGuide = true;

	/// <summary>
	/// 是否开启更新功能
	/// </summary>
	private static bool m_updateMode = true;

	/// <summary>
	/// 是否功能全解锁
	/// </summary>
	private static bool m_functionUnlock = false;

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
	private static List<string> m_webLogIp = new List<string>(2);

	/// <summary>
	/// 安卓平台标签
	/// </summary>
	private static string m_androidPlatformName = "Android";

	/// <summary>
	/// IOS平台标签
	/// </summary>
	private static string m_iOSPlatformName = "IOS";

	/// <summary>
	/// 默认平台标签
	/// </summary>
	private static string m_defaultPlatformName = "Windows";
	#endregion

	#region Property
	/// <summary>
	/// 游戏名字
	/// </summary>
	public static string appName
	{
		get { return m_appName; }
		set { m_appName = value; }
	}

	/// <summary>
	/// 游戏包名
	/// </summary>
	public static string bundleIdentifier
	{
		get { return m_bundleIdentifier; }
		set { m_bundleIdentifier = value; }
	}

	/// <summary>
	/// 游戏版本[App版本、显示版本、资源版本一致]
	/// </summary>
	public static string version
	{
		get { return m_version; }
		set { m_version = value; }
	}

	/// <summary>
	/// 构建版本
	/// </summary>
	public static int bundleVersionCode
	{
		get { return m_bundleVersionCode; }
		set { m_bundleVersionCode = value; }
	}

	/// <summary>
	/// 宏定义
	/// </summary>
	/// <value>The scripting define symbols.</value>
	public static string scriptingDefineSymbols
	{
		get { return m_scriptingDefineSymbols; }
		set { m_scriptingDefineSymbols = value; }
	}

	/// <summary>
	/// 登陆地址
	/// </summary>
	/// <value>The login URL.</value>
	public static string loginUrl
	{
		get { return m_loginUrl; }
		set { m_loginUrl = value; }
	}

	/// <summary>
	/// 首选URL
	/// </summary>
	public static string url
	{
		get { return m_url; }
		set { m_url = value; }
	}

	/// <summary>
	/// 备用URL
	/// </summary>
	public static string spareUrl
	{
		get { return m_spareUrl; }
		set { m_spareUrl = value; }
	}

	/// <summary>
	/// 是否开启引导
	/// </summary>
	public static bool isOpenGuide
	{
		get { return m_isOpenGuide; }
		set { m_isOpenGuide = value; }
	}

	/// <summary>
	/// 是否开启更新功能
	/// </summary>
	public static bool updateMode
	{
		get { return m_updateMode; }
		set { m_updateMode = value; }
	}

	/// <summary>
	/// 是否功能全解锁
	/// </summary>
	public static bool functionUnlock
	{
		get { return m_functionUnlock; }
		set { m_functionUnlock = value; }
	}

	/// <summary>
	/// 是否开启日志
	/// </summary>
	public static bool log
	{
		get { return m_log; }
		set { m_log = value; }
	}

	/// <summary>
	/// 是否开启Web日志
	/// </summary>
	public static bool webLog
	{
		get { return m_webLog; }
		set { m_webLog = value; }
	}

	/// <summary>
	/// WebLog白名单
	/// </summary>
	public static List<string> webLogIp
	{
		get { return m_webLogIp; }
		set { m_webLogIp = value; }
	}

	/// <summary>
	/// 安卓平台标签
	/// </summary>
	public static string androidPlatformName
	{
		get { return m_androidPlatformName; }
		set { m_androidPlatformName = value; }
	}

	/// <summary>
	/// IOS平台标签
	/// </summary>
	public static string iOSPlatformName
	{
		get { return m_iOSPlatformName; }
		set { m_iOSPlatformName = value; }
	}

	/// <summary>
	/// 默认平台标签
	/// </summary>
	public static string defaultPlatformName
	{
		get { return m_defaultPlatformName; }
		set { m_defaultPlatformName = value; }
	}
	#endregion

	#region Function
	/// <summary>
	/// 初始化
	/// </summary>
	public static void Init()
	{
        // 读取文本
        TextAsset config = Resources.Load<TextAsset>("AppConfig");
        if (config != null)
        {
            Ini app = new Ini();
            app.Read(config.text);
            // 取值
            m_appName = app.GetString("appName") ?? "";
            m_bundleIdentifier = app.GetString("bundleIdentifier") ?? "";
            m_version = app.GetString("version") ?? "";
            m_bundleVersionCode = app.GetInt("bundleVersionCode");
            m_scriptingDefineSymbols = app.GetString("scriptingDefineSymbols") ?? "";
            m_loginUrl = app.GetString("loginUrl") ?? "";
            m_url = app.GetString("url") ?? "";
            m_spareUrl = app.GetString("spareUrl") ?? "";
            m_isOpenGuide = app.GetBool("isOpenGuide");
            m_updateMode = app.GetBool("updateMode");
            m_functionUnlock = app.GetBool("functionUnlock");
            m_log = app.GetBool("log");
            m_webLog = app.GetBool("webLog");
            m_webLogIp.Clear();
            foreach (var ip in (app.GetString("webLogIp") ?? "").Split('|'))
            {
                m_webLogIp.Add(ip);
            }
            m_androidPlatformName = app.GetString("androidPlatformName") ?? "";
            m_iOSPlatformName = app.GetString("iOSPlatformName") ?? "";
            m_defaultPlatformName = app.GetString("defaultPlatformName") ?? "";
        }
    }
    #endregion
}