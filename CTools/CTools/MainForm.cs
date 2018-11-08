using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace CTools
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.excelFolderText.Text = Setting.GetString(Option.Excel);
            this.jsonFolderText.Text = Setting.GetString(Option.Json);
            this.typeIndexText.Text = Setting.GetString(Option.TypeIndex);
            this.typeValueSymbolText.Text = Setting.GetString(Option.TypeFormat);
            this.typeValueIndexText.Text = Setting.GetString(Option.TypeFormatIndex);
            this.nameIndexText.Text = Setting.GetString(Option.NameIndex);
            this.nameValueSymbolText.Text = Setting.GetString(Option.NameFormat);
            this.nameValueIndexText.Text = Setting.GetString(Option.NameFormatIndex);
            this.attrIndexText.Text = Setting.GetString(Option.AttrIndex);
            this.attrValueSymbolText.Text = Setting.GetString(Option.AttrFormat);
            this.attrValueIndexText.Text = Setting.GetString(Option.AttrFormatIndex);
            this.pointIndexText.Text = Setting.GetString(Option.PointIndex);
            this.langTagText.Text = Setting.GetString(Option.LangTag);
            this.langFormatUUID.Text = Setting.GetString(Option.LangUUID);
            this.langFormatName.Text = Setting.GetString(Option.Lang);
            this.langFileText.Text = Setting.GetString(Option.LangFile);

            fix_Click(null, null);
        }

        private void fix_Click(object sender, EventArgs e)
        {
            bool Enabled = !this.typeIndexText.Enabled;
            this.excelFolderText.Enabled = Enabled;
            this.jsonFolderText.Enabled = Enabled;
            this.typeIndexText.Enabled = Enabled;
            this.typeValueSymbolText.Enabled = Enabled;
            this.typeValueIndexText.Enabled = Enabled;
            this.nameIndexText.Enabled = Enabled;
            this.nameValueSymbolText.Enabled = Enabled;
            this.nameValueIndexText.Enabled = Enabled;
            this.attrIndexText.Enabled = Enabled;
            this.attrValueSymbolText.Enabled = Enabled;
            this.attrValueIndexText.Enabled = Enabled;
            this.pointIndexText.Enabled = Enabled;
            this.langTagText.Enabled = Enabled;
            this.langFormatUUID.Enabled = Enabled;
            this.langFormatName.Enabled = Enabled;
            this.langFileText.Enabled = Enabled;

            if (Enabled)
            {
                this.fix.BackgroundImage = global::CTools.Properties.Resources.save;
            }
            else
            {
                this.fix.BackgroundImage = global::CTools.Properties.Resources.fix;

                Setting.SetString(Option.Excel, this.excelFolderText.Text);
                Setting.SetString(Option.Json, this.jsonFolderText.Text);
                Setting.SetString(Option.TypeIndex, this.typeIndexText.Text);
                Setting.SetString(Option.TypeFormat, this.typeValueSymbolText.Text);
                Setting.SetString(Option.TypeFormatIndex, this.typeValueIndexText.Text);
                Setting.SetString(Option.NameIndex, this.nameIndexText.Text);
                Setting.SetString(Option.NameFormat, this.nameValueSymbolText.Text);
                Setting.SetString(Option.NameFormatIndex, this.nameValueIndexText.Text);
                Setting.SetString(Option.AttrIndex, this.attrIndexText.Text);
                Setting.SetString(Option.AttrFormat, this.attrValueSymbolText.Text);
                Setting.SetString(Option.AttrFormatIndex, this.attrValueIndexText.Text);
                Setting.SetString(Option.PointIndex, this.pointIndexText.Text);
                Setting.SetString(Option.LangTag, this.langTagText.Text);
                Setting.SetString(Option.LangUUID, this.langFormatUUID.Text);
                Setting.SetString(Option.Lang, this.langFormatName.Text);
                Setting.SetString(Option.LangFile, this.langFileText.Text);
                Setting.Save();
            }
        }

        private void excelFolderBtn_Click(object sender, EventArgs e)
        {
            Util.FolderDialog(Setting.GetString(Option.Excel), (path)=> {
                this.excelFolderText.Text = path;
                Setting.SetString(Option.Excel, this.excelFolderText.Text);
                Setting.Save();
            }, false);
        }

        private void jsonFolderBtn_Click(object sender, EventArgs e)
        {
            Util.FolderDialog(Setting.GetString(Option.Json), (path) => {
                this.jsonFolderText.Text = path;
                Setting.SetString(Option.Json, this.jsonFolderText.Text);
                Setting.Save();
            }, true);
        }

        private Dictionary<string, bool> fileList = new Dictionary<string, bool>();
        private void excelFolderText_TextChanged(object sender, EventArgs e)
        {
            fileList.Clear();
            this.fileCheckedListBox.Items.Clear();

            string path = this.excelFolderText.Text;
            if (Directory.Exists(path))
            {
                string[] file = Directory.GetFiles(path, "*.xls*", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < file.Length; ++i)
                {
                    path = Path.GetFileName(file[i]);
                    fileList.Add(path, this.fileCheckBox.Checked);
                    if (string.IsNullOrEmpty(this.fileSearchText.Text) || path.Contains(this.fileSearchText.Text))
                    {
                        this.fileCheckedListBox.Items.Add(path, this.fileCheckBox.Checked);
                    }
                }
            }
        }

        private void fileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool Checked = this.fileCheckBox.Checked;

            List<string> list = new List<string>();
            foreach (var item in this.fileList)
            {
                if (string.IsNullOrEmpty(this.fileSearchText.Text) || item.Key.Contains(this.fileSearchText.Text))
                {
                    list.Add(item.Key);
                }
            }
            foreach (var item in list)
            {
                this.fileList[item] = Checked;
            }

            for (int i = 0; i < this.fileCheckedListBox.Items.Count; ++i)
            {
                string name = this.fileCheckedListBox.Items[i].ToString();
                if (string.IsNullOrEmpty(this.fileSearchText.Text) || name.Contains(this.fileSearchText.Text))
                {
                    this.fileCheckedListBox.SetItemChecked(i, Checked);
                }
            }
        }

        private void fileSearchText_TextChanged(object sender, EventArgs e)
        {
            this.fileCheckedListBox.Items.Clear();
            foreach (var item in this.fileList)
            {
                if (string.IsNullOrEmpty(this.fileSearchText.Text) || item.Key.Contains(this.fileSearchText.Text))
                {
                    this.fileCheckedListBox.Items.Add(item.Key, item.Value);
                }
            }
        }

        private void fileCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = this.fileCheckedListBox.SelectedIndex;
            string value = this.fileCheckedListBox.Items[index].ToString();
            if (this.fileList.ContainsKey(value))
            {
                this.fileList[value] = this.fileCheckedListBox.GetItemChecked(index);
            }
        }

        private void checkBtn_Click(object sender, EventArgs e)
        {
            if (!IsSave())
            {
                Util.ShowMessageBox(string.Format("请先锁定配置"), null, "提示", MessageBoxIcon.Warning);
                return;
            }
            // 读取配置
            ExcelDataSet.instance.Clear();
            bool bResult = true;
            for (int i = 0; i < this.fileCheckedListBox.Items.Count; ++i)
            {
                string name = this.fileCheckedListBox.Items[i].ToString();
                if (this.fileCheckedListBox .GetItemChecked(i) && (string.IsNullOrEmpty(this.fileSearchText.Text) || name.Contains(this.fileSearchText.Text)))
                {
                    var data = new ExcelReader().Open(Path.Combine(this.excelFolderText.Text, name), ()=> { bResult = false; });
                    if (!bResult)
                    {
                        break;
                    }
                    name = Util.GetExcelFileName(Path.GetFileNameWithoutExtension(name));
                    if (!ExcelDataSet.instance.TryAdd(name, data))
                    {
                        Util.ShowMessageBox(string.Format("{0}\n相同前缀'{1}'文件重复加载", this.fileCheckedListBox.Items[i], Util.GetExcelFileName(name)), () => { bResult = false; });
                        break;
                    }
                }
            }
            if (bResult)
            {
                Util.ShowMessageBox("完成", null, "提示", MessageBoxIcon.Information);
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!IsSave())
            {
                Util.ShowMessageBox(string.Format("请先锁定配置"), null, "提示", MessageBoxIcon.Warning);
                return;
            }
            // 读取配置
            ExcelDataSet.instance.Clear();
            bool bResult = true;
            for (int i = 0; i < this.fileCheckedListBox.Items.Count; ++i)
            {
                string name = this.fileCheckedListBox.Items[i].ToString();
                if (this.fileCheckedListBox.GetItemChecked(i) && (string.IsNullOrEmpty(this.fileSearchText.Text) || name.Contains(this.fileSearchText.Text)))
                {
                    var data = new ExcelReader().Open(Path.Combine(this.excelFolderText.Text, name), () => { bResult = false; });
                    if (!bResult)
                    {
                        break;
                    }
                    name = Util.GetExcelFileName(Path.GetFileNameWithoutExtension(name));
                    if (!ExcelDataSet.instance.TryAdd(name, data))
                    {
                        Util.ShowMessageBox(string.Format("{0}\n相同前缀'{1}'文件重复加载", this.fileCheckedListBox.Items[i], Util.GetExcelFileName(name)), () => { bResult = false; });
                        break;
                    }
                }
            }
            if (bResult)
            {
                foreach (var kvp in ExcelDataSet.instance.data)
                {
                    Encoding cd = new UTF8Encoding(false);
                    string json = JsonConvert.SerializeObject(kvp.Value, Formatting.Indented);
                    using (FileStream file = new FileStream(Path.Combine(this.jsonFolderText.Text, kvp.Key + ".json"), FileMode.Create, FileAccess.Write))
                    {
                        using (TextWriter writer = new StreamWriter(file, cd))
                        {
                            writer.Write(json);
                        }
                    }
                }
                Util.ShowMessageBox("完成", null, "提示", MessageBoxIcon.Asterisk);
            }
        }

        private bool IsSave()
        {
            return this.excelFolderText.Text.Equals(Setting.GetString(Option.Excel)) &&
                this.jsonFolderText.Text.Equals(Setting.GetString(Option.Json)) &&
                this.typeIndexText.Text.Equals(Setting.GetString(Option.TypeIndex)) &&
                this.typeValueSymbolText.Text.Equals(Setting.GetString(Option.TypeFormat)) &&
                this.typeValueIndexText.Text.Equals(Setting.GetString(Option.TypeFormatIndex)) &&
                this.nameIndexText.Text.Equals(Setting.GetString(Option.NameIndex)) &&
                this.nameValueSymbolText.Text.Equals(Setting.GetString(Option.NameFormat)) &&
                this.nameValueIndexText.Text.Equals(Setting.GetString(Option.NameFormatIndex)) &&
                this.attrIndexText.Text.Equals(Setting.GetString(Option.AttrIndex)) &&
                this.attrValueSymbolText.Text.Equals(Setting.GetString(Option.AttrFormat)) &&
                this.attrValueIndexText.Text.Equals(Setting.GetString(Option.AttrFormatIndex)) &&
                this.pointIndexText.Text.Equals(Setting.GetString(Option.PointIndex)) &&
                this.langTagText.Text.Equals(Setting.GetString(Option.LangTag)) &&
                this.langFormatUUID.Text.Equals(Setting.GetString(Option.LangUUID)) &&
                this.langFormatName.Text.Equals(Setting.GetString(Option.Lang)) &&
                this.langFileText.Text.Equals(Setting.GetString(Option.LangFile));
        }

        /// <summary>
        /// 控件激活设置
        /// </summary>
        /// <param name="enabled"></param>
        private void ControlActive(bool enabled)
        {
            this.excelFolderText.Enabled = enabled;
            this.excelFolderBtn.Enabled = enabled;
            this.jsonFolderText.Enabled = enabled;
            this.jsonFolderBtn.Enabled = enabled;
            this.fileCheckedListBox.Enabled = enabled;
            this.checkBtn.Enabled = enabled;
            this.startBtn.Enabled = enabled;
            this.fix.Enabled = enabled;
            this.fileCheckBox.Enabled = enabled;
            this.fileSearchText.Enabled = enabled;

            enabled = false;
            this.excelFolderText.Enabled = enabled;
            this.jsonFolderText.Enabled = enabled;
            this.typeIndexText.Enabled = enabled;
            this.typeValueSymbolText.Enabled = enabled;
            this.typeValueIndexText.Enabled = enabled;
            this.nameIndexText.Enabled = enabled;
            this.nameValueSymbolText.Enabled = enabled;
            this.nameValueIndexText.Enabled = enabled;
            this.attrIndexText.Enabled = enabled;
            this.attrValueSymbolText.Enabled = enabled;
            this.attrValueIndexText.Enabled = enabled;
            this.pointIndexText.Enabled = enabled;
            this.langTagText.Enabled = enabled;
            this.langFormatUUID.Enabled = enabled;
            this.langFormatName.Enabled = enabled;
            this.langFileText.Enabled = enabled;
            this.fix.BackgroundImage = global::CTools.Properties.Resources.fix;
        }
    }
}
