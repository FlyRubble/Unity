﻿using UnityEngine;
using System.IO;
using UnityAsset;
using System.Collections.Generic;

namespace Framework
{
    using UI;
    using IO;
    using JsonFx;
    /// <summary>
    /// 状态
    /// </summary>
    public class AssetDecompressing : State
    {
        #region Variable
        /// <summary>
        /// 沙盒资源是否需要解压
        /// </summary>
        private bool m_assetDecompressing = false;

        /// <summary>
        /// 时间，用于计算解压速度
        /// </summary>
        private float m_time = 0;

        /// <summary>
        /// 要解压的资源大小
        /// </summary>
        private float m_size = 0;

        /// <summary>
        /// 当前已解压大小
        /// </summary>
        private float m_currentSize = 0;

        /// <summary>
        /// 解压资源记录
        /// </summary>
        private Dictionary<float, AsyncAsset> m_async = new Dictionary<float, AsyncAsset>();
        #endregion

        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);

            // 是否需要更新沙盒资源
            m_assetDecompressing = !App.version.Equals(PlayerPrefs.GetString(Const.SANDBOX_VERSION));
            if (m_assetDecompressing)
            {
                // 清空文件夹，以便重新解压
                if (Directory.Exists(Application.persistentDataPath))
                {
                    Directory.Delete(Application.persistentDataPath, true);
                }
                // 获取清单文件
                WWW www = new WWW(App.streamingAssetsPath + Const.MANIFESTFILE);
                while (!www.isDone) ;
                App.manifest = JsonReader.Deserialize<ManifestConfig>(www.assetBundle.LoadAsset<TextAsset>(Path.GetFileNameWithoutExtension(www.url)).text);
                if (www.assetBundle != null)
                {
                    www.assetBundle.Unload(true);
                }
                // 解压准备
                m_async.Clear();
                AsyncAsset async = AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + Const.UPDATE_FILE, (bResult, asset) =>
                {
                    Helper.WriteAllBytes(App.assetPath + Const.UPDATE_FILE, asset.bytes);
                });
                m_size += 0.1F;
                m_async.Add(0.1F, async);
                foreach (var data in App.manifest.data.Values)
                {
                    m_size += data.size / 1024F;
                    async = AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + data.name, (bResult, asset) =>
                    {
                        Helper.WriteAllBytes(App.assetPath + data.name, asset.bytes);
                    });
                    m_async.Add(data.size / 1024F, async);
                }
                async = AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + Const.MANIFESTFILE, (bResult, asset) =>
                {
                    Helper.WriteAllBytes(App.assetPath + Const.MANIFESTFILE, asset.bytes);
                });
                m_size += 0.5F;
                m_async.Add(0.5F, async);
            }
            else
            {
                Debug.LogError("沙盒资源更新");
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (m_assetDecompressing)
            {
                if (Time.realtimeSinceStartup >= m_time + 1F)
                {
                    m_time = Time.realtimeSinceStartup;

                    float speed = m_currentSize;
                    m_currentSize = 0;
                    foreach (var data in m_async)
                    {
                        m_currentSize += data.Key * data.Value.progress;
                    }
                    speed = m_currentSize - speed;

                    Debug.LogErrorFormat("解压进度{0:F2}/{1:F2}MB({2:F2}%),解压速度{3:F2}MB/s", m_currentSize, m_size, 100F * m_currentSize / m_size, speed);
                    if (m_currentSize == m_size)
                    {
                        AssetManager.instance.UnloadAssets();
                        PlayerPrefs.SetString(Const.SANDBOX_VERSION, App.version);
                        //StateMachine.instance.OnEnter(new CheckUpdate());
                    }
                }
            }
        }
        #endregion
    }
}