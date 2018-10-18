public class Const
{
    #region App
    /// <summary>
    /// 定义配置组名字
    /// </summary>
    public const string NAME = "name";

    /// <summary>
    /// 产品的名字
    /// </summary>
    public const string PRODUCT_NAME = "productName";

    /// <summary>
    /// 包名
    /// </summary>
    public const string BUNDLE_IDENTIFIER = "bundleIdentifier";

    /// <summary>
    /// 版本
    /// </summary>
    public const string VERSION = "version";

    /// <summary>
    /// Code版本
    /// </summary>
    public const string BUNDLE_VERSION_CODE = "bundleVersionCode";

    /// <summary>
    /// 宏定义
    /// </summary>
    public const string SCRIPTING_DEFINE_SYMBOLS = "scriptingDefineSymbols";

    /// <summary>
    /// 登录地址
    /// </summary>
    public const string LOGIN_URL = "loginUrl";

    /// <summary>
    /// CDN资源地址
    /// </summary>
    public const string CDN = "cdn";

    /// <summary>
    /// 是否开启引导
    /// </summary>
    public const string OPEN_GUIDE = "openGuide";

    /// <summary>
    /// 是否开启更新
    /// </summary>
    public const string OPEN_UPDATE = "openUpdate";

    /// <summary>
    /// 是否解锁全部功能
    /// </summary>
    public const string UNLOCK_ALL_FUNCTION = "unlockAllFunction";

    /// <summary>
    /// 是否开启日志
    /// </summary>
    public const string LOG = "log";

    /// <summary>
    /// 是否开启web日志
    /// </summary>
    public const string WEB_LOG = "webLog";

    /// <summary>
    /// web日志开启的白名单
    /// </summary>
    public const string WEB_LOG_IP = "webLogIp";

    /// <summary>
    /// Android平台名
    /// </summary>
    public const string ANDROID_PLATFORM_NAME = "androidPlatformName";

    /// <summary>
    /// iOS平台名
    /// </summary>
    public const string IOS_PLATFORM_NAME = "iOSPlatformName";

    /// <summary>
    /// 默认平台名
    /// </summary>
    public const string DEFAULT_PLATFORM_NAME = "defaultPlatformName";
    #endregion

    /// <summary>
    /// 最大同时加载资源数
    /// </summary>
    public const int MAX_LOADER = 6;

    /// <summary>
    /// 更新文件
    /// </summary>
    public const string SANDBOX_VERSION = "SandboxVersion";

    /// <summary>
    /// 远程版本文件
    /// </summary>
    public const string REMOTE_VERSION = "/version_V{0}.json";

    /// <summary>
    /// 清单文件
    /// </summary>
    public const string MANIFESTFILE = "data/conf/manifestfile.json";

    /// <summary>
    /// 更新文件
    /// </summary>
    public const string UPDATE_FILE = "data/conf/updatefile.json";

    #region UI界面
    /// <summary>
    /// 画布
    /// </summary>
    public const string UI_CANVAS = "canvas";

    /// <summary>
    /// 加载界面
    /// </summary>
    public const string UI_LOADING = "uiloading";

    /// <summary>
    /// 普通提示框
    /// </summary>
    public const string UI_NORMAL_TIPS_BOX = "uinormaltipsbox";
    #endregion

    #region 语言
    /// <summary>
    /// 网络不可达
    /// </summary>
    public const string ID_NETWORK_INVALID = "连接网络失败，请检查网络设置。";

    /// <summary>
    /// 版本过低，请更新版本。
    /// </summary>
    public const string ID_VERSION_LOW = "客户端版本过低，确定安装新版本？";

    /// <summary>
    /// 请求资源中
    /// </summary>
    public const string ID_GETING = "请求资源中...";
    
    #endregion
}