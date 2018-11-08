using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

    /// <summary>
    /// lang
    /// </summary>
    private Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> m_lang = null;
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
        m_lang = new Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>>();
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
        m_lang.Clear();
    }

    /// <summary>
    /// 得到语言
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Error TryGetLang(string fileName, string sheetName, string key, out string value)
    {
        Dictionary<string, List<Dictionary<string, object>>> sheet = null;
        value = string.Empty;
        Error error = Error.Lang;
        fileName = Setting.GetString(Option.LangTag) + "_" + Util.GetExcelFileName(fileName);

        string path = Path.Combine(Setting.GetString(Option.Excel), Setting.GetString(Option.LangFile));
        if (Directory.Exists(path))
        {
            if (!m_lang.ContainsKey(fileName))
            {
                string[] file = Directory.GetFiles(path, "*.xls*", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < file.Length; ++i)
                {
                    if (Path.GetFileNameWithoutExtension(file[i]).Equals(fileName))
                    {
                        var data = new ExcelReader().Open(file[i], null);
                        m_lang.Add(fileName, data);
                        break;
                    }
                }

            }
            if (m_lang.ContainsKey(fileName))
            {
                sheet = m_lang[fileName];
            }
        }
        else if (File.Exists(path))
        {
            foreach (var lang in m_lang)
            {
                sheet = lang.Value;
                break;
            }
            if (sheet == null)
            {
                sheet = new ExcelReader().Open(path, null);
                m_lang.Add(fileName, sheet);
            }
        }

        if (sheet != null)
        {
            if (sheet.ContainsKey(sheetName))
            {
                foreach (var kvp in sheet[sheetName])
                {
                    if (kvp.ContainsKey(Setting.GetString(Option.LangUUID)) && kvp[Setting.GetString(Option.LangUUID)].Equals(key) && kvp.ContainsKey(Setting.GetString(Option.Lang)))
                    {
                        error = Error.None;
                        value = kvp[Setting.GetString(Option.Lang)].ToString();
                        break;
                    }
                }
            }
            else
            {
                foreach (var tabel in sheet.Values)
                {
                    foreach (var kvp in tabel)
                    {
                        if (kvp.ContainsKey(Setting.GetString(Option.LangUUID)) && kvp[Setting.GetString(Option.LangUUID)].Equals(key) && kvp.ContainsKey(Setting.GetString(Option.Lang)))
                        {
                            error = Error.None;
                            value = kvp[Setting.GetString(Option.Lang)].ToString();
                            break;
                        }
                    }
                    if (error == Error.None)
                    {
                        break;
                    }
                }
            }
        }
        return error;
    }
    #endregion
}
