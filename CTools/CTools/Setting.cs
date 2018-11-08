using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

public sealed class Setting
{
    #region
    private static string m_path = "Setting.json";

    /// <summary>
    /// 数据
    /// </summary>
    private static Dictionary<string, string> m_data = new Dictionary<string, string>();
    #endregion

    #region Property
    #endregion

    #region Function
    /// <summary>
    /// 是否包含
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool ContainsKey(Option key)
    {
        return m_data.ContainsKey(key.ToString());
    }

    /// <summary>
    /// 得到字符串值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetString(Option key)
    {
        string value = string.Empty;
        if (!string.IsNullOrEmpty(key.ToString()) && m_data.ContainsKey(key.ToString()))
        {
            value = m_data[key.ToString()];
        }
        return value;
    }

    /// <summary>
    /// 得到整型值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static int GetInt(Option key)
    {
        int value = 0;
        if (!string.IsNullOrEmpty(key.ToString()) && m_data.ContainsKey(key.ToString()))
        {
            int.TryParse(m_data[key.ToString()], out value);
        }
        return value;
    }

    /// <summary>
    /// 设置字符串值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetString(Option key, string value)
    {
        if (!string.IsNullOrEmpty(key.ToString()) && !string.IsNullOrEmpty(value))
        {
            if (m_data.ContainsKey(key.ToString()))
            {
                m_data[key.ToString()] = value;
            }
            else
            {
                m_data.Add(key.ToString(), value);
            }
        }
    }

    /// <summary>
    /// 设置整型值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetInt(Option key, int value)
    {
        SetString(key, value.ToString());
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        m_path = Path.Combine(Directory.GetCurrentDirectory(), m_path);
        if (File.Exists(m_path))
        {
            using (StreamReader streamReader = new StreamReader(m_path))
            {
                Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(streamReader.ReadToEnd());
                if (dic != null)
                {
                    m_data = dic;
                }
                streamReader.Close();
            }
        }

        if (!ContainsKey(Option.Excel))
        {
            m_data.Add(Option.Excel.ToString(), string.Empty);
        }

        if (!ContainsKey(Option.Json))
        {
            m_data.Add(Option.Json.ToString(), string.Empty);
        }

        if (!ContainsKey(Option.TypeIndex))
        {
            SetInt(Option.TypeIndex, 0);
        }

        if (!ContainsKey(Option.TypeFormat))
        {
            SetString(Option.TypeFormat, ":");
        }

        if (!ContainsKey(Option.TypeFormatIndex))
        {
            SetInt(Option.TypeFormatIndex, 0);
        }

        if (!ContainsKey(Option.NameIndex))
        {
            SetInt(Option.NameIndex, 1);
        }

        if (!ContainsKey(Option.NameFormat))
        {
            SetString(Option.NameFormat, ":");
        }

        if (!ContainsKey(Option.NameFormatIndex))
        {
            SetInt(Option.NameFormatIndex, 0);
        }

        if (!ContainsKey(Option.AttrIndex))
        {
            SetInt(Option.AttrIndex, 1);
        }

        if (!ContainsKey(Option.AttrFormat))
        {
            SetString(Option.AttrFormat, ":");
        }

        if (!ContainsKey(Option.AttrFormatIndex))
        {
            SetInt(Option.AttrFormatIndex, 1);
        }
        
        if (!ContainsKey(Option.PointIndex))
        {
            SetInt(Option.PointIndex, 2);
        }

        if (!ContainsKey(Option.LangTag))
        {
            SetString(Option.LangTag, "lang");
        }

        if (!ContainsKey(Option.LangUUID))
        {
            SetString(Option.LangUUID, "uuid");
        }

        if (!ContainsKey(Option.Lang))
        {
            SetString(Option.Lang, "cn");
        }

        if (!ContainsKey(Option.LangFile))
        {
            SetString(Option.LangFile, "lang.xls");
        }

        Save();
    }

    public static void Save()
    {
        Encoding encoding = new UTF8Encoding(true);
        string json = JsonConvert.SerializeObject(m_data, Formatting.Indented);
        using (StreamWriter streamReader = new StreamWriter(m_path, false, encoding))
        {
            streamReader.Write(json);
            streamReader.Flush();
            streamReader.Close();
        }
    }
    #endregion
}