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
    /// 工程设置路径
    /// </summary>
    public string projectSettings
    {
        get { return Directory.GetCurrentDirectory() + "/ProjectSettings/App.json"; }
    }

    /// <summary>
    /// app路径
    /// </summary>
    public string appPath
    {
        get { return Directory.GetCurrentDirectory() + "/Assets/data/resources/app.json"; }
    }

    /// <summary>
    /// App清单
    /// </summary>
    class AppManifest
    {
        #region Variable
        /// <summary>
        /// 配置组的选择
        /// </summary>
        private int m_selected = 0;

        /// <summary>
        /// App表
        /// </summary>
        private List<App> m_list = new List<App>();
        #endregion

        #region Property
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
        /// App表
        /// </summary>
        public List<App> list
        {
            get { return m_list; }
            set { m_list = value; }
        }

        /// <summary>
        /// 配置组名字表
        /// </summary>
        public List<string> nameLsit
        {
            get
            {
                List<string> list = new List<string>();
                for (int i = 0; i < m_list.Count; ++i)
                {
                    list.Add(m_list[i].tag);
                }
                return list;
            }
        }
        #endregion
    }

    /// <summary>
    /// Settings清单
    /// </summary>
    private AppManifest m_appManifest = new AppManifest();

    /// <summary>
    /// 是否打包完整资源
    /// </summary>
    private bool m_buildAsset = false;

    /// <summary>
    /// 是否打包更新资源
    /// </summary>
    private bool m_buildUpdateAsset = false;

    /// <summary>
    /// OnEnable
    /// </summary>
    private void OnEnable()
    {
        if (!File.Exists(projectSettings))
        {
            m_appManifest.list.Add(new App() { tag = "自定义" });
            File.WriteAllText(projectSettings, JsonWriter.Serialize(m_appManifest));
        }

        m_appManifest = JsonReader.Deserialize<AppManifest>(File.ReadAllText(projectSettings));
        App app = null;
        for (int i = 0; i < m_appManifest.list.Count; ++i)
        {
            app = m_appManifest.list[i];
            if (app.tag.Equals("自定义"))
            {
                m_appManifest.list.Remove(app);
                m_appManifest.list.Insert(0, app);
                break;
            }
        }

        app = m_appManifest.list[m_appManifest.selected];
        File.WriteAllText(appPath, JsonWriter.Serialize(app));
        AssetDatabase.Refresh();
        App.instance.Init();
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
			App.instance.scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			var index = App.instance.scriptingDefineSymbols.Contains("AB_MODE") ? 1 : 0;
			var selected = GUILayout.Toolbar(index, new string[] { "编辑器模式", "高级AB模式" });
			if (index != selected)
            {
                bSave = true;
                ModeToggle (selected);
            }
			// App名字
			value = EditorGUILayout.TextField("Product Name", App.instance.appName);
			if (value != App.instance.appName) {
				bSave = true;
				App.instance.appName = value;
				ChangeSettings (ChangeType.AppName);
			}
			// 包名
			value = EditorGUILayout.TextField("Bundle Identifier", App.instance.bundleIdentifier);
			if (value != App.instance.bundleIdentifier) {
				bSave = true;
				App.instance.bundleIdentifier = value;
				ChangeSettings (ChangeType.BundleIdentifier);
			}
			// 版本
			value = EditorGUILayout.TextField("Version*", App.instance.version);
			if (value != App.instance.version) {
				bSave = true;
				App.instance.version = value;
				ChangeSettings (ChangeType.Version);
			}
			// 版本Code
			#if UNITY_ANDROID
			App.instance.bundleVersionCode = EditorGUILayout.IntField("BuildVersionCode", App.instance.bundleVersionCode);
			if (PlayerSettings.Android.bundleVersionCode != App.instance.bundleVersionCode)
			{
				bSave = true;	
				ChangeSettings (ChangeType.BundleVersionCode);
			}
			#elif UNITY_IOS
			App.instance.bundleVersionCode = EditorGUILayout.IntField("BuildNumber", App.instance.bundleVersionCode);
			if (PlayerSettings.Android.bundleVersionCode != App.instance.bundleVersionCode)
			{
				bSave = true;	
				ChangeSettings (ChangeType.BundleVersionCode);
			}
			#else
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.IntField("BuildVersionCode", App.instance.bundleVersionCode);
			EditorGUILayout.LabelField("*仅Android或IOS有效");
			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
			#endif

			// 平台、宏定义
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Platform", EditorUserBuildSettings.activeBuildTarget.ToString());
			EditorGUILayout.TextField("Scripting Define Symbol", App.instance.scriptingDefineSymbols);
			EditorGUI.EndDisabledGroup();

            // 设置配置选项组
            var list = m_appManifest.nameLsit;
            selected = EditorGUILayout.Popup("服务器配置", m_appManifest.selected, list.ToArray());
            if (selected != m_appManifest.selected)
            {
                bSave = true;
                m_appManifest.selected = selected;

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
			EditorGUI.BeginDisabledGroup(m_appManifest.selected != 0 || Application.isPlaying);
            {
				// 登陆地址
				value = EditorGUILayout.TextField("登录地址", App.instance.loginUrl);
				if (value != App.instance.loginUrl) {
					bSave = true;
					App.instance.loginUrl = value;
				}

				// URL
				value = EditorGUILayout.TextField("CDN资源地址", App.instance.CDN);
				if (value != App.instance.CDN) {
					bSave = true;
					App.instance.CDN = value;
				}

				// 备用URL
				value = EditorGUILayout.TextField("CDN备用资源地址", App.instance.spareCDN);
				if (value != App.instance.spareCDN) {
					bSave = true;
					App.instance.spareCDN = value;
				}

				// 是否开启引导
				bool bValue = EditorGUILayout.Toggle("开启新手引导?", App.instance.isOpenGuide);
				if (bValue != App.instance.isOpenGuide) {
					bSave = true;
					App.instance.isOpenGuide = bValue;
				}

				// 是否开启更新模式
				bValue = EditorGUILayout.Toggle("开启资源更新?", App.instance.updateMode);
				if (bValue != App.instance.updateMode) {
					bSave = true;
					App.instance.updateMode = bValue;
				}

				// 是否完全解锁所有功能
				bValue = EditorGUILayout.Toggle("开启所有功能?", App.instance.functionUnlock);
				if (bValue != App.instance.functionUnlock) {
					bSave = true;
					App.instance.functionUnlock = bValue;
				}

				// 是否开启日志
				bValue = EditorGUILayout.Toggle("开启日志&GM工具?", App.instance.log);
				if (bValue != App.instance.log) {
					bSave = true;
					App.instance.log = bValue;
				}

				// 是否开启Web日志
				bValue = EditorGUILayout.Toggle("开启远程日志?", App.instance.webLog);
				if (bValue != App.instance.webLog) {
					bSave = true;
					App.instance.webLog = bValue;
				}

				// [安卓]CDN资源标签
				value = EditorGUILayout.TextField("[安卓]CDN资源标签", App.instance.androidPlatformName);
				if (value != App.instance.androidPlatformName) {
					bSave = true;
					App.instance.androidPlatformName = value;
				}

				// [苹果]CDN资源标签
				value = EditorGUILayout.TextField("[苹果]CDN资源标签", App.instance.iOSPlatformName);
				if (value != App.instance.iOSPlatformName) {
					bSave = true;
					App.instance.iOSPlatformName = value;
				}

				// [桌面]CDN资源标签
				value = EditorGUILayout.TextField("[桌面]CDN资源标签", App.instance.defaultPlatformName);
				if (value != App.instance.defaultPlatformName) {
					bSave = true;
					App.instance.defaultPlatformName = value;
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
					m_appManifest.selected = selected;
				}
				if (GUILayout.Button("编辑配置..."))
				{
                    string path = Directory.GetCurrentDirectory() + "/ProjectSettings/App.instance.ini";
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

            serializedObject.ApplyModifiedProperties();

			if (bSave)
            {
				bSave = false;

                File.WriteAllText(projectSettings, JsonWriter.Serialize(m_appManifest));
                App app = m_appManifest.list[m_appManifest.selected];
                File.WriteAllText(appPath, JsonWriter.Serialize(app));
                AssetDatabase.Refresh();
                App.instance.Init();
            }
		}
		EditorGUI.EndChangeCheck ();
        EditorGUI.EndDisabledGroup();

        // 打完整资源包应用
        if (m_buildAsset)
        {
            m_buildAsset = false;
            AssetBundleEditor.BuildAssetBundles(AssetBundleEditor.outputPath);
            AssetBundleEditor.BuildManifestFile(AssetBundleEditor.outputPath);
            AssetBundleEditor.BuildUpdateFile(AssetBundleEditor.outputPath);
            AssetBundleEditor.CopyAssetBundles();
        }
        // 打更新资源包
        if (m_buildAsset)
        {
            m_buildUpdateAsset = false;
            AssetBundleEditor.BuildAssetBundles(AssetBundleEditor.outputPath);
            AssetBundleEditor.CopyUpdateAssetBundles();
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
				App.instance.scriptingDefineSymbols = "";
				var tokens = App.instance.scriptingDefineSymbols.Split(';');
				foreach (var token in tokens)
				{
					if (token != "AB_MODE")
					{
						App.instance.scriptingDefineSymbols += token + ";";
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
				var tokens = App.instance.scriptingDefineSymbols.Split(';');
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
					if (App.instance.scriptingDefineSymbols.EndsWith (";")) {
						App.instance.scriptingDefineSymbols += "AB_MODE;";
					} else {
						App.instance.scriptingDefineSymbols += ";AB_MODE;";
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
				PlayerSettings.productName = App.instance.appName;
			} break;
		case ChangeType.BundleIdentifier:
			{
                #if UNITY_ANDROID || UNITY_IOS
				PlayerSettings.applicationIdentifier = App.instance.bundleIdentifier;
                #endif
			} break;
		case ChangeType.Version:
			{
				PlayerSettings.bundleVersion = App.instance.version;
			} break;
		case ChangeType.BundleVersionCode:
			{
				#if UNITY_ANDROID
				PlayerSettings.Android.bundleVersionCode = App.instance.bundleVersionCode;
				#elif UNITY_IOS
				PlayerSettings.iOS.buildNumber = App.instance.bundleVersionCode;
				#endif
			} break;
		case ChangeType.ScriptingDefineSymbols:
			{
				if (!PlayerSettings.GetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup).Equals (App.instance.scriptingDefineSymbols)) {
					PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, App.instance.scriptingDefineSymbols);
				}
			} break;
		}
	}
}