using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public sealed class Util
{
    #region
    /// <summary>
    /// 文件夹模态框
    /// </summary>
    /// <param name="textBox"></param>
    /// <param name="path"></param>
    public static void FolderDialog(string path, Action<string> action, bool ShowNewFolderButton = true)
    {
        FolderBrowserDialog dialog = new FolderBrowserDialog();//OpenFileDialog, FolderBrowserDialog
        dialog.SelectedPath = path;
        dialog.ShowNewFolderButton = ShowNewFolderButton;
        //dialog.Description = "oooooo";
        //dialog.Multiselect = true;
        //dialog.Filter = "*.*|*.*";

        //dialog.ValidateNames = false;
        //dialog.CheckFileExists = false;
        //dialog.CheckPathExists = true;
        //dialog.FileName = "Folder Selection";

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            if (action != null)
            {
                action(dialog.SelectedPath);
            }
        }
    }

    /// <summary>
    /// 显示消息框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="action"></param>
    /// <param name="title"></param>
    public static void ShowMessageBox(string text, Action action, string title = "Error", MessageBoxIcon icon = MessageBoxIcon.Error)
    {
        DialogResult dialogResult = MessageBox.Show(text, title, MessageBoxButtons.OK, icon);
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
    /// 得到Excel文件名
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetExcelFileName(string fileName)
    {
        return fileName.Split('-')[0];
    }
    #endregion
}