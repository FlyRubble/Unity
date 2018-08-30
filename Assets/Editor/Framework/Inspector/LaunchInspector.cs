using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using Framework.JsonFx;
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
    /// Settings清单
    /// </summary>
    private Settings m_settings = null;

    /// <summary>
    /// 启动配置
    /// </summary>
    private LaunchConfig m_app = null;

    /// <summary>
    /// 是否打包完整资源
    /// </summary>
    private bool m_buildAsset = false;

    /// <summary>
    /// 是否打包更新资源
    /// </summary>
    private bool m_buildUpdateAsset = false;

    /// <summary>
    /// 工程设置路径
    /// </summary>
    public string appSettings
    {
        get { return Directory.GetCurrentDirectory() + "/Assets/App.settings"; }
    }

    /// <summary>
    /// app路径
    /// </summary>
    public string appJson
    {
        get { return Directory.GetCurrentDirectory() + "/Assets/data/resources/app.json"; }
    }

    /// <summary>
    /// 配置组名字表
    /// </summary>
    public List<string> nameList
    {
        get
        {
            List<string> names = new List<string>();
            for (int i = 0; i < m_settings.launchConfig.Count; ++i)
            {
                names.Add(m_settings.launchConfig[i].tag);
            }
            return names;
        }
    }

    /// <summary>
    /// OnEnable
    /// </summary>
    private void OnEnable()
    {
        if (!File.Exists(appSettings))
        {
            m_settings = new Settings();
            m_settings.launchConfig.Add(new LaunchConfig() { tag = "自定义" });
            File.WriteAllText(appSettings, JsonWriter.Serialize(m_settings));
        }

        m_settings = JsonReader.Deserialize<Settings>(File.ReadAllText(appSettings));
        for (int i = 0; i < m_settings.launchConfig.Count; ++i)
        {
            m_app = m_settings.launchConfig[i];
            if (m_app.tag.Equals("自定义"))
            {
                m_settings.launchConfig.Remove(m_app);
                m_settings.launchConfig.Insert(0, m_app);
                break;
            }
        }

        m_app = m_settings.launchConfig[m_settings.selected];
        ChangeSettings(ChangeType.ScriptingDefineSymbols);
    }

    /// <summary>
    /// OnInspectorGUI
    /// </summary>
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(Application.isPlaying);
        EditorGUI.BeginChangeCheck();
        {
            serializedObject.Update();
			bool bSave = false;
			string value = string.Empty;
			// 模式
			var index = m_app.scriptingDefineSymbols.Contains("AB_MODE") ? 1 : 0;
			var selected = GUILayout.Toolbar(index, new string[] { "编辑器模式", "高级AB模式" });
			if (index != selected)
            {
                bSave = true;
                ModeToggle (selected);
            }
			// App名字
			value = EditorGUILayout.TextField("Product Name", m_app.appName);
			if (value != m_app.appName) {
				bSave = true;
				m_app.appName = value;
				ChangeSettings (ChangeType.AppName);
			}
			// 包名
			value = EditorGUILayout.TextField("Bundle Identifier", m_app.bundleIdentifier);
			if (value != m_app.bundleIdentifier) {
				bSave = true;
				m_app.bundleIdentifier = value;
				ChangeSettings (ChangeType.BundleIdentifier);
			}
			// 版本
			value = EditorGUILayout.TextField("Version*", m_app.version);
			if (value != m_app.version) {
				bSave = true;
				m_app.version = value;
				ChangeSettings (ChangeType.Version);
			}
			// 版本Code
			#if UNITY_ANDROID
			m_app.bundleVersionCode = EditorGUILayout.IntField("BuildVersionCode", m_app.bundleVersionCode);
			if (PlayerSettings.Android.bundleVersionCode != m_app.bundleVersionCode)
			{
				bSave = true;	
				ChangeSettings (ChangeType.BundleVersionCode);
			}
			#elif UNITY_IOS
			m_app.bundleVersionCode = EditorGUILayout.IntField("BuildNumber", m_app.bundleVersionCode);
			if (PlayerSettings.Android.bundleVersionCode != m_app.bundleVersionCode)
			{
				bSave = true;	
				ChangeSettings (ChangeType.BundleVersionCode);
			}
			#else
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.IntField("BuildVersionCode", m_app.bundleVersionCode);
			EditorGUILayout.LabelField("*仅Android或IOS有效");
			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
			#endif

			// 平台、宏定义
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Platform", EditorUserBuildSettings.activeBuildTarget.ToString());
            EditorGUILayout.TextField("Scripting Define Symbol", m_app.scriptingDefineSymbols);
            EditorGUI.EndDisabledGroup();

            // 设置配置选项组
            var list = nameList;
            selected = EditorGUILayout.Popup("服务器配置", m_settings.selected, list.ToArray());
            if (selected != m_settings.selected)
            {
                bSave = true;
                m_settings.selected = selected;
                m_app = m_settings.launchConfig[m_settings.selected];

                ChangeSettings(ChangeType.AppName);
                ChangeSettings(ChangeType.BundleIdentifier);
                ChangeSettings(ChangeType.Version);
                ChangeSettings(ChangeType.BundleVersionCode);
                ChangeSettings(ChangeType.ScriptingDefineSymbols);
            }

            // 脚本
            EditorGUI.BeginDisabledGroup(true);
			SerializedProperty property = serializedObject.GetIterator ();
			if (property.NextVisible (true)) {
				EditorGUILayout.PropertyField (property, new GUIContent ("Script"), true, new GUILayoutOption[0]);
			}
			EditorGUI.EndDisabledGroup();

			// 其它字段属性
			EditorGUI.BeginDisabledGroup(m_settings.selected != 0);
            {
				// 登陆地址
				value = EditorGUILayout.TextField("登录地址", m_app.loginUrl);
				if (value != m_app.loginUrl) {
					bSave = true;
					m_app.loginUrl = value;
				}

				// URL
				value = EditorGUILayout.TextField("CDN资源地址", m_app.cdn);
				if (value != m_app.cdn) {
					bSave = true;
					m_app.cdn = value;
				}

				// 备用URL
				value = EditorGUILayout.TextField("CDN备用资源地址", m_app.spareCDN);
				if (value != m_app.spareCDN) {
					bSave = true;
					m_app.spareCDN = value;
				}

				// 是否开启引导
				bool bValue = EditorGUILayout.Toggle("开启新手引导?", m_app.isOpenGuide);
				if (bValue != m_app.isOpenGuide) {
					bSave = true;
					m_app.isOpenGuide = bValue;
				}

				// 是否开启更新模式
				bValue = EditorGUILayout.Toggle("开启资源更新?", m_app.updateMode);
				if (bValue != m_app.updateMode) {
					bSave = true;
					m_app.updateMode = bValue;
				}

				// 是否完全解锁所有功能
				bValue = EditorGUILayout.Toggle("开启所有功能?", m_app.functionUnlock);
				if (bValue != m_app.functionUnlock) {
					bSave = true;
					m_app.functionUnlock = bValue;
				}

				// 是否开启日志
				bValue = EditorGUILayout.Toggle("开启日志&GM工具?", m_app.log);
				if (bValue != m_app.log) {
					bSave = true;
					m_app.log = bValue;
				}

				// 是否开启Web日志
				bValue = EditorGUILayout.Toggle("开启远程日志?", m_app.webLog);
				if (bValue != m_app.webLog) {
					bSave = true;
					m_app.webLog = bValue;
				}

                // 远程日志白名单
                string ip = "";
                for (int i = 0; i < m_app.webLogIp.Count; ++i)
                {
                    ip += ";" + m_app.webLogIp[i];
                }
                if (ip.StartsWith(",") || ip.StartsWith(";") || ip.StartsWith("|"))
                {
                    ip = ip.Substring(1, ip.Length - 1);
                }
                value = EditorGUILayout.TextField("远程日志白名单", ip);
                if (value != ip)
                {
                    bSave = true;
                    m_app.webLogIp.Clear();
                    string[] array = value.Split(',', ';', '|');
                    for (int i = 0; i < array.Length; ++i)
                    {
                        m_app.webLogIp.Add(array[i]);
                    }
                }

                // [安卓]CDN资源标签
                value = EditorGUILayout.TextField("[安卓]CDN资源标签", m_app.androidPlatformName);
				if (value != m_app.androidPlatformName) {
					bSave = true;
					m_app.androidPlatformName = value;
				}

				// [苹果]CDN资源标签
				value = EditorGUILayout.TextField("[苹果]CDN资源标签", m_app.iOSPlatformName);
				if (value != m_app.iOSPlatformName) {
					bSave = true;
					m_app.iOSPlatformName = value;
				}

				// [桌面]CDN资源标签
				value = EditorGUILayout.TextField("[桌面]CDN资源标签", m_app.defaultPlatformName);
				if (value != m_app.defaultPlatformName) {
					bSave = true;
					m_app.defaultPlatformName = value;
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
                    m_settings.selected = selected;

                    LaunchConfig app = m_settings.launchConfig[m_settings.selected];
                    app.appName = m_app.appName;
                    app.bundleIdentifier = m_app.bundleIdentifier;
                    app.version = m_app.version;
                    app.bundleVersionCode = m_app.bundleVersionCode;
                    app.scriptingDefineSymbols = m_app.scriptingDefineSymbols;
                    app.loginUrl = m_app.loginUrl;
                    app.cdn = m_app.cdn;
                    app.spareCDN = m_app.spareCDN;
                    app.isOpenGuide = m_app.isOpenGuide;
                    app.updateMode = m_app.updateMode;
                    app.functionUnlock = m_app.functionUnlock;
                    app.log = m_app.log;
                    app.webLog = m_app.webLog;
                    app.webLogIp = m_app.webLogIp;
                    app.androidPlatformName = m_app.androidPlatformName;
                    app.iOSPlatformName = m_app.iOSPlatformName;
                    app.defaultPlatformName = m_app.defaultPlatformName;
                    m_app = app;

                    ChangeSettings(ChangeType.AppName);
                    ChangeSettings(ChangeType.BundleIdentifier);
                    ChangeSettings(ChangeType.Version);
                    ChangeSettings(ChangeType.BundleVersionCode);
                    ChangeSettings(ChangeType.ScriptingDefineSymbols);
                }
				if (GUILayout.Button("编辑配置..."))
				{
                    if (File.Exists (appSettings))
                    {
                        System.Diagnostics.Process.Start(appSettings);
                    }
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

            index = m_app.scriptingDefineSymbols.Contains("AB_MODE") ? 1 : 0;
            EditorGUI.BeginDisabledGroup(index == 0);
            {
                // 打完整资源包应用
                if (GUILayout.Button("打包完整资源"))
                {
                    m_buildAsset = true;
                }
                // 打更新资源包
                if (GUILayout.Button("打包更新资源"))
                {
                    m_buildUpdateAsset = true;
                }
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();

			if (bSave)
            {
				bSave = false;

                File.WriteAllText(appSettings, JsonWriter.Serialize(m_settings));
                
                App app = new App();
                app.version = m_app.version;
                app.loginUrl = m_app.loginUrl;
                app.cdn = m_app.cdn;
                app.spareCDN = m_app.spareCDN;
                app.isOpenGuide = m_app.isOpenGuide;
                app.updateMode = m_app.updateMode;
                app.functionUnlock = m_app.functionUnlock;
                app.log = m_app.log;
                app.webLog = m_app.webLog;
                app.webLogIp = m_app.webLogIp;
#if UNITY_IOS
                app.platform = m_app.iOSPlatformName;
#elif UNITY_ANDROID
                app.platform = m_app.androidPlatformName;
#else
                app.platform = m_app.defaultPlatformName;
#endif
                File.WriteAllText(appJson, JsonWriter.Serialize(app));
                AssetDatabase.Refresh();
            }
		}
		EditorGUI.EndChangeCheck ();
        EditorGUI.EndDisabledGroup();

        // 打完整资源包应用
        if (m_buildAsset)
        {
            m_buildAsset = false;
            AssetBundleEditor.BuildAssetBundlesAndCopy(AssetBundleEditor.outputPath);
            GUIUtility.ExitGUI();
        }
        // 打更新资源包
        if (m_buildUpdateAsset)
        {
            m_buildUpdateAsset = false;
            AssetBundleEditor.BuildUpdateAssetBundlesAndZip(AssetBundleEditor.outputPath, AssetBundleEditor.outputVersionPath, App.Init().version, App.Init().platform, App.Init().cdn);
            GUIUtility.ExitGUI();
        }

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
            var tokens = m_app.scriptingDefineSymbols.Split(';');
            m_app.scriptingDefineSymbols = "";
            foreach (var token in tokens)
            {
                if (token != "AB_MODE" && !string.IsNullOrEmpty(token))
                {
                    m_app.scriptingDefineSymbols += token + ";";
                }
            }
            if (m_app.scriptingDefineSymbols.EndsWith(";"))
            {
                m_app.scriptingDefineSymbols = m_app.scriptingDefineSymbols.Substring(0, m_app.scriptingDefineSymbols.Length - 1);
            }
            ChangeSettings(ChangeType.ScriptingDefineSymbols);
        } break;
		case 1:
			{
            EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
            {
                    new EditorBuildSettingsScene("Assets/Launch.unity", true),
            };

            var tokens = m_app.scriptingDefineSymbols.Split(';');
            bool abMode = false;
            foreach (var token in tokens)
            {
                if (token == "AB_MODE")
                {
                    abMode = true;
                    break;
                }
            }
            if (!abMode)
            {
                if (m_app.scriptingDefineSymbols.EndsWith(";"))
                {
                    m_app.scriptingDefineSymbols += "AB_MODE";
                }
                else if (m_app.scriptingDefineSymbols.Length > 0)
                {
                    m_app.scriptingDefineSymbols += ";AB_MODE";
                }
                else
                {
                    m_app.scriptingDefineSymbols += "AB_MODE";
                }
            }
            ChangeSettings(ChangeType.ScriptingDefineSymbols);
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
				PlayerSettings.productName = m_app.appName;
			} break;
		case ChangeType.BundleIdentifier:
			{
                #if UNITY_ANDROID || UNITY_IOS
				PlayerSettings.applicationIdentifier = m_app.bundleIdentifier;
                #endif
			} break;
		case ChangeType.Version:
			{
				PlayerSettings.bundleVersion = m_app.version;
			} break;
		case ChangeType.BundleVersionCode:
			{
				#if UNITY_ANDROID
				PlayerSettings.Android.bundleVersionCode = m_app.bundleVersionCode;
				#elif UNITY_IOS
				PlayerSettings.iOS.buildNumber = m_app.bundleVersionCode;
				#endif
			} break;
		case ChangeType.ScriptingDefineSymbols:
			{
				if (!PlayerSettings.GetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup).Equals (m_app.scriptingDefineSymbols)) {
					PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, m_app.scriptingDefineSymbols);
				}
			} break;
		}
	}
}