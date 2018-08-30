using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置文件
/// </summary>
public class Settings
{
    #region Variable
    /// <summary>
    /// 配置组的选择
    /// </summary>
    private int m_selected = 0;

    /// <summary>
    /// 启动配置表
    /// </summary>
    private List<LaunchConfig> m_launchConfig = new List<LaunchConfig>();
    #endregion

    #region Property
    /// <summary>
    /// 配置组选择
    /// </summary>
    /// <value>The selected.</value>
    public int selected
    {
        get { return m_selected; }
        set { m_selected = value; }
    }

    /// <summary>
    /// 启动配置表
    /// </summary>
    public List<LaunchConfig> launchConfig
    {
        get { return m_launchConfig; }
        set { m_launchConfig = value; }
    }
    #endregion
}
