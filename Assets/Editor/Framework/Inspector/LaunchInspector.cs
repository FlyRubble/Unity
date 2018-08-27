using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using Framework.Singleton;
using Framework.IO;

/// <summary>
/// 启动监视器
/// </summary>
[CustomEditor(typeof(Launch))]
public class LaunchInspector : Editor
{
	enum ChangeType
	{
		AppName,
		BundleIdentifier,
		Version,
		BundleVersionCode,
		ScriptingDefineSymbols
	}

    /// <summary>
    /// App配置清单
    /// </summary>
    class AppConfigManifest : Singleton<AppConfigManifest>
    {
        #region Variable
        /// <summary>
        /// App配置清单
        /// </summary>
        private Ini m_app = null;

        /// <summary>
        /// 配置组的选择
        /// </summary>
        private int m_selected = 0;
        #endregion

        #region Property
        /// <summary>
        /// App配置清单
        /// </summary>
        /// <value>The app.</value>
        public Ini app
        {
            get { return m_app; }
        }

        /// <summary>
        /// 配置组选择
        /// </summary>
        /// <value>The selected.</value>
        public int selected
        {
            get { return m_selected; }
            set { m_selected = value; }
        }

        /// <summary>
        /// 配置组名字表
        /// </summary>
        public List<string> nameLsit
        {
            get { return m_app.nameList.GetRange(1, m_app.nameList.Count - 1); }
        }
        #endregion

        #region Function
        /// <summary>
        /// 构造
        /// </summary>
        public AppConfigManifest()
        {
            string path = Directory.GetCurrentDirectory() + "/ProjectSettings/Config.ini";
            m_app = new Ini(path);
            // 读取文本
            m_app.Read();

            // 补充选项
            if (!m_app.Contains(""))
            {
                m_app.SetInt("selected", 0);
            }

            // 补充自定义
            if (!m_app.Contains("自定义"))
            {
                m_app.SetString("自定义", "name", "自定义");
            }

            // 取值
            m_selected = m_app.GetInt("selected");
            var array = m_app.nameList.GetRange(1, m_app.nameList.Count - 1);
            string name = array.Count > m_selected ? array[m_selected] : "";
            
            AppConfig.appName = m_app.GetString(name, "appName") ?? "";
            AppConfig.bundleIdentifier = m_app.GetString(name, "bundleIdentifier") ?? "";
            AppConfig.version = m_app.GetString(name, "version") ?? "";
            AppConfig.bundleVersionCode = m_app.GetInt(name, "bundleVersionCode");
            AppConfig.scriptingDefineSymbols = m_app.GetString(name, "scriptingDefineSymbols") ?? "";
            AppConfig.loginUrl = m_app.GetString(name, "loginUrl") ?? "";
            AppConfig.url = m_app.GetString(name, "url") ?? "";
            AppConfig.spareUrl = m_app.GetString(name, "spareUrl") ?? "";
            AppConfig.isOpenGuide = m_app.GetBool(name, "isOpenGuide");
            AppConfig.updateMode = m_app.GetBool(name, "updateMode");
            AppConfig.functionUnlock = m_app.GetBool(name, "functionUnlock");
            AppConfig.log = m_app.GetBool(name, "log");
            AppConfig.webLog = m_app.GetBool(name, "webLog");
            AppConfig.webLogIp.Clear();
            foreach (var ip in (m_app.GetString(name, "webLogIp") ?? "").Split('|'))
            {
                AppConfig.webLogIp.Add(ip);
            }
            AppConfig.androidPlatformName = m_app.GetString(name, "androidPlatformName") ?? "";
            AppConfig.iOSPlatformName = m_app.GetString(name, "iOSPlatformName") ?? "";
            AppConfig.defaultPlatformName = m_app.GetString(name, "defaultPlatformName") ?? "";
        }
        #endregion
    }

    public override void OnInspectorGUI()
    {
		AppConfig.Init ();
        EditorGUI.BeginDisabledGroup(Application.isPlaying);
        EditorGUI.BeginChangeCheck();
        {
            serializedObject.Update();
			bool bSave = false;
			string value = string.Empty;
			// 模式
			AppConfig.scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			var index = AppConfig.scriptingDefineSymbols.Contains("AB_MODE") ? 1 : 0;
			var selected = GUILayout.Toolbar(index, new string[] { "编辑器模式", "高级AB模式" });
			if (index != selected)
            {
				ModeToggle (selected);
            }
			// App名字
			value = EditorGUILayout.TextField("Product Name", AppConfig.appName);
			if (value != AppConfig.appName) {
				bSave = true;
				AppConfig.appName = value;
				ChangeSettings (ChangeType.AppName);
			}
			// 包名
			value = EditorGUILayout.TextField("Bundle Identifier", AppConfig.bundleIdentifier);
			if (value != AppConfig.bundleIdentifier) {
				bSave = true;
				AppConfig.bundleIdentifier = value;
				ChangeSettings (ChangeType.BundleIdentifier);
			}
			// 版本
			value = EditorGUILayout.TextField("Version*", AppConfig.version);
			if (value != AppConfig.version) {
				bSave = true;
				AppConfig.version = value;
				ChangeSettings (ChangeType.Version);
			}
			// 版本Code
			#if UNITY_ANDROID
			AppConfig.bundleVersionCode = EditorGUILayout.IntField("BuildVersionCode", AppConfig.bundleVersionCode);
			if (PlayerSettings.Android.bundleVersionCode != AppConfig.bundleVersionCode)
			{
				bSave = true;	
				ChangeSettings (ChangeType.BundleVersionCode);
			}
			#elif UNITY_IOS
			AppConfig.bundleVersionCode = EditorGUILayout.IntField("BuildNumber", AppConfig.bundleVersionCode);
			if (PlayerSettings.Android.bundleVersionCode != AppConfig.bundleVersionCode)
			{
				bSave = true;	
				ChangeSettings (ChangeType.BundleVersionCode);
			}
			#else
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.IntField("BuildVersionCode", AppConfig.bundleVersionCode);
			EditorGUILayout.LabelField("*仅Android或IOS有效");
			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
			#endif

			// 平台、宏定义
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Platform", EditorUserBuildSettings.activeBuildTarget.ToString());
			EditorGUILayout.TextField("Scripting Define Symbol", AppConfig.scriptingDefineSymbols);
			EditorGUI.EndDisabledGroup();

            // 设置配置选项组
            var list = AppConfigManifest.instance.nameLsit;
			selected = EditorGUILayout.Popup("AppConfig", AppConfigManifest.instance.selected, list.ToArray());
			if (selected != AppConfigManifest.instance.selected)
            {
				bSave = true;
                AppConfigManifest.instance.selected = selected;

				string name = AppConfigManifest.instance.selected < list.Count ? list[AppConfigManifest.instance.selected] : "";
				AppConfig.appName = AppConfigManifest.instance.app.GetString(name, "appName") ?? "";
				AppConfig.bundleIdentifier = AppConfigManifest.instance.app.GetString(name, "bundleIdentifier") ?? "";
				AppConfig.version = AppConfigManifest.instance.app.GetString(name, "version") ?? "";
				AppConfig.bundleVersionCode = AppConfigManifest.instance.app.GetInt(name, "bundleVersionCode");
				AppConfig.scriptingDefineSymbols = AppConfigManifest.instance.app.GetString(name, "scriptingDefineSymbols") ?? "";
				AppConfig.loginUrl = AppConfigManifest.instance.app.GetString(name, "loginUrl") ?? "";
				AppConfig.url = AppConfigManifest.instance.app.GetString(name, "url") ?? "";
				AppConfig.spareUrl = AppConfigManifest.instance.app.GetString(name, "spareUrl") ?? "";
				AppConfig.isOpenGuide = AppConfigManifest.instance.app.GetBool(name, "isOpenGuide");
				AppConfig.updateMode = AppConfigManifest.instance.app.GetBool(name, "updateMode");
				AppConfig.functionUnlock = AppConfigManifest.instance.app.GetBool(name, "functionUnlock");
				AppConfig.log = AppConfigManifest.instance.app.GetBool(name, "log");
				AppConfig.webLog = AppConfigManifest.instance.app.GetBool(name, "webLog");
				AppConfig.webLogIp.Clear ();
				foreach (var ip in (AppConfigManifest.instance.app.GetString(name, "webLogIp") ?? "").Split('|')) {
					AppConfig.webLogIp.Add (ip);
				}
				AppConfig.androidPlatformName = AppConfigManifest.instance.app.GetString(name, "androidPlatformName") ?? "";
				AppConfig.iOSPlatformName = AppConfigManifest.instance.app.GetString(name, "iOSPlatformName") ?? "";
				AppConfig.defaultPlatformName = AppConfigManifest.instance.app.GetString(name, "defaultPlatformName") ?? "";

				ChangeSettings (ChangeType.AppName);
				ChangeSettings (ChangeType.BundleIdentifier);
				ChangeSettings (ChangeType.Version);
				ChangeSettings (ChangeType.BundleVersionCode);
				ChangeSettings (ChangeType.ScriptingDefineSymbols);
            }

			// 脚本
			EditorGUI.BeginDisabledGroup(true);
			SerializedProperty property = serializedObject.GetIterator ();
			if (property.NextVisible (true)) {
				EditorGUILayout.PropertyField (property, new GUIContent ("Script"), true, new GUILayoutOption[0]);
			}
			EditorGUI.EndDisabledGroup();

			// 其它字段属性
			EditorGUI.BeginDisabledGroup(AppConfigManifest.instance.selected != 0 || Application.isPlaying);
            {
				// 登陆地址
				value = EditorGUILayout.TextField("LoginUrl", AppConfig.loginUrl);
				if (value != AppConfig.loginUrl) {
					bSave = true;
					AppConfig.loginUrl = value;
				}

				// URL
				value = EditorGUILayout.TextField("Url", AppConfig.url);
				if (value != AppConfig.url) {
					bSave = true;
					AppConfig.url = value;
				}

				// 备用URL
				value = EditorGUILayout.TextField("SpareUrl", AppConfig.spareUrl);
				if (value != AppConfig.spareUrl) {
					bSave = true;
					AppConfig.spareUrl = value;
				}

				// 是否开启引导
				bool bValue = EditorGUILayout.Toggle("IsOpenGuide", AppConfig.isOpenGuide);
				if (bValue != AppConfig.isOpenGuide) {
					bSave = true;
					AppConfig.isOpenGuide = bValue;
				}

				// 是否开启更新模式
				bValue = EditorGUILayout.Toggle("UpdateMode", AppConfig.updateMode);
				if (bValue != AppConfig.updateMode) {
					bSave = true;
					AppConfig.updateMode = bValue;
				}

				// 是否完全解锁所有功能
				bValue = EditorGUILayout.Toggle("FunctionUnlock", AppConfig.functionUnlock);
				if (bValue != AppConfig.functionUnlock) {
					bSave = true;
					AppConfig.functionUnlock = bValue;
				}

				// 是否开启日志
				bValue = EditorGUILayout.Toggle("Log", AppConfig.log);
				if (bValue != AppConfig.log) {
					bSave = true;
					AppConfig.log = bValue;
				}

				// 是否开启Web日志
				bValue = EditorGUILayout.Toggle("WebLog", AppConfig.webLog);
				if (bValue != AppConfig.webLog) {
					bSave = true;
					AppConfig.webLog = bValue;
				}

				// [安卓]CDN资源标签
				value = EditorGUILayout.TextField("[安卓]CDN资源标签", AppConfig.androidPlatformName);
				if (value != AppConfig.androidPlatformName) {
					bSave = true;
					AppConfig.androidPlatformName = value;
				}

				// [苹果]CDN资源标签
				value = EditorGUILayout.TextField("[苹果]CDN资源标签", AppConfig.iOSPlatformName);
				if (value != AppConfig.iOSPlatformName) {
					bSave = true;
					AppConfig.iOSPlatformName = value;
				}

				// [桌面]CDN资源标签
				value = EditorGUILayout.TextField("[桌面]CDN资源标签", AppConfig.defaultPlatformName);
				if (value != AppConfig.defaultPlatformName) {
					bSave = true;
					AppConfig.defaultPlatformName = value;
				}
            }
			EditorGUI.EndDisabledGroup();

			if (selected != 0)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("复制到自定义..."))
				{
					bSave = true;
					selected = list.IndexOf ("自定义");
					AppConfigManifest.instance.selected = selected;
				}
				if (GUILayout.Button("编辑配置..."))
				{
                    string path = Directory.GetCurrentDirectory() + "/ProjectSettings/AppConfig.ini";
                    if (!File.Exists (path)) {
						File.Create (path);
					}
					System.Diagnostics.Process.Start(path);
				}
				EditorGUILayout.EndVertical();

				if (!Application.isPlaying)
				{
					EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
				}
			}
			// 清除本地资源解压缓存
			if (GUILayout.Button ("清除本地资源解压缓存")) {
				FileUtil.DeleteFileOrDirectory (Application.persistentDataPath);
			}
            // 打包完整资源
            if (GUILayout.Button("打包完整资源"))
            {
                AssetBundleEditor.BuildAssetBundles();
                AssetBundleEditor.CopyAssetBundles();
            }
            // 打更新资源包
            if (GUILayout.Button("打更新资源包"))
            {
                AssetBundleEditor.BuildAssetBundles();
                AssetBundleEditor.CopyUpdateAssetBundles();
            }

            serializedObject.ApplyModifiedProperties();

			if (bSave) {
				bSave = false;

				string name = selected < list.Count ? list[selected] : "";
                AppConfigManifest.instance.app.SetString(name, "appName", AppConfig.appName);
                AppConfigManifest.instance.app.SetString(name, "bundleIdentifier", AppConfig.bundleIdentifier);
                AppConfigManifest.instance.app.SetString(name, "version", AppConfig.version);
                AppConfigManifest.instance.app.SetInt(name, "bundleVersionCode", AppConfig.bundleVersionCode);
                AppConfigManifest.instance.app.SetString(name, "scriptingDefineSymbols", AppConfig.scriptingDefineSymbols);
                AppConfigManifest.instance.app.SetString(name, "loginUrl", AppConfig.loginUrl);
                AppConfigManifest.instance.app.SetString(name, "url", AppConfig.url);
                AppConfigManifest.instance.app.SetString(name, "spareUrl", AppConfig.spareUrl);
                AppConfigManifest.instance.app.SetBool(name, "isOpenGuide", AppConfig.isOpenGuide);
                AppConfigManifest.instance.app.SetBool(name, "updateMode", AppConfig.updateMode);
                AppConfigManifest.instance.app.SetBool(name, "functionUnlock", AppConfig.functionUnlock);
                AppConfigManifest.instance.app.SetBool(name, "log", AppConfig.log);
                AppConfigManifest.instance.app.SetBool(name, "webLog", AppConfig.webLog);
				string webLogIp = "";
				foreach (var ip in AppConfig.webLogIp) {
                    if (webLogIp.Length > 0)
                    {
                        webLogIp += "|";
                    }
					webLogIp += ip;
				}
                AppConfigManifest.instance.app.SetString(name, "webLogIp", webLogIp);
                AppConfigManifest.instance.app.SetString(name, "androidPlatformName", AppConfig.androidPlatformName);
                AppConfigManifest.instance.app.SetString(name, "iOSPlatformName", AppConfig.iOSPlatformName);
                AppConfigManifest.instance.app.SetString(name, "defaultPlatformName", AppConfig.defaultPlatformName);
                AppConfigManifest.instance.app.SetInt("selected", AppConfigManifest.instance.selected);
                AppConfigManifest.instance.app.Write ();

                // 写入到Resources
                string path = Directory.GetCurrentDirectory() + "/Assets/data/Resources/AppConfig.txt";
                Ini app = new Ini(path);
                app.SetString("appName", AppConfig.appName);
                app.SetString("bundleIdentifier", AppConfig.bundleIdentifier);
                app.SetString("version", AppConfig.version);
                app.SetInt("bundleVersionCode", AppConfig.bundleVersionCode);
                app.SetString("scriptingDefineSymbols", AppConfig.scriptingDefineSymbols);
                app.SetString("loginUrl", AppConfig.loginUrl);
                app.SetString("url", AppConfig.url);
                app.SetString("spareUrl", AppConfig.spareUrl);
                app.SetBool("isOpenGuide", AppConfig.isOpenGuide);
                app.SetBool("updateMode", AppConfig.updateMode);
                app.SetBool("functionUnlock", AppConfig.functionUnlock);
                app.SetBool("log", AppConfig.log);
                app.SetBool("webLog", AppConfig.webLog);
                app.SetString("webLogIp", webLogIp);
                app.SetString("androidPlatformName", AppConfig.androidPlatformName);
                app.SetString("iOSPlatformName", AppConfig.iOSPlatformName);
                app.SetString("defaultPlatformName", AppConfig.defaultPlatformName);
                app.Write();
                AssetDatabase.Refresh();
            }
		}
		EditorGUI.EndChangeCheck ();
        EditorGUI.EndDisabledGroup();


    }

	/// <summary>
	/// 模式切换
	/// </summary>
	/// <param name="selected">Selected.</param>
	public void ModeToggle(int selected)
    {
		switch (selected) {
		case 0:
			{
				AppConfig.scriptingDefineSymbols = "";
				var tokens = AppConfig.scriptingDefineSymbols.Split(';');
				foreach (var token in tokens)
				{
					if (token != "AB_MODE")
					{
						AppConfig.scriptingDefineSymbols += token + ";";
					}
				}
				ChangeSettings (ChangeType.ScriptingDefineSymbols);
			} break;
		case 1:
			{
				EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
				{
					new EditorBuildSettingsScene("Assets/Launch.unity", true),
				};

				FileUtil.DeleteFileOrDirectory (Application.persistentDataPath);
				var tokens = AppConfig.scriptingDefineSymbols.Split(';');
				bool abMode = false;
				foreach (var token in tokens)
				{
					if (token == "AB_MODE")
					{
						abMode = true;
						break;
					}
				}
				if (!abMode) {
					if (AppConfig.scriptingDefineSymbols.EndsWith (";")) {
						AppConfig.scriptingDefineSymbols += "AB_MODE;";
					} else {
						AppConfig.scriptingDefineSymbols += ";AB_MODE;";
					}
				}
				ChangeSettings (ChangeType.ScriptingDefineSymbols);
			} break;
		}
    }

	/// <summary>
	/// 改变设置
	/// </summary>
	/// <param name="type">Type.</param>
	private void ChangeSettings(ChangeType type)
	{
		switch (type) {
		case ChangeType.AppName:
			{
				PlayerSettings.productName = AppConfig.appName;
			} break;
		case ChangeType.BundleIdentifier:
			{
                #if UNITY_ANDROID || UNITY_IOS
				PlayerSettings.applicationIdentifier = AppConfig.bundleIdentifier;
                #endif
			} break;
		case ChangeType.Version:
			{
				PlayerSettings.bundleVersion = AppConfig.version;
			} break;
		case ChangeType.BundleVersionCode:
			{
				#if UNITY_ANDROID
				PlayerSettings.Android.bundleVersionCode = AppConfig.bundleVersionCode;
				#elif UNITY_IOS
				PlayerSettings.iOS.buildNumber = AppConfig.bundleVersionCode;
				#endif
			} break;
		case ChangeType.ScriptingDefineSymbols:
			{
				if (!PlayerSettings.GetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup).Equals (AppConfig.scriptingDefineSymbols)) {
					PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, AppConfig.scriptingDefineSymbols);
				}
			} break;
		}
	}
}