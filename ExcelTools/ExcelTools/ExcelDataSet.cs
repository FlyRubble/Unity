using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExcelDataSet
{
    #region Variable
    /// <summary>
    /// 单例对象
    /// </summary>
    private static ExcelDataSet m_instance = null;

    /// <summary>
    /// 数据
    /// </summary>
    private Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> m_data = null;
    #endregion

    #region Property
    /// <summary>
    /// 单例
    /// </summary>
    public static ExcelDataSet instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new ExcelDataSet();
            }
            return m_instance;
        }
    }

    /// <summary>
    /// 数据
    /// </summary>
    public Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> data
    {
        get
        {
            return m_data;
        }
    }
    #endregion

    #region Function
    /// <summary>
    /// 构造
    /// </summary>
    public ExcelDataSet()
    {
        m_data = new Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool TryAdd(string name, Dictionary<string, List<Dictionary<string, object>>> data)
    {
        bool bResult = false;
        if (!m_data.ContainsKey(name))
        {
            m_data.Add(name, data);
            bResult = true;
        }
        return bResult;
    }

    /// <summary>
    /// 得到
    /// </summary>
    /// <param name="name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool TryGet(string name, out Dictionary<string, List<Dictionary<string, object>>> data)
    {
        return m_data.TryGetValue(name, out data);
    }

    /// <summary>
    /// 清理数据
    /// </summary>
    public void Clear()
    {
        m_data.Clear();
    }
    #endregion
}
