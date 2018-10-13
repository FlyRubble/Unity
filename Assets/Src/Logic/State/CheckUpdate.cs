using UnityEngine;
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
    public class CheckUpdate : State
    {
        #region Variable
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

            AssetManager.instance.url = App.persistentDataPath;
            // 获取清单文件
            WWW www = new WWW(App.persistentDataPath + Const.MANIFESTFILE);
            while (!www.isDone) ;
            App.manifest = JsonReader.Deserialize<ManifestConfig>(www.assetBundle.LoadAsset<TextAsset>(Path.GetFileNameWithoutExtension(www.url)).text);
            if (www.assetBundle != null)
            {
                www.assetBundle.Unload(true);
            }

            // 打开Loading界面
            AssetManager.instance.UnloadAssets();
            UIManager.instance.Clear();
            UIManager.instance.OpenUI(Const.UI_LOADING);

            // 远程Version文件
            www = new WWW(App.cdn + App.platform + Const.MANIFESTFILE);
            while (!www.isDone) ;
            App.manifest = JsonReader.Deserialize<ManifestConfig>(www.assetBundle.LoadAsset<TextAsset>(Path.GetFileNameWithoutExtension(www.url)).text);
            if (www.assetBundle != null)
            {
                www.assetBundle.Unload(true);
            }
            //Dictionary<string, object> data = JsonReader.Deserialize<Dictionary<string, object>>(t.text);
            // 解压
            //m_async.Clear();
            //AsyncAsset async = AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + Const.UPDATE_FILE, (bResult, asset) => {
            //    Helper.WriteAllBytes(App.assetPath + Const.UPDATE_FILE, asset.bytes);
            //    AssetManager.instance.UnloadAssets(asset);
            //});
            //m_size += 0.1F;
            //m_async.Add(0.1F, async);
            //foreach (var data in App.manifest.data.Values)
            //{
            //    m_size += data.size / 1024F;
            //    async = AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + data.name, (bResult, asset) => {
            //        Helper.WriteAllBytes(App.assetPath + data.name, asset.bytes);
            //        AssetManager.instance.UnloadAssets(asset);
            //    });
            //    m_async.Add(data.size / 1024F, async);
            //}
            //async = AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + Const.MANIFESTFILE, (bResult, asset) => {
            //    Helper.WriteAllBytes(App.assetPath + Const.MANIFESTFILE, asset.bytes);
            //    AssetManager.instance.UnloadAssets(asset);
            //});
            //m_size += 0.5F;
            //m_async.Add(0.5F, async);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public override void Update()
        {
            base.Update();

            //if (Time.realtimeSinceStartup >= m_time + 1F)
            //{
            //    m_time = Time.realtimeSinceStartup;

            //    float speed = m_currentSize;
            //    m_currentSize = 0;
            //    foreach (var data in m_async)
            //    {
            //        m_currentSize += data.Key * data.Value.progress;
            //    }
            //    speed = m_currentSize - speed;
                
            //    Debug.LogErrorFormat("解压进度{0:F2}/{1:F2}MB({2:F2}%),解压速度{3:F2}MB/s", m_currentSize, m_size, 100F * m_currentSize / m_size, speed);
            //}
        }
        #endregion
    }
}