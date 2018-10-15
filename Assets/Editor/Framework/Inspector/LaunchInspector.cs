using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Framework.JsonFx;

/// <summary>
/// 启动监视器
/// </summary>
[CustomEditor(typeof(Launch))]
public class LaunchInspector : Editor
{
	enum ChangeType
    {
        ProductName,
        BundleIdentifier,
		Version,
		BundleVersionCode,
		ScriptingDefineSymbols
	}

    /// <summary>
    /// 数据
    /// </summary>
    private List<Dictionary<string, object>> m_data = new List<Dictionary<string, object>>() {
        new Dictionary<string, object>(){
            { Const.NAME, "自定义" },
            { Const.PRODUCT_NAME, "街头篮球" },
            { Const.BUNDLE_IDENTIFIER, "com.playgame.jtlq" },
            { Const.VERSION, "1.0.0" },
            { Const.BUNDLE_VERSION_CODE, 1 },
            { Const.SCRIPTING_DEFINE_SYMBOLS, "" },
            { Const.LOGIN_URL, "http://localhost/" },
            { Const.CDN, "http://localhost/" },
            { Const.OPEN_GUIDE, true },
            { Const.OPEN_UPDATE, true },
            { Const.UNLOCK_ALL_FUNCTION, false },
            { Const.LOG, false },
            { Const.WEB_LOG, false },
            { Const.WEB_LOG_IP, "" },
            { Const.ANDROID_PLATFORM_NAME, "Android"},
            { Const.IOS_PLATFORM_NAME, "IOS"},
            { Const.DEFAULT_PLATFORM_NAME, "PC"},
        },
        new Dictionary<string, object>(){
            { Const.NAME, "本地测试" },
            { Const.PRODUCT_NAME, "街头篮球" },
            { Const.BUNDLE_IDENTIFIER, "com.playgame.jtlq" },
            { Const.VERSION, "1.0.0" },
            { Const.BUNDLE_VERSION_CODE, 1 },
            { Const.SCRIPTING_DEFINE_SYMBOLS, "" },
            { Const.LOGIN_URL, "http://localhost/" },
            { Const.CDN, "http://localhost/" },
            { Const.OPEN_GUIDE, true },
            { Const.OPEN_UPDATE, true },
            { Const.UNLOCK_ALL_FUNCTION, false },
            { Const.LOG, true },
            { Const.WEB_LOG, false },
            { Const.WEB_LOG_IP, "" },
            { Const.ANDROID_PLATFORM_NAME, "Android"},
            { Const.IOS_PLATFORM_NAME, "IOS"},
            { Const.DEFAULT_PLATFORM_NAME, "PC"},
        }
    };

    /// <summary>
    /// 选中的数据
    /// </summary>
    private Dictionary<string, object> m_select = new Dictionary<string, object>();

    /// <summary>
    /// 名字表
    /// </summary>
    private List<string> m_nameList = new List<string>();

    /// <summary>
    /// 是否打包完整资源
    /// </summary>
    private bool m_buildAsset = false;

    /// <summary>
    /// 是否打包更新资源
    /// </summary>
    private bool m_buildUpdateAsset = false;

    /// <summary>
    /// 路径
    /// </summary>
    public string path
    {
        get { return Directory.GetCurrentDirectory() + "/ProjectSettings/Setting.asset"; }
    }

    /// <summary>
    /// version路径
    /// </summary>
    public string versionJson
    {
        get { return Directory.GetCurrentDirectory() + "/Assets/data/resources/version.json"; }
    }

    /// <summary>
    /// 名字列表
    /// </summary>
    public void SetNameList()
    {
        m_nameList.Clear();
        foreach (var data in m_data)
        {
            m_nameList.Add(data["name"].ToString());
        }
    }

    /// <summary>
    /// OnEnable
    /// </summary>
    private void OnEnable()
    {
        SetNameList();

        var select = m_data[0];
        if (!File.Exists(path))
        {
            File.WriteAllText(path, JsonWriter.Serialize(m_data[0]));
            AssetDatabase.Refresh();
        }
        else
        {
            m_data[0] = JsonReader.Deserialize<Dictionary<string, object>>(File.ReadAllText(path));
        }
        int selected = m_data[0].ContainsKey("selected") ? (int)m_data[0]["selected"] : 0;
        selected = selected < m_data.Count ? selected : 0;
        m_select = m_data[selected];
        foreach (var data in select)
        {
            if (!m_select.ContainsKey(data.Key))
            {
                m_select.Add(data.Key, data.Value);
            }
        }

        if (!File.Exists(versionJson))
        {
            SaveVersion();
            AssetDatabase.Refresh();
        }
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
            // 模式
            string value = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var index = value.Contains("AB_MODE") ? 1 : 0;
			var selected = GUILayout.Toolbar(index, new string[] { "编辑器模式", "高级AB模式" });
			if (index != selected)
            {
                bSave = true;
                ModeToggle (selected);
            }
            // 产品名字
			value = EditorGUILayout.TextField("Product Name", m_select[Const.PRODUCT_NAME].ToString());
			if (value != m_select[Const.PRODUCT_NAME].ToString())
            {
				bSave = true;
                m_select[Const.PRODUCT_NAME] = value;
				ChangeSettings (ChangeType.ProductName);
			}
			// 包名
			value = EditorGUILayout.TextField("Bundle Identifier", m_select[Const.BUNDLE_IDENTIFIER].ToString());
			if (value != m_select[Const.BUNDLE_IDENTIFIER].ToString()) {
				bSave = true;
                m_select[Const.BUNDLE_IDENTIFIER] = value;
				ChangeSettings (ChangeType.BundleIdentifier);
			}
			// 版本
			value = EditorGUILayout.TextField("Version*", m_select[Const.VERSION].ToString());
			if (value != m_select[Const.VERSION].ToString()) {
				bSave = true;
                m_select[Const.VERSION] = value;
				ChangeSettings (ChangeType.Version);
			}
            // 版本Code
#if UNITY_ANDROID
			int bundleVersionCode = EditorGUILayout.IntField("BundleVersionCode", (int)m_select[Const.BUNDLE_VERSION_CODE]);
			if (PlayerSettings.Android.bundleVersionCode != bundleVersionCode)
			{
				bSave = true;
                m_select[Const.BUNDLE_VERSION_CODE] = bundleVersionCode;
				ChangeSettings (ChangeType.BundleVersionCode);
			}
#elif UNITY_IOS
            int buildNumber = EditorGUILayout.IntField("BuildNumber", (int)m_select[Const.BUNDLE_VERSION_CODE]);
            if (PlayerSettings.iOS.buildNumber != buildNumber.ToString())
            {
                bSave = true;
                m_select[Const.BUNDLE_VERSION_CODE] = buildNumber;
                ChangeSettings(ChangeType.BundleVersionCode);
            }
#else
            EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.IntField("BundleVersionCode", (int)m_select[Const.BUNDLE_VERSION_CODE]);
			EditorGUILayout.LabelField("*仅Android或IOS有效");
			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
#endif

            // 平台、宏定义
            EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Platform", EditorUserBuildSettings.activeBuildTarget.ToString());
            EditorGUILayout.TextField("Scripting Define Symbol", m_select[Const.SCRIPTING_DEFINE_SYMBOLS].ToString());
            EditorGUI.EndDisabledGroup();

            // 设置配置选项组
            selected = EditorGUILayout.Popup("服务器配置", m_nameList.IndexOf(m_select[Const.NAME].ToString()), m_nameList.ToArray());
            if (selected != m_nameList.IndexOf(m_select[Const.NAME].ToString()))
            {
                bSave = true;
                m_select = m_data[selected];

                ChangeSettings(ChangeType.ProductName);
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
			EditorGUI.BeginDisabledGroup(selected != 0);
            {
				// 登陆地址
				value = EditorGUILayout.TextField("登录地址", m_select[Const.LOGIN_URL].ToString());
				if (value != m_select[Const.LOGIN_URL].ToString()) {
					bSave = true;
                    m_select[Const.LOGIN_URL] = value;
				}

				// CDN
				value = EditorGUILayout.TextField("CDN资源地址", m_select[Const.CDN].ToString());
				if (value != m_select[Const.CDN].ToString()) {
					bSave = true;
                    m_select[Const.CDN] = value;
				}

				// 是否开启引导
				bool bValue = EditorGUILayout.Toggle("开启新手引导?", (bool)m_select[Const.OPEN_GUIDE]);
				if (bValue != (bool)m_select[Const.OPEN_GUIDE]) {
					bSave = true;
                    m_select[Const.OPEN_GUIDE] = bValue;
				}

				// 是否开启更新模式
				bValue = EditorGUILayout.Toggle("开启资源更新?", (bool)m_select[Const.OPEN_UPDATE]);
				if (bValue != (bool)m_select[Const.OPEN_UPDATE]) {
					bSave = true;
                    m_select[Const.OPEN_UPDATE] = bValue;
				}

				// 是否完全解锁所有功能
				bValue = EditorGUILayout.Toggle("开启所有功能?", (bool)m_select[Const.UNLOCK_ALL_FUNCTION]);
				if (bValue != (bool)m_select[Const.UNLOCK_ALL_FUNCTION]) {
					bSave = true;
                    m_select[Const.UNLOCK_ALL_FUNCTION] = bValue;
				}

				// 是否开启日志
				bValue = EditorGUILayout.Toggle("开启日志&GM工具?", (bool)m_select[Const.LOG]);
				if (bValue != (bool)m_select[Const.LOG]) {
					bSave = true;
                    m_select[Const.LOG] = bValue;
				}

				// 是否开启Web日志
				bValue = EditorGUILayout.Toggle("开启远程日志?", (bool)m_select[Const.WEB_LOG]);
				if (bValue != (bool)m_select[Const.WEB_LOG]) {
					bSave = true;
                    m_select[Const.WEB_LOG] = bValue;
				}

                // 远程日志白名单
                value = EditorGUILayout.TextField("远程日志白名单", m_select[Const.WEB_LOG_IP].ToString());
                if (value != m_select[Const.WEB_LOG_IP].ToString())
                {
                    bSave = true;
                    string ip = string.Empty;
                    string[] array = value.Split(',', ';', '|');
                    for (int i = 0; i < array.Length; ++i)
                    {
                        if (!string.IsNullOrEmpty(array[i]))
                        {
                            ip += ";" + array[i];
                        }
                    }
                    if (ip.StartsWith(";"))
                    {
                        ip = ip.Substring(1, ip.Length - 1);
                    }
                    m_select[Const.WEB_LOG_IP] = ip;
                }

                // [安卓]CDN资源标签
                value = EditorGUILayout.TextField("[安卓]CDN资源标签", m_select[Const.ANDROID_PLATFORM_NAME].ToString());
				if (value != m_select[Const.ANDROID_PLATFORM_NAME].ToString()) {
					bSave = true;
                    m_select[Const.ANDROID_PLATFORM_NAME] = value;
				}

				// [苹果]CDN资源标签
				value = EditorGUILayout.TextField("[苹果]CDN资源标签", m_select[Const.IOS_PLATFORM_NAME].ToString());
				if (value != m_select[Const.IOS_PLATFORM_NAME].ToString()) {
					bSave = true;
                    m_select[Const.IOS_PLATFORM_NAME] = value;
				}

				// [桌面]CDN资源标签
				value = EditorGUILayout.TextField("[桌面]CDN资源标签", m_select[Const.DEFAULT_PLATFORM_NAME].ToString());
				if (value != m_select[Const.DEFAULT_PLATFORM_NAME].ToString()) {
					bSave = true;
                    m_select[Const.DEFAULT_PLATFORM_NAME] = value;
				}
            }
			EditorGUI.EndDisabledGroup();

			if (selected != 0)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("复制到自定义..."))
				{
                    bSave = true;
                    foreach (var kvp in m_select)
                    {
                        if (kvp.Key.Equals(Const.NAME)) continue;
                        if (m_data[0].ContainsKey(kvp.Key))
                        {
                            m_data[0][kvp.Key] = kvp.Value;
                        }
                        else
                        {
                            m_data[0].Add(kvp.Key, kvp.Value);
                        }
                    }
                    m_select = m_data[0];

                    ChangeSettings(ChangeType.ProductName);
                    ChangeSettings(ChangeType.BundleIdentifier);
                    ChangeSettings(ChangeType.Version);
                    ChangeSettings(ChangeType.BundleVersionCode);
                    ChangeSettings(ChangeType.ScriptingDefineSymbols);
                }
                if (GUILayout.Button("编辑配置..."))
                {
                    if (File.Exists(versionJson))
                    {
                        System.Diagnostics.Process.Start(versionJson);
                    }
                }
                EditorGUILayout.EndVertical();
			}
            // 打开本地沙盒资源目录
            if (GUILayout.Button("打开本地沙盒资源目录"))
            {
                string persistentDataPath = Application.persistentDataPath.Replace("/", "\\");
                System.Diagnostics.Process.Start("explorer.exe", persistentDataPath);
            }
            // 清除本地沙盒资源
            if (GUILayout.Button ("清除本地沙盒资源")) {
                PlayerPrefs.SetString(Const.SANDBOX_VERSION, null);
            }

            value = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            index = value.Contains("AB_MODE") ? 1 : 0;
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
                if (m_data[0].ContainsKey("selected"))
                {
                    m_data[0]["selected"] = selected;
                }
                else
                {
                    m_data[0].Add("selected", selected);
                }
                File.WriteAllText(path, JsonWriter.Serialize(m_data[0]));
                SaveVersion();
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
            AssetBundleEditor.BuildUpdateAssetBundlesAndZip(AssetBundleEditor.outputPath, AssetBundleEditor.outputVersionPath, App.version, App.platform, App.cdn);
            GUIUtility.ExitGUI();
        }

    }

    /// <summary>
    /// 保存版本配置
    /// </summary>
    private void SaveVersion()
    {
        string[] list = new string[] {
                    Const.PRODUCT_NAME,
                    Const.VERSION,
                    Const.LOGIN_URL,
                    Const.CDN,
                    Const.OPEN_GUIDE,
                    Const.OPEN_UPDATE,
                    Const.UNLOCK_ALL_FUNCTION,
                    Const.LOG,
                    Const.WEB_LOG,
                    Const.WEB_LOG_IP,
                    Const.ANDROID_PLATFORM_NAME,
                    Const.IOS_PLATFORM_NAME,
                    Const.DEFAULT_PLATFORM_NAME
                };
        Dictionary<string, object> dic = new Dictionary<string, object>();
        for (int i = 0; i < list.Length; ++i)
        {
            if (m_select.ContainsKey(list[i]))
            {
                dic.Add(list[i], m_select[list[i]]);
            }
        }
        File.WriteAllText(versionJson, JsonWriter.Serialize(dic));
    }

    /// <summary>
    /// 模式切换
    /// </summary>
    /// <param name="selected">Selected.</param>
    public void ModeToggle(int selected)
    {
        string value = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        switch (selected) {
		case 0:
			{
            var tokens = value.Split(';');
            value = string.Empty;
            foreach (var token in tokens)
            {
                if (token != "AB_MODE" && !string.IsNullOrEmpty(token))
                {
                    value += token + ";";
                }
            }
            if (value.EndsWith(";"))
            {
                value = value.Substring(0, value.Length - 1);
            }
            m_select[Const.SCRIPTING_DEFINE_SYMBOLS] = value;
            ChangeSettings(ChangeType.ScriptingDefineSymbols);
        } break;
		case 1:
			{
            var tokens = value.Split(';');
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
                if (value.Length == 0)
                {
                    value = "AB_MODE";
                }
                else
                {
                    value = "AB_MODE;" + value;
                }
            }
            m_select[Const.SCRIPTING_DEFINE_SYMBOLS] = value;
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
        switch (type)
        {
        case ChangeType.ProductName:
        {
            PlayerSettings.productName = m_select[Const.PRODUCT_NAME].ToString();
        }
        break;
        case ChangeType.BundleIdentifier:
        {
#if UNITY_ANDROID || UNITY_IOS
            PlayerSettings.applicationIdentifier = m_select[Const.BUNDLE_IDENTIFIER].ToString();
#endif
        }
        break;
        case ChangeType.Version:
        {
            PlayerSettings.bundleVersion = m_select[Const.VERSION].ToString();
        }
        break;
        case ChangeType.BundleVersionCode:
        {
#if UNITY_ANDROID
            PlayerSettings.Android.bundleVersionCode = (int)m_select[Const.BUNDLE_VERSION_CODE];
#elif UNITY_IOS
            PlayerSettings.iOS.buildNumber = m_select[Const.BUNDLE_VERSION_CODE].ToString();
#endif
        }
        break;
        case ChangeType.ScriptingDefineSymbols:
        {
            string value = m_select[Const.SCRIPTING_DEFINE_SYMBOLS].ToString();
            if (!PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Equals(value))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, value);
            }
        }
        break;
        }
    }
}