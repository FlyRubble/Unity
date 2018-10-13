using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using Framework.Event;
using Framework;

public class UINormalTipsBox : UIBase
{
    #region Const
    /// <summary>
    /// 文本内容
    /// </summary>
    public const string TEXT_CONTENT = "text_content";

    /// <summary>
    /// 确认事件
    /// </summary>
    public const string SURE_ACTION = "sure_action";
    #endregion
    protected override void Start()
    {
        base.Start();
    }

    public override void OnNotification(Param param = null)
    {
        base.OnNotification(param);

        transform.Find("_txt").GetComponent<Text>().text = m_param[TEXT_CONTENT].ToString();
        Button btn = transform.Find("_btn_sure").GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            Action action = (Action)m_param[SURE_ACTION];
            action();
        });
    }
}
