using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

public sealed class Setting
{
    #region
    /// <summary>
    /// 类型索引位置
    /// </summary>
    private static int m_typeIndex = 0;

    /// <summary>
    /// 类型格式
    /// </summary>
    private static object[] m_typeFormat = { ":", 0 };

    /// <summary>
    /// 字段名字索引位置
    /// </summary>
    private static int m_nameIndex = 1;

    /// <summary>
    /// 字段格式
    /// </summary>
    private static object[] m_nameFormat = { ":" , 0 };

    /// <summary>
    /// 属性索引位置
    /// </summary>
    private static int m_attrIndex = 1;

    /// <summary>
    /// 属性格式
    /// </summary>
    private static object[] m_attrFormat = { ":", 1 };

    /// <summary>
    /// 正文开始索引位置
    /// </summary>
    private static int m_startIndex = 3;

    /// <summary>
    /// 语言连接格式
    /// </summary>
    private static string[] m_langFormat = { "uuid", "cn"};

    /// <summary>
    /// 需要连接的字段类型标志
    /// </summary>
    private static string m_langTag = "lang";

    /// <summary>
    /// 语言连接文件的文件名(带扩展名)
    /// </summary>
    private static string m_langFileName = "lang.xls";
    #endregion

    #region Property
    /// <summary>
    /// 类型索引位置
    /// </summary>
    public static int typeIndex
    {
        get
        {
            return m_typeIndex;
        }
    }

    /// <summary>
    /// 类型格式
    /// </summary>
    public static object[] typeFormat
    {
        get
        {
            return m_typeFormat;
        }
    }
    /// <summary>
    /// 字段名字索引位置
    /// </summary>
    public static int nameIndex
    {
        get
        {
            return m_nameIndex;
        }
    }

    /// <summary>
    /// 字段格式
    /// </summary>
    public static object[] nameFormat
    {
        get
        {
            return m_nameFormat;
        }
    }

    /// <summary>
    /// 属性索引位置
    /// </summary>
    public static int attrIndex
    {
        get
        {
            return m_attrIndex;
        }
    }

    /// <summary>
    /// 属性格式
    /// </summary>
    public static object[] attrFormat
    {
        get
        {
            return m_attrFormat;
        }
    }

    /// <summary>
    /// 正文开始索引位置
    /// </summary>
    public static int startIndex
    {
        get
        {
            return m_startIndex;
        }
    }

    /// <summary>
    /// 语言连接格式
    /// </summary>
    public static string[] langFormat
    {
        get
        {
            return m_langFormat;
        }
    }

    /// <summary>
    /// 需要连接的字段类型标志
    /// </summary>
    public static string langTag
    {
        get
        {
            return m_langTag;
        }
    }

    /// <summary>
    /// 语言连接文件的文件名(带扩展名)
    /// </summary>
    public static string langFileName
    {
        get
        {
            return m_langFileName;
        }
    }
    #endregion

    #region Function
    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Setting.json");
        if (File.Exists(path))
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(streamReader.ReadToEnd());
                if (dic != null)
                {
                    try
                    {
                        object value = null;
                        if (dic.TryGetValue("typeIndex", out value))
                        {
                            m_typeIndex = int.Parse(value.ToString());
                        }

                        if (dic.TryGetValue("typeFormat", out value))
                        {
                            object[] obj = (object[])value;
                            if (obj.Length >= m_typeFormat.Length)
                            {
                                m_typeFormat = obj;
                            }
                        }

                        if (dic.TryGetValue("nameIndex", out value))
                        {
                            m_nameIndex = int.Parse(value.ToString());
                        }

                        if (dic.TryGetValue("nameFormat", out value))
                        {
                            object[] obj = (object[])value;
                            if (obj.Length >= m_nameFormat.Length)
                            {
                                m_nameFormat = obj;
                            }
                        }

                        if (dic.TryGetValue("attrIndex", out value))
                        {
                            m_attrIndex = int.Parse(value.ToString());
                        }

                        if (dic.TryGetValue("attrFormat", out value))
                        {
                            object[] obj = (object[])value;
                            if (obj.Length >= m_attrFormat.Length)
                            {
                                m_attrFormat = obj;
                            }
                        }
                        
                        if (dic.TryGetValue("startIndex", out value))
                        {
                            m_startIndex = int.Parse(value.ToString());
                        }

                        if (dic.TryGetValue("langFormat", out value))
                        {
                            string[] obj = (string[])value;
                            if (obj.Length >= m_langFormat.Length)
                            {
                                m_langFormat = obj;
                            }
                        }

                        if (dic.TryGetValue("langTag", out value))
                        {
                            m_langTag = value.ToString();
                        }

                        if (dic.TryGetValue("langFileName", out value))
                        {
                            m_langFileName = value.ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        Util.ShowMessageBox(string.Format("{0}\n配置格式错误", path), () => { Environment.Exit(0); });
                    }
                }
                streamReader.Close();
            }
        }
        else
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("typeIndex", typeIndex);
            dic.Add("typeFormat", typeFormat);
            dic.Add("nameIndex", nameIndex);
            dic.Add("nameFormat", nameFormat);
            dic.Add("attrIndex", attrIndex);
            dic.Add("attrFormat", attrFormat);
            dic.Add("startIndex", startIndex);
            dic.Add("langFormat", langFormat);
            dic.Add("langTag", langTag);
            dic.Add("langFileName", langFileName);

            Encoding encoding = new UTF8Encoding(true);
            string json = JsonConvert.SerializeObject(dic, Formatting.Indented);
            using (StreamWriter streamReader = new StreamWriter(path, false, encoding))
            {
                streamReader.Write(json);
                streamReader.Flush();
                streamReader.Close();
            }
        }
    }
    #endregion
}