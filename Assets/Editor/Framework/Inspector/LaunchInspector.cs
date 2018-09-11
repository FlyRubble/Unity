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
            { App.Name, "n1" },
            { App.ProductName, "n1" },
            { App.BundleIdentifier, "n1" },
            { App.Version, "n1" },
            { App.BundleVersionCode, 0 },
            { App.ScriptingDefineSymbols, "n1" },
            { App.LoginUrl, "n1" },
            { App.Cdn, "n1" },
            { App.IsOpenGuide, true },
            { App.IsOpenUpdate, true },
            { App.IsUnlockAllFunction, false },
            { App.Log, false },
            { App.WebLog, false },
            { App.WebLogIp, "" },
            { App.AndroidPlatformName, "n1"},
            { App.IOSPlatformName, "n1"},
            { App.DefaultPlatformName, "n1"},
        },
        new Dictionary<string, object>(){
            { App.Name, "n2" },
            { App.ProductName, "n2" },
            { App.BundleIdentifier, "n2" },
            { App.Version, "n2" },
            { App.BundleVersionCode, 1 },
            { App.ScriptingDefineSymbols, "n2" },
            { App.LoginUrl, "n2" },
            { App.Cdn, "n2" },
            { App.IsOpenGuide, true },
            { App.IsOpenUpdate, true },
            { App.IsUnlockAllFunction, false },
            { App.Log, false },
            { App.WebLog, false },
            { App.WebLogIp, "" },
            { App.AndroidPlatformName, "n2"},
            { App.IOSPlatformName, "n2"},
            { App.DefaultPlatformName, "n2"},
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
			value = EditorGUILayout.TextField("Product Name", m_select[App.ProductName].ToString());
			if (value != m_select[App.ProductName].ToString())
            {
				bSave = true;
                m_select[App.ProductName] = value;
				ChangeSettings (ChangeType.ProductName);
			}
			// 包名
			value = EditorGUILayout.TextField("Bundle Identifier", m_select[App.BundleIdentifier].ToString());
			if (value != m_select[App.BundleIdentifier].ToString()) {
				bSave = true;
                m_select[App.BundleIdentifier] = value;
				ChangeSettings (ChangeType.BundleIdentifier);
			}
			// 版本
			value = EditorGUILayout.TextField("Version*", m_select[App.Version].ToString());
			if (value != m_select[App.Version].ToString()) {
				bSave = true;
                m_select[App.Version] = value;
				ChangeSettings (ChangeType.Version);
			}
            // 版本Code
#if UNITY_ANDROID
			int bundleVersionCode = EditorGUILayout.IntField("BundleVersionCode", (int)m_select[App.BundleVersionCode]);
			if (PlayerSettings.Android.bundleVersionCode != bundleVersionCode)
			{
				bSave = true;
                m_select[App.BundleVersionCode] = bundleVersionCode;
				ChangeSettings (ChangeType.BundleVersionCode);
			}
#elif UNITY_IOS
            int buildNumber = EditorGUILayout.IntField("BuildNumber", (int)m_select[App.BundleVersionCode]);
            if (PlayerSettings.iOS.buildNumber != buildNumber.ToString())
            {
                bSave = true;
                m_select[App.BundleVersionCode] = buildNumber;
                ChangeSettings(ChangeType.BundleVersionCode);
            }
#else
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.IntField("BundleVersionCode", (int)m_select[App.BundleVersionCode]);
			EditorGUILayout.LabelField("*仅Android或IOS有效");
			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
#endif

            // 平台、宏定义
            EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField("Platform", EditorUserBuildSettings.activeBuildTarget.ToString());
            EditorGUILayout.TextField("Scripting Define Symbol", m_select[App.ScriptingDefineSymbols].ToString());
            EditorGUI.EndDisabledGroup();

            // 设置配置选项组
            selected = EditorGUILayout.Popup("服务器配置", m_nameList.IndexOf(m_select[App.Name].ToString()), m_nameList.ToArray());
            if (selected != m_nameList.IndexOf(m_select[App.Name].ToString()))
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
				value = EditorGUILayout.TextField("登录地址", m_select[App.LoginUrl].ToString());
				if (value != m_select[App.LoginUrl].ToString()) {
					bSave = true;
                    m_select[App.LoginUrl] = value;
				}

				// CDN
				value = EditorGUILayout.TextField("CDN资源地址", m_select[App.Cdn].ToString());
				if (value != m_select[App.Cdn].ToString()) {
					bSave = true;
                    m_select[App.Cdn] = value;
				}

				// 是否开启引导
				bool bValue = EditorGUILayout.Toggle("开启新手引导?", (bool)m_select[App.IsOpenGuide]);
				if (bValue != (bool)m_select[App.IsOpenGuide]) {
					bSave = true;
                    m_select[App.IsOpenGuide] = bValue;
				}

				// 是否开启更新模式
				bValue = EditorGUILayout.Toggle("开启资源更新?", (bool)m_select[App.IsOpenUpdate]);
				if (bValue != (bool)m_select[App.IsOpenUpdate]) {
					bSave = true;
                    m_select[App.IsOpenUpdate] = bValue;
				}

				// 是否完全解锁所有功能
				bValue = EditorGUILayout.Toggle("开启所有功能?", (bool)m_select[App.IsUnlockAllFunction]);
				if (bValue != (bool)m_select[App.IsUnlockAllFunction]) {
					bSave = true;
                    m_select[App.IsUnlockAllFunction] = bValue;
				}

				// 是否开启日志
				bValue = EditorGUILayout.Toggle("开启日志&GM工具?", (bool)m_select[App.Log]);
				if (bValue != (bool)m_select[App.Log]) {
					bSave = true;
                    m_select[App.Log] = bValue;
				}

				// 是否开启Web日志
				bValue = EditorGUILayout.Toggle("开启远程日志?", (bool)m_select[App.WebLog]);
				if (bValue != (bool)m_select[App.WebLog]) {
					bSave = true;
                    m_select[App.WebLog] = bValue;
				}

                // 远程日志白名单
                value = EditorGUILayout.TextField("远程日志白名单", m_select[App.WebLogIp].ToString());
                if (value != m_select[App.WebLogIp].ToString())
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
                    m_select[App.WebLogIp] = ip;
                }

                // [安卓]CDN资源标签
                value = EditorGUILayout.TextField("[安卓]CDN资源标签", m_select[App.AndroidPlatformName].ToString());
				if (value != m_select[App.AndroidPlatformName].ToString()) {
					bSave = true;
                    m_select[App.AndroidPlatformName] = value;
				}

				// [苹果]CDN资源标签
				value = EditorGUILayout.TextField("[苹果]CDN资源标签", m_select[App.IOSPlatformName].ToString());
				if (value != m_select[App.IOSPlatformName].ToString()) {
					bSave = true;
                    m_select[App.IOSPlatformName] = value;
				}

				// [桌面]CDN资源标签
				value = EditorGUILayout.TextField("[桌面]CDN资源标签", m_select[App.DefaultPlatformName].ToString());
				if (value != m_select[App.DefaultPlatformName].ToString()) {
					bSave = true;
                    m_select[App.DefaultPlatformName] = value;
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
			// 清除本地资源解压缓存
			if (GUILayout.Button ("清除本地资源解压缓存")) {
				FileUtil.DeleteFileOrDirectory (Application.persistentDataPath);
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
                    App.ProductName,
                    App.Version,
                    App.LoginUrl,
                    App.Cdn,
                    App.IsOpenGuide,
                    App.IsOpenUpdate,
                    App.IsUnlockAllFunction,
                    App.Log,
                    App.WebLog,
                    App.WebLogIp,
                    App.AndroidPlatformName,
                    App.IOSPlatformName,
                    App.DefaultPlatformName
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
            m_select[App.ScriptingDefineSymbols] = value;
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
            m_select[App.ScriptingDefineSymbols] = value;
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
            PlayerSettings.productName = m_select[App.ProductName].ToString();
        }
        break;
        case ChangeType.BundleIdentifier:
        {
#if UNITY_ANDROID || UNITY_IOS
            PlayerSettings.applicationIdentifier = m_select[App.BundleIdentifier].ToString();
#endif
        }
        break;
        case ChangeType.Version:
        {
            PlayerSettings.bundleVersion = m_select[App.Version].ToString();
        }
        break;
        case ChangeType.BundleVersionCode:
        {
#if UNITY_ANDROID
            PlayerSettings.Android.bundleVersionCode = (int)m_select[App.BundleVersionCode];
#elif UNITY_IOS
            PlayerSettings.iOS.buildNumber = m_select[App.BundleVersionCode].ToString();
#endif
        }
        break;
        case ChangeType.ScriptingDefineSymbols:
        {
            string value = m_select[App.ScriptingDefineSymbols].ToString();
            if (!PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Equals(value))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, value);
            }
        }
        break;
        }
    }
}