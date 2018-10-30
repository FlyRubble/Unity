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
            ShowMessageBox(e.Message, fail);
        }
        finally
        {
            if (book != null)
            {
                foreach (DataTable sheet in book.Tables)
                {
                    if (IsEnglish(sheet.TableName))
                    {
                        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                        m_data.Add(sheet.TableName, list);

                        if (sheet.Rows.Count >= Setting.startIndex)
                        {
                            DataRow fieldType = sheet.Rows[Setting.typeIndex];
                            DataRow fieldName = sheet.Rows[Setting.nameIndex];
                            DataRow fieldAttr = sheet.Rows[Setting.attrIndex];
                            for (int i = Setting.startIndex; i < sheet.Rows.Count; i++)
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
                                            if (!TryParse(row[column], type, out value))
                                            {
                                                ShowMessageBox(string.Format("{0}\n{1}\n字段['{2}'],索引行['{3}']内容与类型不匹配", path, sheet.TableName, name, i), fail);
                                                goto End;
                                            }
                                            if (dic.ContainsKey(name))
                                            {
                                                ShowMessageBox(string.Format("{0}\n{1}\n字段['{2}']重复", path, sheet.TableName, name), fail);
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
                                                                ShowMessageBox(string.Format("{0}\n{1}\n{2}\n存在相同主键值 {3}", path, sheet.TableName, name, value), fail);
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
    /// 显示消息框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="action"></param>
    /// <param name="title"></param>
    public static void ShowMessageBox(string text, Action action, string title = "Error")
    {
        DialogResult dialogResult = MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        if (dialogResult == DialogResult.OK)
        {
            action?.Invoke();
        }
    }

    /// <summary>
    /// 是否是英文
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsEnglish(string text)
    {
        Regex regex = new Regex("^[a-zA-Z]+$");
        return regex.IsMatch(text);
    }

    /// <summary>
    /// 得到名字
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string GetName(string value)
    {
        string[] sValue = { Setting.nameFormat[0].ToString(), Setting.nameFormat[1].ToString() };
        string[] array = value.Split(sValue[0].ToCharArray()[0]);
        int index = int.Parse(sValue[1]);
        return index < array.Length ? array[index] : string.Empty;
    }

    /// <summary>
    /// 得到类型
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string GetType(string value)
    {
        string[] sValue = { Setting.typeFormat[0].ToString(), Setting.typeFormat[1].ToString() };
        string[] array = value.Split(sValue[0].ToCharArray()[0]);
        int index = int.Parse(sValue[1]);
        return index < array.Length ? array[index] : string.Empty;
    }

    /// <summary>
    /// 得到属性
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string GetAttr(string value)
    {
        string[] sValue = { Setting.attrFormat[0].ToString(), Setting.attrFormat[1].ToString() };
        string[] array = value.Split(sValue[0].ToCharArray()[0]);
        int index = int.Parse(sValue[1]);
        return index < array.Length ? array[index] : string.Empty;
    }

    /// <summary>
    /// 得到值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool TryParse(object value, string type, out object result)
    {
        bool bResult = true;
        result = value;
        try
        {
            switch (type)
            {
            case "bool":
            {
                result = int.Parse(value.ToString()) != 0;
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
                if (type.Equals(Setting.langTag))
                {

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
            bResult = false;
        }
        return bResult;
    }
    #endregion
}
