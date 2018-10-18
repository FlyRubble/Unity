using UnityEngine.UI;
using Framework.UI;
using Framework.Event;
using Framework;

public class UILoading : UIBase
{
    #region Const
    /// <summary>
    /// 提示内容
    /// </summary>
    public const string TEXT_TIPS = "text_tips";

    /// <summary>
    /// 进度值
    /// </summary>
    public const string SLIDER = "slider";

    /// <summary>
    /// 描述
    /// </summary>
    public const string TEXT_DETAILS = "text_details";
    #endregion

    #region Variable
    /// <summary>
    /// tips
    /// </summary>
    Text m_tips = null;

    /// <summary>
    /// 进度值
    /// </summary>
    Slider m_slider = null;

    /// <summary>
    /// 描述
    /// </summary>
    Text m_details = null;
    #endregion

    #region Function
    /// <summary>
    /// 启动
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        m_tips = Util.Find<Text>(transform, "obj/txt_tips");
        m_slider = Util.Find<Slider>(transform, "obj/slider");
        m_details = Util.Find<Text>(transform, "obj/txt_details");
    }

    /// <summary>
    /// 事件通知
    /// </summary>
    /// <param name="param"></param>
    public override void OnNotification(Param param = null)
    {
        base.OnNotification(param);

        if (m_param.Contain(OPEN) || m_param.Contain(UPDATE))
        {
            if (m_param.Contain(TEXT_TIPS))
            {
                m_tips.text = m_param.GetString(TEXT_TIPS);
            }
            if (m_param.Contain(SLIDER))
            {
                m_slider.value = m_param.GetFloat(SLIDER);
            }
            if (m_param.Contain(TEXT_DETAILS))
            {
                m_details.text = m_param.GetString(TEXT_DETAILS);
            }
        }
    }
    #endregion
}
