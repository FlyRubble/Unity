using System;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Excel;

public class ExcelReader
{
    #region
    /// <summary>
    /// 当前文件数据
    /// </summary>
    private Dictionary<string, List<Dictionary<string, object>>> m_data = new Dictionary<string, List<Dictionary<string, object>>>();
    #endregion

    #region Property
    /// <summary>
    /// 当前文件数据
    /// </summary>
    public Dictionary<string, List<Dictionary<string, object>>> data
    {
        get
        {
            return m_data;
        }
    }
    #endregion

    #region Function
    /// <summary>
    /// 打开文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fail"></param>
    /// <returns></returns>
    public Dictionary<string, List<Dictionary<string, object>>> Open(string path, Action fail)
    {
        m_data.Clear();
        DataSet book = null;
        try
        {
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                book = excelReader.AsDataSet();
                fileStream.Close();
            }
        }
        catch (Exception e)
        {
            Util.ShowMessageBox(e.Message, fail);
        }
        finally
        {
            if (book != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(path);
                foreach (DataTable sheet in book.Tables)
                {
                    if (IsEnglish(sheet.TableName) && !sheet.TableName.StartsWith("Sheet"))
                    {
                        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                        m_data.Add(sheet.TableName, list);

                        if (sheet.Rows.Count >= Setting.GetInt(Option.PointIndex))
                        {
                            DataRow fieldType = sheet.Rows[Setting.GetInt(Option.TypeIndex)];
                            DataRow fieldName = sheet.Rows[Setting.GetInt(Option.NameIndex)];
                            DataRow fieldAttr = sheet.Rows[Setting.GetInt(Option.AttrIndex)];
                            for (int i = Setting.GetInt(Option.PointIndex); i < sheet.Rows.Count; i++)
                            {
                                DataRow row = sheet.Rows[i];
                                if (!string.IsNullOrEmpty(row[0].ToString()))
                                {
                                    Dictionary<string, object> dic = new Dictionary<string, object>();
                                    list.Add(dic);

                                    foreach (DataColumn column in sheet.Columns)
                                    {
                                        string name = GetName(fieldName[column].ToString());
                                        string attr = GetAttr(fieldAttr[column].ToString());
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            string type = GetType(fieldType[column].ToString());
                                            object value = null;
                                            Error error = TryParse(fileName, sheet.TableName, row[column], type, out value);
                                            if (error == Error.Type)
                                            {
                                                Util.ShowMessageBox(string.Format("{0}\n{1}\n字段['{2}'],索引行['{3}']内容与类型不匹配", path, sheet.TableName, name, i), fail);
                                                goto End;
                                            }
                                            else if (error == Error.Lang)
                                            {
                                                Util.ShowMessageBox(string.Format("{0}\n{1}\n字段['{2}'],索引行['{3}']多语言找寻不到", path, sheet.TableName, name, i), fail);
                                                goto End;
                                            }
                                            if (dic.ContainsKey(name))
                                            {
                                                Util.ShowMessageBox(string.Format("{0}\n{1}\n字段['{2}']重复", path, sheet.TableName, name), fail);
                                                goto End;
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(attr))
                                                {
                                                    switch (attr)
                                                    {
                                                    case "guid":
                                                    case "GUID":
                                                    {
                                                        foreach (var lt in list)
                                                        {
                                                            if (lt.ContainsKey(name) && lt[name].Equals(value))
                                                            {
                                                                Util.ShowMessageBox(string.Format("{0}\n{1}\n{2}\n存在相同主键值 {3}", path, sheet.TableName, name, value), fail);
                                                                goto End;
                                                            }
                                                        }
                                                    }
                                                    break;
                                                    case "none":
                                                    case "NONE":
                                                    { }
                                                    continue;
                                                    }
                                                }
                                                dic.Add(name, value);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            End: { }
        }
        return m_data;
    }

    /// <summary>
    /// 是否是英文
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsEnglish(string text)
    {
        Regex regex = new Regex("^[a-z_A-Z]+$");
        return regex.IsMatch(text);
    }

    /// <summary>
    /// 得到名字
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string GetName(string value)
    {
        string[] array = value.Split(Setting.GetString(Option.NameFormat).ToCharArray()[0]);
        int index = Setting.GetInt(Option.NameFormatIndex);
        return index < array.Length ? array[index] : string.Empty;
    }

    /// <summary>
    /// 得到类型
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string GetType(string value)
    {
        string[] array = value.Split(Setting.GetString(Option.TypeFormat).ToCharArray()[0]);
        int index = Setting.GetInt(Option.TypeFormatIndex);
        return index < array.Length ? array[index] : string.Empty;
    }

    /// <summary>
    /// 得到属性
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string GetAttr(string value)
    {
        string[] array = value.Split(Setting.GetString(Option.AttrFormat).ToCharArray()[0]);
        int index = Setting.GetInt(Option.AttrFormatIndex);
        return index < array.Length ? array[index] : string.Empty;
    }

    /// <summary>
    /// 得到值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static Error TryParse(string fileName, string sheetName, object value, string type, out object result)
    {
        Error error = Error.None;
        result = value;
        try
        {
            switch (type)
            {
            case "bool":
            {
                bool bResult = false;
                if (bool.TryParse(value.ToString(), out bResult))
                {
                    result = bResult;
                }
                else
                {
                    result = int.Parse(value.ToString()) != 0;
                }
            }
            break;
            case "int":
            {
                result = int.Parse(value.ToString());
            }
            break;
            case "float":
            {
                result = float.Parse(value.ToString());
            }
            break;
            case "double":
            {
                result = double.Parse(value.ToString());
            }
            break;
            default:
            {
                if (type.Equals(Setting.GetString(Option.LangTag)))
                {
                    string outValue = string.Empty;
                    error = ExcelDataSet.instance.TryGetLang(fileName, sheetName, value.ToString(), out outValue);
                    result = outValue;
                }
                else
                {
                    result = value.ToString();
                }
            }
            break;
            }
        }
        catch (Exception e)
        {
            error = Error.Type;
        }
        return error;
    }
    #endregion
}
