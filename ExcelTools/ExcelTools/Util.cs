using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public sealed class Util
{
    #region
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
    #endregion
}