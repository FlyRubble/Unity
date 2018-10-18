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
    /// 确认文本
    /// </summary>
    public const string TEXT_SURE = "text_sure";

    /// <summary>
    /// 确认事件
    /// </summary>
    public const string ACTION_SURE = "action_sure";

    /// <summary>
    /// 取消文本
    /// </summary>
    public const string TEXT_CANCEL = "text_cancel";

    /// <summary>
    /// 取消事件
    /// </summary>
    public const string ACTION_CANCEL = "action_cancel";

    /// <summary>
    /// 关闭按钮状态
    /// </summary>
    public const string ACTIVE_CLOSE = "active_close";

    /// <summary>
    /// 关闭事件
    /// </summary>
    public const string ACTION_CLOSE = "action_close";
    #endregion

    /// <summary>
    /// 开始
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 通知
    /// </summary>
    /// <param name="param"></param>
    public override void OnNotification(Param param = null)
    {
        base.OnNotification(param);

        if (m_param.Contain(OPEN))
        {
            // 文本内容
            if (m_param.Contain(TEXT_CONTENT))
            {
                Util.SetText(transform, "obj_bg/txt", m_param.GetString(TEXT_CONTENT));
            }
            // 确认
            Util.SetActive(transform, "obj_bg/obj_btn/btn_sure", m_param.Contain(ACTION_SURE));
            if (m_param.Contain(ACTION_SURE))
            {
                Util.SetText(transform, "obj_bg/obj_btn/btn_sure/txt_sure", m_param.Contain(TEXT_SURE) ? m_param.GetString(TEXT_SURE) : Util.GetLanguage("", "确认"));
                Util.SetButton(transform, "obj_bg/obj_btn/btn_sure", m_param.GetAction(ACTION_SURE));
            }
            // 取消
            Util.SetActive(transform, "obj_bg/obj_btn/btn_cancel", m_param.Contain(ACTION_CANCEL));
            if (m_param.Contain(ACTION_CANCEL))
            {
                Util.SetText(transform, "obj_bg/obj_btn/btn_cancel/txt_cancel", m_param.Contain(TEXT_CANCEL) ? m_param.GetString(TEXT_CANCEL) : Util.GetLanguage("", "取消"));
                Util.SetButton(transform, "obj_bg/obj_btn/btn_cancel", m_param.GetAction(ACTION_CANCEL));
            }
            // 关闭
            Util.SetActive(transform, "obj_bg/btn_close", m_param.Contain(ACTIVE_CLOSE) ? m_param.GetBool(ACTIVE_CLOSE) : true);
            Util.SetButton(transform, "obj_bg/btn_close", m_param.Contain(ACTION_CLOSE) ? m_param.GetAction(ACTION_CLOSE) : () => {
                UIManager.instance.CloseUI(Const.UI_NORMAL_TIPS_BOX);
            });
        }
    }
}
