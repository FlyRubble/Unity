using UnityEngine;
using UnityEditor;
using System.IO;
using Framework.IO;
using Framework.JsonFx;
using ICSharpCode.SharpZipLib.Zip;
using System;
using UnityEngine.U2D;
using UnityEditor.U2D;

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
    /// 资源打包并拷贝
    /// </summary>
    /// <param name="output"></param>
    public static void BuildAssetBundlesAndCopy(string output, bool rebuild)
    {
        BuildAssetBundles(output, rebuild);
        BuildManifestFile(output);
        BuildUpdateFile(output);
        CopyAssetBundles(output);
    }

    /// <summary>
    /// 资源打包并压缩更新资源包
    /// </summary>
    /// <param name="output"></param>
    /// <param name="dest"></param>
    /// <param name="version"></param>
    /// <param name="platform"></param>
    /// <param name="cdn"></param>
    public static void BuildUpdateAssetBundlesAndZip(string output, string dest, string version, string platform, bool rebuild, string cdn = null)
    {
        BuildAssetBundles(output, rebuild);
        BuildManifestFile(output);
        BuildUpdateFile(output, cdn + "/" + platform + "/v" + version);
        CopyUpdateAssetBundles(output, dest, version, cdn + "/" + platform);
    }

    /// <summary>
    /// 资源打包
    /// </summary>
    public static void BuildAssetBundles(string output, bool rebuild)
    {
        // 移除所有assetBundleName
        string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        foreach (var assetBundleName in assetBundleNames)
        {
            AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
        }
        // 设置图集
        string dataPath = assetPath + "/data/texture";
        string[] directoryPaths = Directory.GetDirectories(dataPath, "*.atlas", SearchOption.AllDirectories);
        foreach (var directoryPath in directoryPaths)
        {
            string relativePath = directoryPath.Replace(assetPath, "Assets");
            SetSpriteAtlas(relativePath);
        }
        // 设置assetBundleName
        dataPath = assetPath + "/data";
        string[] filePaths = Directory.GetFiles(dataPath, "*.*", SearchOption.AllDirectories);
        foreach (var filePath in filePaths)
        {
            string path = filePath;
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
            continue;
            }
            

            if (Path.GetDirectoryName(path).EndsWith(".atlas"))
            {
                continue;
            }
            string relativePath = path.Replace(assetPath, "Assets");
            var asset = AssetImporter.GetAtPath(relativePath);
            switch (extension)
            {
            case ".lua":
            {
                path = path.Substring(0, path.Length - ".lua".Length) + ".txt";
            }
            break;
            case ".unity":
            {
                path = path.Substring(0, path.Length - ".unity".Length) + ".scene";
            }
            break;
            }
            asset.assetBundleName = GetAssetBundleName(path);
        }

        // 打包
        if (rebuild && Directory.Exists(output))
        {
            FileUtil.DeleteFileOrDirectory(output);
            AssetDatabase.Refresh();
        }
        Directory.CreateDirectory(output);
        AssetDatabase.Refresh();
        SpriteAtlasUtility.PackAllAtlases(buildTarget);
        BuildPipeline.BuildAssetBundles(output, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
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
    /// 清单文件
    /// </summary>
    /// <param name="output"></param>
    public static void BuildManifestFile(string output)
    {
        ManifestConfig manifestConfig = GetManifest(output);
        // 写入Manifest
        if (manifestConfig != null)
        {
            // 写入到文件
            File.WriteAllText(assetPath + "/data/conf/manifestfile.json", JsonWriter.Serialize(manifestConfig));
            // 刷新
            AssetDatabase.Refresh();
            // Build清单文件
            AssetBundleBuild[] builds = new AssetBundleBuild[1];
            builds[0].assetBundleName = "data/conf/manifestfile";
            builds[0].assetBundleVariant = null;
            builds[0].assetNames = new string[1] { assetPath + "/data/conf/manifestfile.json" };
            BuildPipeline.BuildAssetBundles(output, builds, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
        }
    }

    /// <summary>
    /// 更新文件
    /// </summary>
    /// <param name="output"></param>
    public static void BuildUpdateFile(string output, string cdn = null)
    {
        ManifestConfig newManifestConfig = GetManifest(output);

        ManifestConfig oldManifestConfig = newManifestConfig;
        if (!string.IsNullOrEmpty(cdn))
        {
            string url = cdn + "/data/conf/manifestfile.json";
            WWW www = new WWW(url);
            while (!www.isDone) ;
            if (string.IsNullOrEmpty(www.error) && www.progress == 1f)
            {
                TextAsset text = www.assetBundle.LoadAsset(Path.GetFileNameWithoutExtension(url)) as TextAsset;
                oldManifestConfig = JsonReader.Deserialize<ManifestConfig>(text.text);
                www.assetBundle.Unload(true);
            }
            www.Dispose();
        }

        ManifestConfig manifestConfig = new ManifestConfig();
        if (!string.IsNullOrEmpty(cdn))
        {
            string url = cdn + "/data/conf/updatefile.json";
            WWW www = new WWW(url);
            while (!www.isDone) ;
            if (string.IsNullOrEmpty(www.error) && www.progress == 1f)
            {
                TextAsset text = www.assetBundle.LoadAsset(Path.GetFileNameWithoutExtension(url)) as TextAsset;
                manifestConfig = JsonReader.Deserialize<ManifestConfig>(text.text);
                www.assetBundle.Unload(true);
            }
            www.Dispose();
        }

        // 写入Manifest
        if (newManifestConfig != null && oldManifestConfig != null)
        {
            foreach (var data in newManifestConfig.data.Values)
            {
                if (oldManifestConfig.Contains(data.name) && oldManifestConfig.Get(data.name).MD5 == data.MD5)
                {
                    continue;
                }
                manifestConfig.Add(data);
            }

            // 写入到文件
            File.WriteAllText(assetPath + "/data/conf/updatefile.json", JsonWriter.Serialize(manifestConfig));
            // 刷新
            AssetDatabase.Refresh();
            // Build清单文件
            AssetBundleBuild[] builds = new AssetBundleBuild[1];
            builds[0].assetBundleName = "data/conf/updatefile";
            builds[0].assetBundleVariant = null;
            builds[0].assetNames = new string[1] { assetPath + "/data/conf/updatefile.json" };
            BuildPipeline.BuildAssetBundles(output, builds, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
        }
    }

    /// <summary>
    /// 拷贝资源包
    /// </summary>
    public static void CopyAssetBundles(string output)
    {
        ManifestConfig manifestConfig = JsonReader.Deserialize<ManifestConfig>(File.ReadAllText(assetPath + "/data/conf/manifestfile.json"));
        // 写入Manifest
        if (manifestConfig != null)
        {
            AssetDatabase.Refresh();
            // 拷贝资源
            if (Directory.Exists(streamingAssets))
            {
                FileUtil.DeleteFileOrDirectory(streamingAssets);
            }
            AssetDatabase.Refresh();
            Directory.CreateDirectory(streamingAssets);
            AssetDatabase.Refresh();

            FileUtil.CopyFileOrDirectory(output + "/data", streamingAssets + "/data");
            string[] filePaths = Directory.GetFiles(streamingAssets + "/data", "*.manifest", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                FileUtil.DeleteFileOrDirectory(filePath);
            }
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// 拷贝更新资源包
    /// </summary>
    public static void CopyUpdateAssetBundles(string output, string dest, string version, string cdn = null)
    {
        ManifestConfig remote = new ManifestConfig();
        if (!string.IsNullOrEmpty(cdn))
        {
            string url = cdn + "/data/conf/updatefile.json";
            WWW www = new WWW(url);
            while (!www.isDone) ;
            if (string.IsNullOrEmpty(www.error) && www.progress == 1f)
            {
                TextAsset text = www.assetBundle.LoadAsset(Path.GetFileNameWithoutExtension(url)) as TextAsset;
                remote = JsonReader.Deserialize<ManifestConfig>(text.text);
                www.assetBundle.Unload(true);
            }
            www.Dispose();
        }

        ManifestConfig local = JsonReader.Deserialize<ManifestConfig>(File.ReadAllText(assetPath + "/data/conf/updatefile.json"));
        if (local != null)
        {
            ManifestConfig manifestConfig = new ManifestConfig();
            foreach (var data in local.data.Values)
            {
                if (remote.Contains(data.name) && remote.Get(data.name).MD5 == data.MD5)
                {
                    continue;
                }
                manifestConfig.Add(data);
            }
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            string updateFilePath = dest + "/updatefile.json";
            string updateFileValue = JsonWriter.Serialize(manifestConfig);
            File.WriteAllText(updateFilePath, updateFileValue);
            AssetDatabase.Refresh();

            manifestConfig.Add(new Manifest() { name = "data/conf/manifestfile.json" });
            manifestConfig.Add(new Manifest() { name = "data/conf/updatefile.json" });

            using (MemoryStream stream = new MemoryStream())
            {
                using (ZipOutputStream zip = new ZipOutputStream(stream))
                {
                    zip.SetComment(version);
                    foreach (var data in manifestConfig.data.Values)
                    {
                        ZipEntry entry = new ZipEntry(data.name);
                        entry.DateTime = new DateTime();
                        entry.DosTime = 0;
                        zip.PutNextEntry(entry);

                        string filepPth = output + "/" + data.name;
                        var bytes = File.ReadAllBytes(filepPth);
                        zip.Write(bytes, 0, bytes.Length);
                    }

                    zip.Finish();
                    zip.Flush();

                    var fileBytes = new byte[stream.Length];
                    Array.Copy(stream.GetBuffer(), fileBytes, fileBytes.Length);

                    string platform = "PC";
#if UNITY_ANDROID
                    platform = "Android";
#elif UNITY_IOS
                    platform = "iOS";
#endif
                    DateTime dt = DateTime.Now;
                    string date = string.Format("{0}.{1}.{2}_{3}.{4}.{5}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                    string md5 = GetMD5(fileBytes);
                    File.WriteAllBytes(string.Format("{0}/{1}_{2}_{3}_{4}.zip", dest, platform, version, date, md5), fileBytes);
                }
            }
            File.Delete(updateFilePath);
            AssetDatabase.Refresh();
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
    /// 得到MD5值
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private static string GetMD5(byte[] bytes)
    {
        string md5Value = string.Empty;
        try
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(bytes);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            md5Value = sb.ToString();
        }
        catch (System.Exception e)
        {
            throw new System.Exception(e.Message);
        }
        return md5Value;
    }

    /// <summary>
    /// 得到清单文件
    /// </summary>
    /// <returns></returns>
    private static ManifestConfig GetManifest(string output)
    {
        output += "/" + output.Split('/')[output.Split('/').Length - 1];
        ManifestConfig manifestConfig = null;
        if (File.Exists(output))
        {
            manifestConfig = new ManifestConfig();
            var bundle = AssetBundle.LoadFromFile(output);
            AssetBundleManifest abManifest = bundle.LoadAsset("assetbundlemanifest") as AssetBundleManifest;
            string[] bundleNames = abManifest.GetAllAssetBundles();
            for (int i = 0; i < bundleNames.Length; ++i)
            {
                if (bundleNames[i].EndsWith("updatefile.json") || bundleNames[i].EndsWith("manifestfile.json"))
                {
                    continue;
                }

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
            bundle.Unload(true);
        }
        return manifestConfig;
    }

    /// <summary>
    /// 设置图集
    /// </summary>
    /// <param name="directoryName"></param>
    /// <param name="relativePath"></param>
    private static void SetSpriteAtlas(string relativePath)
    {
        string spriteAtlasPath = relativePath + ".spriteatlas";
        SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(spriteAtlasPath);
        if (spriteAtlas == null)
        {
            spriteAtlas = new SpriteAtlas();
            AssetDatabase.CreateAsset(spriteAtlas, spriteAtlasPath);

            SpriteAtlasPackingSettings spriteAtlasPackingSettings = SpriteAtlasExtensions.GetPackingSettings(spriteAtlas);
            spriteAtlasPackingSettings.enableTightPacking = false;
            spriteAtlasPackingSettings.padding = 2;
            SpriteAtlasTextureSettings spriteAtlasTextureSettings = SpriteAtlasExtensions.GetTextureSettings(spriteAtlas);
            spriteAtlasTextureSettings.sRGB = true;
            
            var obj = AssetDatabase.LoadMainAssetAtPath(relativePath);
            UnityEngine.Object[] objects = new UnityEngine.Object[] { obj };
            SpriteAtlasExtensions.Add(spriteAtlas, objects);
        }
    }
}