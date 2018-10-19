using UnityEngine;
using System.IO;
using UnityAsset;
using System.Collections.Generic;

namespace Framework
{
    using UI;
    using Event;
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
        private List<AsyncAsset> m_async = new List<AsyncAsset>();
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
                // 解压准备
                m_async.Clear();
                AsyncAsset async = AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + Const.UPDATE_FILE, (bResult, asset) =>
                {
                    if (bResult)
                    {
                        Util.WriteAllBytes(App.assetPath + Const.UPDATE_FILE, asset.bytes);
                    }
                    else
                    {
                        Debugger.Log(asset.error);
                    }
                });
                async.args = 0.1F;
                m_size += (float)async.args;
                m_async.Add(async);
                foreach (var data in App.manifest.data.Values)
                {
                    async = AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + data.name, (bResult, asset) =>
                    {
                        if (bResult)
                        {
                            Util.WriteAllBytes(App.assetPath + data.name, asset.bytes);
                        }
                        else
                        {
                            Debugger.Log(asset.error);
                        }
                    });
                    async.args = data.size / 1024F;
                    m_size += (float)async.args;
                    m_async.Add(async);
                }
                async = AssetManager.instance.AssetBundleLoadAsync(AssetManager.instance.url + Const.MANIFESTFILE, (bResult, asset) =>
                {
                    if (bResult)
                    {
                        Util.WriteAllBytes(App.assetPath + Const.MANIFESTFILE, asset.bytes);
                    }
                    else
                    {
                        Debugger.LogError(asset.error);
                    }
                });
                async.args = 0.5F;
                m_size += (float)async.args;
                m_async.Add(async);
            }
            else
            {
                StateMachine.instance.OnEnter(new AssetUpdate());
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
                float speed = m_currentSize;
                if (Time.realtimeSinceStartup >= m_time + 1F)
                {
                    m_time = Time.realtimeSinceStartup;

                    m_currentSize = 0;
                    foreach (var data in m_async)
                    {
                        m_currentSize += (float)data.args * data.progress;
                    }
                    speed = m_currentSize - speed;
                }
                UIManager.instance.OpenUI(Const.UI_LOADING, Param.Create(new object[] { UILoading.SLIDER, m_currentSize / m_size }));
                if (m_currentSize == m_size)
                {
                    m_assetDecompressing = false;
                    //PlayerPrefs.SetString(Const.SANDBOX_VERSION, App.version);
                    Schedule.instance.ScheduleOnce(0.3F, () => { StateMachine.instance.OnEnter(new AssetUpdate()); });
                }
            }
        }
        #endregion
    }
}