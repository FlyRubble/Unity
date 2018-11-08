using System.Windows.Forms;

namespace CTools
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// 主要组盒子
        /// </summary>
        private GroupBox mainGroupBox = null;

        /// <summary>
        /// Json文件夹
        /// </summary>
        private Label jsonFolder = null;

        /// <summary>
        /// Json文件夹路径
        /// </summary>
        private TextBox jsonFolderText = null;

        /// <summary>
        /// Json文件夹按钮
        /// </summary>
        private Button jsonFolderBtn = null;

        /// <summary>
        /// Excel文件夹
        /// </summary>
        private Label excelFolder = null;

        /// <summary>
        /// Excel文件夹路径
        /// </summary>
        private TextBox excelFolderText = null;

        /// <summary>
        /// Excel文件夹按钮
        /// </summary>
        private Button excelFolderBtn = null;

        /// <summary>
        /// 文件组盒子
        /// </summary>
        private GroupBox fileGroupBox = null;

        /// <summary>
        /// file CheckBox
        /// </summary>
        private CheckBox fileCheckBox = null;

        /// <summary>
        /// file Search Text
        /// </summary>
        private TextBox fileSearchText = null;

        /// <summary>
        /// File Search
        /// </summary>
        private Label fileSearch = null;

        /// <summary>
        /// file Checked List Box
        /// </summary>
        private CheckedListBox fileCheckedListBox = null;

        /// <summary>
        /// 选项组盒子
        /// </summary>
        private GroupBox optionsGroupBox = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label typeTitle = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label typeIndex = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox typeIndexText = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label typeValueTitle = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox typeValueSymbolText = null;
        
        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox typeValueIndexText = null;
        
        /// <summary>
        /// Label
        /// </summary>
        private Label nameTitle = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label nameIndex = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox nameIndexText = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label nameValueTitle = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox nameValueSymbolText = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox nameValueIndexText = null;
        
        /// <summary>
        /// Label
        /// </summary>
        private Label attrTitle = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label attrIndex = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox attrIndexText = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label attrValueTitle = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox attrValueSymbolText = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox attrValueIndexText = null;
        
        /// <summary>
        /// Label
        /// </summary>
        private Label pointTitle = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label pointIndexTitle = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox pointIndexText = null;
        
        /// <summary>
        /// Label
        /// </summary>
        private Label langTitle = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label langTagTitle = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox langTagText = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label langFormatTitle = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox langFormatUUID = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox langFormatName = null;

        /// <summary>
        /// Label
        /// </summary>
        private Label langFileTitle = null;

        /// <summary>
        /// TextBox
        /// </summary>
        private TextBox langFileText = null;

        /// <summary>
        /// 检查
        /// </summary>
        private Button checkBtn = null;

        /// <summary>
        /// 开始
        /// </summary>
        private Button startBtn = null;

        /// <summary>
        /// 版权
        /// </summary>
        private Label copyright = null;

        /// <summary>
        /// 版本
        /// </summary>
        private Label version = null;

        /// <summary>
        /// 修正按钮
        /// </summary>
        private Button fix;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainGroupBox = new System.Windows.Forms.GroupBox();
            this.jsonFolder = new System.Windows.Forms.Label();
            this.jsonFolderText = new System.Windows.Forms.TextBox();
            this.jsonFolderBtn = new System.Windows.Forms.Button();
            this.excelFolder = new System.Windows.Forms.Label();
            this.excelFolderText = new System.Windows.Forms.TextBox();
            this.excelFolderBtn = new System.Windows.Forms.Button();
            this.fileGroupBox = new System.Windows.Forms.GroupBox();
            this.fileCheckBox = new System.Windows.Forms.CheckBox();
            this.fileSearchText = new System.Windows.Forms.TextBox();
            this.fileSearch = new System.Windows.Forms.Label();
            this.fileCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.fix = new System.Windows.Forms.Button();
            this.langFileText = new System.Windows.Forms.TextBox();
            this.langFileTitle = new System.Windows.Forms.Label();
            this.langFormatName = new System.Windows.Forms.TextBox();
            this.langFormatUUID = new System.Windows.Forms.TextBox();
            this.langFormatTitle = new System.Windows.Forms.Label();
            this.langTagText = new System.Windows.Forms.TextBox();
            this.langTagTitle = new System.Windows.Forms.Label();
            this.langTitle = new System.Windows.Forms.Label();
            this.pointIndexText = new System.Windows.Forms.TextBox();
            this.pointIndexTitle = new System.Windows.Forms.Label();
            this.pointTitle = new System.Windows.Forms.Label();
            this.attrValueIndexText = new System.Windows.Forms.TextBox();
            this.attrValueSymbolText = new System.Windows.Forms.TextBox();
            this.attrValueTitle = new System.Windows.Forms.Label();
            this.attrIndexText = new System.Windows.Forms.TextBox();
            this.attrIndex = new System.Windows.Forms.Label();
            this.attrTitle = new System.Windows.Forms.Label();
            this.nameValueIndexText = new System.Windows.Forms.TextBox();
            this.nameValueSymbolText = new System.Windows.Forms.TextBox();
            this.nameValueTitle = new System.Windows.Forms.Label();
            this.nameIndexText = new System.Windows.Forms.TextBox();
            this.nameIndex = new System.Windows.Forms.Label();
            this.nameTitle = new System.Windows.Forms.Label();
            this.typeValueIndexText = new System.Windows.Forms.TextBox();
            this.typeValueSymbolText = new System.Windows.Forms.TextBox();
            this.typeValueTitle = new System.Windows.Forms.Label();
            this.typeTitle = new System.Windows.Forms.Label();
            this.typeIndex = new System.Windows.Forms.Label();
            this.typeIndexText = new System.Windows.Forms.TextBox();
            this.checkBtn = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.copyright = new System.Windows.Forms.Label();
            this.version = new System.Windows.Forms.Label();
            this.mainGroupBox.SuspendLayout();
            this.fileGroupBox.SuspendLayout();
            this.optionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGroupBox
            // 
            this.mainGroupBox.Controls.Add(this.jsonFolder);
            this.mainGroupBox.Controls.Add(this.jsonFolderText);
            this.mainGroupBox.Controls.Add(this.jsonFolderBtn);
            this.mainGroupBox.Controls.Add(this.excelFolder);
            this.mainGroupBox.Controls.Add(this.excelFolderText);
            this.mainGroupBox.Controls.Add(this.excelFolderBtn);
            this.mainGroupBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mainGroupBox.Location = new System.Drawing.Point(14, 3);
            this.mainGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mainGroupBox.Name = "mainGroupBox";
            this.mainGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mainGroupBox.Size = new System.Drawing.Size(743, 109);
            this.mainGroupBox.TabIndex = 2;
            this.mainGroupBox.TabStop = false;
            this.mainGroupBox.Text = "目录";
            // 
            // jsonFolder
            // 
            this.jsonFolder.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.jsonFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.jsonFolder.Location = new System.Drawing.Point(20, 69);
            this.jsonFolder.Margin = new System.Windows.Forms.Padding(0);
            this.jsonFolder.Name = "jsonFolder";
            this.jsonFolder.Size = new System.Drawing.Size(73, 21);
            this.jsonFolder.TabIndex = 0;
            this.jsonFolder.Text = "Json目录：";
            this.jsonFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // jsonFolderText
            // 
            this.jsonFolderText.Location = new System.Drawing.Point(94, 68);
            this.jsonFolderText.Name = "jsonFolderText";
            this.jsonFolderText.Size = new System.Drawing.Size(577, 23);
            this.jsonFolderText.TabIndex = 1;
            // 
            // jsonFolderBtn
            // 
            this.jsonFolderBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.jsonFolderBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.jsonFolderBtn.Location = new System.Drawing.Point(681, 68);
            this.jsonFolderBtn.Margin = new System.Windows.Forms.Padding(0);
            this.jsonFolderBtn.Name = "jsonFolderBtn";
            this.jsonFolderBtn.Size = new System.Drawing.Size(48, 23);
            this.jsonFolderBtn.TabIndex = 2;
            this.jsonFolderBtn.Text = "···";
            this.jsonFolderBtn.Click += new System.EventHandler(this.jsonFolderBtn_Click);
            // 
            // excelFolder
            // 
            this.excelFolder.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.excelFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.excelFolder.Location = new System.Drawing.Point(20, 28);
            this.excelFolder.Margin = new System.Windows.Forms.Padding(0);
            this.excelFolder.Name = "excelFolder";
            this.excelFolder.Size = new System.Drawing.Size(73, 21);
            this.excelFolder.TabIndex = 0;
            this.excelFolder.Text = "Excel目录：";
            this.excelFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // excelFolderText
            // 
            this.excelFolderText.Location = new System.Drawing.Point(94, 27);
            this.excelFolderText.Name = "excelFolderText";
            this.excelFolderText.Size = new System.Drawing.Size(577, 23);
            this.excelFolderText.TabIndex = 1;
            this.excelFolderText.TextChanged += new System.EventHandler(this.excelFolderText_TextChanged);
            // 
            // excelFolderBtn
            // 
            this.excelFolderBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.excelFolderBtn.FlatAppearance.BorderSize = 0;
            this.excelFolderBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.excelFolderBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.excelFolderBtn.Location = new System.Drawing.Point(681, 27);
            this.excelFolderBtn.Margin = new System.Windows.Forms.Padding(0);
            this.excelFolderBtn.Name = "excelFolderBtn";
            this.excelFolderBtn.Size = new System.Drawing.Size(48, 23);
            this.excelFolderBtn.TabIndex = 2;
            this.excelFolderBtn.Text = "···";
            this.excelFolderBtn.Click += new System.EventHandler(this.excelFolderBtn_Click);
            // 
            // fileGroupBox
            // 
            this.fileGroupBox.Controls.Add(this.fileCheckBox);
            this.fileGroupBox.Controls.Add(this.fileSearchText);
            this.fileGroupBox.Controls.Add(this.fileSearch);
            this.fileGroupBox.Controls.Add(this.fileCheckedListBox);
            this.fileGroupBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fileGroupBox.Location = new System.Drawing.Point(13, 113);
            this.fileGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fileGroupBox.Name = "fileGroupBox";
            this.fileGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fileGroupBox.Size = new System.Drawing.Size(744, 537);
            this.fileGroupBox.TabIndex = 2;
            this.fileGroupBox.TabStop = false;
            this.fileGroupBox.Text = "文件";
            // 
            // fileCheckBox
            // 
            this.fileCheckBox.Checked = true;
            this.fileCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fileCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fileCheckBox.Location = new System.Drawing.Point(9, 25);
            this.fileCheckBox.Name = "fileCheckBox";
            this.fileCheckBox.Size = new System.Drawing.Size(54, 24);
            this.fileCheckBox.TabIndex = 0;
            this.fileCheckBox.Text = "全部";
            this.fileCheckBox.CheckedChanged += new System.EventHandler(this.fileCheckBox_CheckedChanged);
            // 
            // fileSearchText
            // 
            this.fileSearchText.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileSearchText.Location = new System.Drawing.Point(408, 24);
            this.fileSearchText.Name = "fileSearchText";
            this.fileSearchText.Size = new System.Drawing.Size(326, 24);
            this.fileSearchText.TabIndex = 1;
            this.fileSearchText.TextChanged += new System.EventHandler(this.fileSearchText_TextChanged);
            // 
            // fileSearch
            // 
            this.fileSearch.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileSearch.Location = new System.Drawing.Point(350, 25);
            this.fileSearch.Name = "fileSearch";
            this.fileSearch.Size = new System.Drawing.Size(54, 23);
            this.fileSearch.TabIndex = 2;
            this.fileSearch.Text = "Search";
            this.fileSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fileCheckedListBox
            // 
            this.fileCheckedListBox.CheckOnClick = true;
            this.fileCheckedListBox.Location = new System.Drawing.Point(6, 58);
            this.fileCheckedListBox.Name = "fileCheckedListBox";
            this.fileCheckedListBox.Size = new System.Drawing.Size(731, 472);
            this.fileCheckedListBox.TabIndex = 1;
            this.fileCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.fileCheckedListBox_SelectedIndexChanged);
            // 
            // optionsGroupBox
            // 
            this.optionsGroupBox.Controls.Add(this.fix);
            this.optionsGroupBox.Controls.Add(this.langFileText);
            this.optionsGroupBox.Controls.Add(this.langFileTitle);
            this.optionsGroupBox.Controls.Add(this.langFormatName);
            this.optionsGroupBox.Controls.Add(this.langFormatUUID);
            this.optionsGroupBox.Controls.Add(this.langFormatTitle);
            this.optionsGroupBox.Controls.Add(this.langTagText);
            this.optionsGroupBox.Controls.Add(this.langTagTitle);
            this.optionsGroupBox.Controls.Add(this.langTitle);
            this.optionsGroupBox.Controls.Add(this.pointIndexText);
            this.optionsGroupBox.Controls.Add(this.pointIndexTitle);
            this.optionsGroupBox.Controls.Add(this.pointTitle);
            this.optionsGroupBox.Controls.Add(this.attrValueIndexText);
            this.optionsGroupBox.Controls.Add(this.attrValueSymbolText);
            this.optionsGroupBox.Controls.Add(this.attrValueTitle);
            this.optionsGroupBox.Controls.Add(this.attrIndexText);
            this.optionsGroupBox.Controls.Add(this.attrIndex);
            this.optionsGroupBox.Controls.Add(this.attrTitle);
            this.optionsGroupBox.Controls.Add(this.nameValueIndexText);
            this.optionsGroupBox.Controls.Add(this.nameValueSymbolText);
            this.optionsGroupBox.Controls.Add(this.nameValueTitle);
            this.optionsGroupBox.Controls.Add(this.nameIndexText);
            this.optionsGroupBox.Controls.Add(this.nameIndex);
            this.optionsGroupBox.Controls.Add(this.nameTitle);
            this.optionsGroupBox.Controls.Add(this.typeValueIndexText);
            this.optionsGroupBox.Controls.Add(this.typeValueSymbolText);
            this.optionsGroupBox.Controls.Add(this.typeValueTitle);
            this.optionsGroupBox.Controls.Add(this.typeTitle);
            this.optionsGroupBox.Controls.Add(this.typeIndex);
            this.optionsGroupBox.Controls.Add(this.typeIndexText);
            this.optionsGroupBox.Controls.Add(this.checkBtn);
            this.optionsGroupBox.Controls.Add(this.startBtn);
            this.optionsGroupBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.optionsGroupBox.Location = new System.Drawing.Point(764, 3);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Size = new System.Drawing.Size(288, 647);
            this.optionsGroupBox.TabIndex = 3;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "选项";
            // 
            // fix
            // 
            this.fix.AutoSize = true;
            this.fix.BackColor = System.Drawing.Color.Transparent;
            this.fix.BackgroundImage = global::CTools.Properties.Resources.fix;
            this.fix.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.fix.CausesValidation = false;
            this.fix.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fix.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.fix.FlatAppearance.BorderSize = 0;
            this.fix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fix.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fix.ForeColor = System.Drawing.SystemColors.Control;
            this.fix.Location = new System.Drawing.Point(268, 12);
            this.fix.Margin = new System.Windows.Forms.Padding(0);
            this.fix.Name = "fix";
            this.fix.Size = new System.Drawing.Size(16, 21);
            this.fix.TabIndex = 29;
            this.fix.TabStop = false;
            this.fix.UseVisualStyleBackColor = false;
            this.fix.Click += new System.EventHandler(this.fix_Click);
            // 
            // langFileText
            // 
            this.langFileText.Location = new System.Drawing.Point(99, 488);
            this.langFileText.MaxLength = 64;
            this.langFileText.Name = "langFileText";
            this.langFileText.Size = new System.Drawing.Size(181, 23);
            this.langFileText.TabIndex = 28;
            // 
            // langFileTitle
            // 
            this.langFileTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.langFileTitle.Location = new System.Drawing.Point(32, 488);
            this.langFileTitle.Margin = new System.Windows.Forms.Padding(0);
            this.langFileTitle.Name = "langFileTitle";
            this.langFileTitle.Size = new System.Drawing.Size(60, 21);
            this.langFileTitle.TabIndex = 27;
            this.langFileTitle.Text = "File：";
            this.langFileTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // langFormatName
            // 
            this.langFormatName.Location = new System.Drawing.Point(194, 457);
            this.langFormatName.MaxLength = 32;
            this.langFormatName.Name = "langFormatName";
            this.langFormatName.Size = new System.Drawing.Size(86, 23);
            this.langFormatName.TabIndex = 26;
            // 
            // langFormatUUID
            // 
            this.langFormatUUID.Location = new System.Drawing.Point(99, 457);
            this.langFormatUUID.MaxLength = 32;
            this.langFormatUUID.Name = "langFormatUUID";
            this.langFormatUUID.Size = new System.Drawing.Size(86, 23);
            this.langFormatUUID.TabIndex = 25;
            // 
            // langFormatTitle
            // 
            this.langFormatTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.langFormatTitle.Location = new System.Drawing.Point(32, 457);
            this.langFormatTitle.Margin = new System.Windows.Forms.Padding(0);
            this.langFormatTitle.Name = "langFormatTitle";
            this.langFormatTitle.Size = new System.Drawing.Size(60, 21);
            this.langFormatTitle.TabIndex = 24;
            this.langFormatTitle.Text = "Format：";
            this.langFormatTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // langTagText
            // 
            this.langTagText.Location = new System.Drawing.Point(99, 426);
            this.langTagText.MaxLength = 64;
            this.langTagText.Name = "langTagText";
            this.langTagText.Size = new System.Drawing.Size(181, 23);
            this.langTagText.TabIndex = 23;
            // 
            // langTagTitle
            // 
            this.langTagTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.langTagTitle.Location = new System.Drawing.Point(32, 426);
            this.langTagTitle.Margin = new System.Windows.Forms.Padding(0);
            this.langTagTitle.Name = "langTagTitle";
            this.langTagTitle.Size = new System.Drawing.Size(52, 21);
            this.langTagTitle.TabIndex = 22;
            this.langTagTitle.Text = "Tag：";
            this.langTagTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // langTitle
            // 
            this.langTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.langTitle.Location = new System.Drawing.Point(8, 399);
            this.langTitle.Name = "langTitle";
            this.langTitle.Size = new System.Drawing.Size(64, 21);
            this.langTitle.TabIndex = 21;
            this.langTitle.Text = "Lang：";
            // 
            // pointIndexText
            // 
            this.pointIndexText.Location = new System.Drawing.Point(99, 358);
            this.pointIndexText.MaxLength = 16;
            this.pointIndexText.Name = "pointIndexText";
            this.pointIndexText.Size = new System.Drawing.Size(181, 23);
            this.pointIndexText.TabIndex = 20;
            // 
            // pointIndexTitle
            // 
            this.pointIndexTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pointIndexTitle.Location = new System.Drawing.Point(32, 358);
            this.pointIndexTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pointIndexTitle.Name = "pointIndexTitle";
            this.pointIndexTitle.Size = new System.Drawing.Size(52, 21);
            this.pointIndexTitle.TabIndex = 19;
            this.pointIndexTitle.Text = "Index：";
            this.pointIndexTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pointTitle
            // 
            this.pointTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pointTitle.Location = new System.Drawing.Point(8, 331);
            this.pointTitle.Name = "pointTitle";
            this.pointTitle.Size = new System.Drawing.Size(64, 21);
            this.pointTitle.TabIndex = 18;
            this.pointTitle.Text = "Point：";
            // 
            // attrValueIndexText
            // 
            this.attrValueIndexText.Location = new System.Drawing.Point(194, 290);
            this.attrValueIndexText.MaxLength = 16;
            this.attrValueIndexText.Name = "attrValueIndexText";
            this.attrValueIndexText.Size = new System.Drawing.Size(86, 23);
            this.attrValueIndexText.TabIndex = 17;
            // 
            // attrValueSymbolText
            // 
            this.attrValueSymbolText.Location = new System.Drawing.Point(99, 290);
            this.attrValueSymbolText.MaxLength = 16;
            this.attrValueSymbolText.Name = "attrValueSymbolText";
            this.attrValueSymbolText.Size = new System.Drawing.Size(86, 23);
            this.attrValueSymbolText.TabIndex = 16;
            // 
            // attrValueTitle
            // 
            this.attrValueTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attrValueTitle.Location = new System.Drawing.Point(32, 290);
            this.attrValueTitle.Margin = new System.Windows.Forms.Padding(0);
            this.attrValueTitle.Name = "attrValueTitle";
            this.attrValueTitle.Size = new System.Drawing.Size(52, 23);
            this.attrValueTitle.TabIndex = 15;
            this.attrValueTitle.Text = "Value：";
            this.attrValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // attrIndexText
            // 
            this.attrIndexText.Location = new System.Drawing.Point(99, 259);
            this.attrIndexText.MaxLength = 16;
            this.attrIndexText.Name = "attrIndexText";
            this.attrIndexText.Size = new System.Drawing.Size(181, 23);
            this.attrIndexText.TabIndex = 14;
            // 
            // attrIndex
            // 
            this.attrIndex.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attrIndex.Location = new System.Drawing.Point(32, 259);
            this.attrIndex.Margin = new System.Windows.Forms.Padding(0);
            this.attrIndex.Name = "attrIndex";
            this.attrIndex.Size = new System.Drawing.Size(52, 21);
            this.attrIndex.TabIndex = 13;
            this.attrIndex.Text = "Index：";
            this.attrIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // attrTitle
            // 
            this.attrTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attrTitle.Location = new System.Drawing.Point(8, 232);
            this.attrTitle.Name = "attrTitle";
            this.attrTitle.Size = new System.Drawing.Size(64, 21);
            this.attrTitle.TabIndex = 12;
            this.attrTitle.Text = "Attr：";
            // 
            // nameValueIndexText
            // 
            this.nameValueIndexText.Location = new System.Drawing.Point(194, 191);
            this.nameValueIndexText.MaxLength = 16;
            this.nameValueIndexText.Name = "nameValueIndexText";
            this.nameValueIndexText.Size = new System.Drawing.Size(86, 23);
            this.nameValueIndexText.TabIndex = 11;
            // 
            // nameValueSymbolText
            // 
            this.nameValueSymbolText.Location = new System.Drawing.Point(99, 191);
            this.nameValueSymbolText.MaxLength = 16;
            this.nameValueSymbolText.Name = "nameValueSymbolText";
            this.nameValueSymbolText.Size = new System.Drawing.Size(86, 23);
            this.nameValueSymbolText.TabIndex = 10;
            // 
            // nameValueTitle
            // 
            this.nameValueTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameValueTitle.Location = new System.Drawing.Point(32, 191);
            this.nameValueTitle.Margin = new System.Windows.Forms.Padding(0);
            this.nameValueTitle.Name = "nameValueTitle";
            this.nameValueTitle.Size = new System.Drawing.Size(52, 23);
            this.nameValueTitle.TabIndex = 9;
            this.nameValueTitle.Text = "Value：";
            this.nameValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nameIndexText
            // 
            this.nameIndexText.Location = new System.Drawing.Point(99, 160);
            this.nameIndexText.MaxLength = 16;
            this.nameIndexText.Name = "nameIndexText";
            this.nameIndexText.Size = new System.Drawing.Size(181, 23);
            this.nameIndexText.TabIndex = 8;
            // 
            // nameIndex
            // 
            this.nameIndex.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameIndex.Location = new System.Drawing.Point(32, 160);
            this.nameIndex.Margin = new System.Windows.Forms.Padding(0);
            this.nameIndex.Name = "nameIndex";
            this.nameIndex.Size = new System.Drawing.Size(52, 21);
            this.nameIndex.TabIndex = 7;
            this.nameIndex.Text = "Index：";
            this.nameIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nameTitle
            // 
            this.nameTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTitle.Location = new System.Drawing.Point(8, 133);
            this.nameTitle.Name = "nameTitle";
            this.nameTitle.Size = new System.Drawing.Size(64, 21);
            this.nameTitle.TabIndex = 6;
            this.nameTitle.Text = "Name：";
            // 
            // typeValueIndexText
            // 
            this.typeValueIndexText.Location = new System.Drawing.Point(194, 92);
            this.typeValueIndexText.MaxLength = 16;
            this.typeValueIndexText.Name = "typeValueIndexText";
            this.typeValueIndexText.Size = new System.Drawing.Size(86, 23);
            this.typeValueIndexText.TabIndex = 5;
            // 
            // typeValueSymbolText
            // 
            this.typeValueSymbolText.Location = new System.Drawing.Point(99, 92);
            this.typeValueSymbolText.MaxLength = 16;
            this.typeValueSymbolText.Name = "typeValueSymbolText";
            this.typeValueSymbolText.Size = new System.Drawing.Size(86, 23);
            this.typeValueSymbolText.TabIndex = 4;
            // 
            // typeValueTitle
            // 
            this.typeValueTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeValueTitle.Location = new System.Drawing.Point(32, 92);
            this.typeValueTitle.Margin = new System.Windows.Forms.Padding(0);
            this.typeValueTitle.Name = "typeValueTitle";
            this.typeValueTitle.Size = new System.Drawing.Size(52, 23);
            this.typeValueTitle.TabIndex = 3;
            this.typeValueTitle.Text = "Value：";
            this.typeValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // typeTitle
            // 
            this.typeTitle.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeTitle.Location = new System.Drawing.Point(8, 34);
            this.typeTitle.Name = "typeTitle";
            this.typeTitle.Size = new System.Drawing.Size(64, 21);
            this.typeTitle.TabIndex = 0;
            this.typeTitle.Text = "Type：";
            // 
            // typeIndex
            // 
            this.typeIndex.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeIndex.Location = new System.Drawing.Point(32, 61);
            this.typeIndex.Margin = new System.Windows.Forms.Padding(0);
            this.typeIndex.Name = "typeIndex";
            this.typeIndex.Size = new System.Drawing.Size(52, 21);
            this.typeIndex.TabIndex = 0;
            this.typeIndex.Text = "Index：";
            this.typeIndex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // typeIndexText
            // 
            this.typeIndexText.Location = new System.Drawing.Point(99, 61);
            this.typeIndexText.MaxLength = 16;
            this.typeIndexText.Name = "typeIndexText";
            this.typeIndexText.Size = new System.Drawing.Size(181, 23);
            this.typeIndexText.TabIndex = 2;
            // 
            // checkBtn
            // 
            this.checkBtn.BackColor = System.Drawing.SystemColors.Control;
            this.checkBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.checkBtn.Font = new System.Drawing.Font("Comic Sans MS", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBtn.Location = new System.Drawing.Point(15, 533);
            this.checkBtn.Name = "checkBtn";
            this.checkBtn.Size = new System.Drawing.Size(256, 42);
            this.checkBtn.TabIndex = 0;
            this.checkBtn.Text = "Check";
            this.checkBtn.UseVisualStyleBackColor = false;
            this.checkBtn.Click += new System.EventHandler(this.checkBtn_Click);
            // 
            // startBtn
            // 
            this.startBtn.BackColor = System.Drawing.SystemColors.Control;
            this.startBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.startBtn.Font = new System.Drawing.Font("Comic Sans MS", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startBtn.Location = new System.Drawing.Point(15, 587);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(256, 42);
            this.startBtn.TabIndex = 1;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = false;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // copyright
            // 
            this.copyright.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyright.Location = new System.Drawing.Point(12, 655);
            this.copyright.Name = "copyright";
            this.copyright.Size = new System.Drawing.Size(745, 21);
            this.copyright.TabIndex = 29;
            this.copyright.Text = "© Copyright 2018-现在      Rubble 版权所有";
            this.copyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // version
            // 
            this.version.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.version.Location = new System.Drawing.Point(764, 653);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(288, 21);
            this.version.TabIndex = 30;
            this.version.Text = "Version 1.0.0";
            this.version.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1064, 681);
            this.Controls.Add(this.version);
            this.Controls.Add(this.copyright);
            this.Controls.Add(this.mainGroupBox);
            this.Controls.Add(this.fileGroupBox);
            this.Controls.Add(this.optionsGroupBox);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CTools";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainGroupBox.ResumeLayout(false);
            this.mainGroupBox.PerformLayout();
            this.fileGroupBox.ResumeLayout(false);
            this.fileGroupBox.PerformLayout();
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
    }
}

