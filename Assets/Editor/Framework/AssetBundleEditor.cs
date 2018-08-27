﻿using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Framework.IO;

/// <summary>
/// 资源打包
/// </summary>
public class AssetBundleEditor
{
    /// <summary>
    /// AssetBundle File Info
    /// </summary>
    struct ABFI
    {
        public string md5;
        public long size;
    }

    /// <summary>
    /// 得到资源文件夹路径
    /// </summary>
    /// <value>The asset path.</value>
    public static string assetPath
    {
        get { return Directory.GetCurrentDirectory() + "/Assets"; }
    }

    /// <summary>
    /// 输出路径
    /// </summary>
    /// <value>The output path.</value>
    public static string outputPath
    {
        get { return assetPath + "/../StreamingAssets/" + buildTarget.ToString(); }
    }

    /// <summary>
    /// 输出清单文件路径
    /// </summary>
    /// <value>The outputmanifest path.</value>
    public static string outputManifestPath
    {
        get { return outputPath + "/" + buildTarget.ToString(); }
    }

    /// <summary>
    /// 输出清单文件路径
    /// </summary>
    public static string outputManifestFilePath
    {
        get { return outputPath + "/manifest.txt"; }
    }

    /// <summary>
    /// 输出更新清单文件路径
    /// </summary>
    public static string outputUpdateManifestFilePath
    {
        get { return outputPath + "/update.txt"; }
    }

    /// <summary>
    /// 输出版本路径
    /// </summary>
    public static string outputVersionPath
    {
        get { return assetPath + "/../Version"; }
    }

    /// <summary>
    /// StreamingAssets文件夹路径
    /// </summary>
    public static string streamingAssets
    {
        get { return assetPath + "/StreamingAssets"; }
    }

    /// <summary>
    /// 资源打包
    /// </summary>
    public static void BuildAssetBundles()
    {
        // 移除所有assetBundleName
        string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        foreach (var assetBundleName in assetBundleNames)
        {
            AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
        }
        // 设置assetBundleName
        string dataPath = assetPath + "/data";
        string[] paths = Directory.GetFiles(dataPath, "*.*", SearchOption.AllDirectories);
        foreach (var path in paths)
        {
            if (path.Contains("/Resources/") || path.Contains(@"\Resources\") || path.Contains("/resources/") || path.Contains(@"\resources\"))
            {
                continue;
            }
            string extension = Path.GetExtension(path);
            switch (extension)
            {
            case ".meta":
            case ".cs":
            { }
            break;
            default:
            {
                string relativePath = path.Replace(assetPath, "Assets");
                var asset = AssetImporter.GetAtPath(relativePath);
                asset.assetBundleName = GetAssetBundleName(path);
            }
            break;
            }
        }

        // 打包
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
        // 移除所有assetBundleName
        assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        foreach (var assetBundleName in assetBundleNames)
        {
            AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
        }
        // 刷新
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 拷贝资源包
    /// </summary>
    public static void CopyAssetBundles()
    {
        ManifestConfig manifestConfig = GetManifest();
        // 写入Manifest
        if (manifestConfig != null)
        {
            // 写入到文件
            manifestConfig.Write();
            // 拷贝资源
            if (Directory.Exists(streamingAssets))
            {
                Directory.Delete(streamingAssets, true);
            }
            Directory.CreateDirectory(streamingAssets);

            string path = streamingAssets + "/manifest.xml";
            ManifestConfig oldManifest = new ManifestConfig(File.Exists(path) ? path : "");
            oldManifest.Read();
            foreach (var data in manifestConfig.data.Values)
            {
                if (oldManifest.data.ContainsKey(data.name) && oldManifest.data[data.name].MD5 == data.MD5)
                {

                }
                else
                {
                    path = streamingAssets + "/" + data.name;
                    DirectoryInfo info = Directory.GetParent(path);
                    if (!info.Exists)
                    {
                        Directory.CreateDirectory(info.FullName);
                    }
                    FileUtil.CopyFileOrDirectory(outputPath + "/" + data.name, path);
                }
            }
            File.Copy(outputManifestFilePath, assetPath + "/data/resources/manifest.txt", true);
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// 拷贝更新资源包
    /// </summary>
    public static void CopyUpdateAssetBundles()
    {
        ManifestConfig manifestConfig = GetManifest();
        // 写入Manifest
        if (manifestConfig != null)
        {
            // 旧版本清单
            ManifestConfig localManifestConfig = new ManifestConfig(outputManifestFilePath);
            localManifestConfig.Read();
            // 更新文件清单
            ManifestConfig localUpdateManifestConfig = new ManifestConfig(outputUpdateManifestFilePath);
            localUpdateManifestConfig.Read();
            // 版本文件比较
            ManifestConfig updateManifestConfig = new ManifestConfig();
            List<string> paths = new List<string>();
            foreach (var data in manifestConfig.data)
            {
                if (localManifestConfig.data.ContainsKey(data.Key) && localManifestConfig.data[data.Key].MD5.Equals(data.Value.MD5))
                {
                    continue;
                }
                updateManifestConfig.Add(data.Value);
                if (localUpdateManifestConfig.data.ContainsKey(data.Key) && localUpdateManifestConfig.data[data.Key].MD5.Equals(data.Value.MD5))
                {
                    continue;
                }
                paths.Add(data.Value.name);
            }

            // 输出文件
            if (paths.Count > 0)
            {
                AppConfig.Init();
                // 文件夹
                string directory = string.Format(outputVersionPath + "/v{0}", AppConfig.version);
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
                Directory.CreateDirectory(directory);
                // 文件
                updateManifestConfig.Write(outputUpdateManifestFilePath);
                updateManifestConfig.Write(directory + "/update.txt");
                foreach (var data in paths)
                {
                    string path = directory + "/" + data;
                    DirectoryInfo info = Directory.GetParent(path);
                    if (!info.Exists)
                    {
                        Directory.CreateDirectory(info.FullName);
                    }
                    File.Copy(outputPath + "/" + data, directory + "/" + data, true);
                }
            }
        }
    }

    /// <summary>
    /// 打包目标平台
    /// </summary>
    /// <value>The build target.</value>
    static BuildTarget buildTarget
    {
        get
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows64;//EditorUserBuildSettings.activeBuildTarget;
#if UNITY_ANDROID
			buildTarget = BuildTarget.Android;
#elif UNITY_IPHONE
			buildTarget = BuildTarget.iOS;
#elif UNITY_EDITOR_WIN && UNITY_EDITOR_64
            buildTarget = BuildTarget.StandaloneWindows64;
#elif UNITY_EDITOR_WIN && UNITY_EDITOR
			buildTarget = BuildTarget.StandaloneWindows;
#endif
            return buildTarget;
        }
    }

    /// <summary>
    /// 得到AssetBundleName
    /// </summary>
    /// <returns>The asset bundle name.</returns>
    /// <param name="path">Path.</param>
    /// <param name="stopFileName">Stop file name.</param>
    static string GetAssetBundleName(string path, string stopFileName = "Assets")
    {
        string name = Path.GetFileName(path);
        DirectoryInfo info = Directory.GetParent(path);
        if (!info.Name.Equals(stopFileName))
        {
            name = GetAssetBundleName(info.FullName) + "/" + name;
        }
        return name;
    }

    /// <summary>
    /// 版本比较
    /// </summary>
    /// <returns><c>true</c>, if compare was versioned, <c>false</c> otherwise.</returns>
    /// <param name="newManifest">New manifest.</param>
    /// <param name="oldManifest">Old manifest.</param>
    bool VersionCompare(string newManifest, string oldManifest)
    {
        bool bVersion = false;
        string[] newVersion = newManifest.Split('.');
        string[] oldVersion = oldManifest.Split('.');
        if (newVersion.Length == oldVersion.Length && oldVersion.Length == 3)
        {
            int[] newVer = new int[3];
            int[] oldVer = new int[3];
            if (int.TryParse(newVersion[0], out newVer[0]) && int.TryParse(oldVersion[0], out oldVer[0])
                && int.TryParse(newVersion[1], out newVer[1]) && int.TryParse(oldVersion[1], out oldVer[1])
                && int.TryParse(newVersion[2], out newVer[2]) && int.TryParse(oldVersion[2], out oldVer[2]))
            {
                if (newVer[0] > oldVer[0]) { bVersion = true; }
                else if (newVer[1] > oldVer[1]) { bVersion = true; }
                else if (newVer[2] > oldVer[2]) { bVersion = true; }
            }
        }
        return bVersion;
    }

    /// <summary>
    /// 得到MD5
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static ABFI GetABFI(string path)
    {
        ABFI ab = new ABFI();
        try
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            ab.size = Mathf.CeilToInt((fs.Length / 1024f));
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            ab.md5 = sb.ToString();
        }
        catch (System.Exception e)
        {
            throw new System.Exception(e.Message);
        }
        return ab;
    }

    /// <summary>
    /// 得到清单文件
    /// </summary>
    /// <returns></returns>
    private static ManifestConfig GetManifest()
    {
        ManifestConfig manifestConfig = null;
        if (File.Exists(outputManifestPath))
        {
            manifestConfig = new ManifestConfig(outputManifestFilePath);
            var bundle = AssetBundle.LoadFromFile(outputManifestPath);
            AssetBundleManifest abManifest = bundle.LoadAsset("assetbundlemanifest") as AssetBundleManifest;
            string[] bundleNames = abManifest.GetAllAssetBundles();
            for (int i = 0; i < bundleNames.Length; ++i)
            {
                Manifest manifest = new Manifest();
                manifest.name = bundleNames[i];
                ABFI ab = GetABFI(outputPath + "/" + bundleNames[i]);
                manifest.MD5 = ab.md5;
                manifest.size = ab.size;
                foreach (var dependenciesName in abManifest.GetDirectDependencies(bundleNames[i]))
                {
                    manifest.directDependencies.Add(dependenciesName);
                }
                manifestConfig.Add(manifest);
            }
        }
        return manifestConfig;
    }
}