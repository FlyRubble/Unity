using UnityEngine;

namespace Framework
{
    using UI;
    using IO;
    using JsonFx;
    /// <summary>
    /// 状态
    /// </summary>
    public class Unzip : State
    {
        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="param"></param>
        public override void OnEnter(Param param = null)
        {
            base.OnEnter(param);

            WWW www = new WWW(App.localUrl + "data/conf/manifestfile.json");
            while (!www.isDone) ;

            UIManager.instance.OpenUI("uisplash");
            if (string.IsNullOrEmpty(www.error))
            {
                Debug.Log("不需要解压");
                ManifestConfig manifest = JsonReader.Deserialize<ManifestConfig>(www.assetBundle.LoadAsset<TextAsset>("manifestfile").text);
                Debug.Log(manifest.data.Count);
            }
            else
            {
                Debug.Log("需要解压");
            }
        }
        #endregion
    }
}