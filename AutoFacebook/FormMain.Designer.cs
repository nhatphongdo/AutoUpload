namespace AutoFacebook
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ButtonAddNewAccount = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CheckedListFacebookAccounts = new System.Windows.Forms.CheckedListBox();
            this.MenuContextAccounts = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuEditAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDeleteAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.TextSearchGroupPage = new System.Windows.Forms.TextBox();
            this.ButtonAddNewGroupPage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.CheckedListGroupPage = new System.Windows.Forms.CheckedListBox();
            this.MenuContextGroupsPages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuSelectAllDisplayGroupPage = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDeleteGroupPage = new System.Windows.Forms.ToolStripMenuItem();
            this.TextMessageLog = new System.Windows.Forms.TextBox();
            this.LabelInformation = new System.Windows.Forms.Label();
            this.LinkLabelAddImage = new System.Windows.Forms.LinkLabel();
            this.ListImages = new System.Windows.Forms.ListView();
            this.ImageListPosting = new System.Windows.Forms.ImageList(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.ButtonStartPosting = new System.Windows.Forms.Button();
            this.ButtonAddTime = new System.Windows.Forms.Button();
            this.ListSpecificTimes = new System.Windows.Forms.ListBox();
            this.TimePickerSpecificTime = new System.Windows.Forms.DateTimePicker();
            this.RadioSpecificTimes = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.NumericRepeatTotalPosts = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.NumericRepeatMinutes = new System.Windows.Forms.NumericUpDown();
            this.RadioRepeat = new System.Windows.Forms.RadioButton();
            this.TextContent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.label5 = new System.Windows.Forms.Label();
            this.TooltipController = new System.Windows.Forms.ToolTip(this.components);
            this.OpenFileDialogImages = new System.Windows.Forms.OpenFileDialog();
            this.TimerCounter = new System.Windows.Forms.Timer(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.TextKeyword = new System.Windows.Forms.TextBox();
            this.ButtonFindGroup = new System.Windows.Forms.Button();
            this.GridGroupResult = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.MenuContextAccounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.MenuContextGroupsPages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericRepeatTotalPosts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericRepeatMinutes)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridGroupResult)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1254, 939);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(8, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1238, 892);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Auto Post";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ButtonAddNewAccount);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.CheckedListFacebookAccounts);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1232, 886);
            this.splitContainer1.SplitterDistance = 370;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // ButtonAddNewAccount
            // 
            this.ButtonAddNewAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonAddNewAccount.Location = new System.Drawing.Point(2, 838);
            this.ButtonAddNewAccount.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonAddNewAccount.Name = "ButtonAddNewAccount";
            this.ButtonAddNewAccount.Size = new System.Drawing.Size(367, 48);
            this.ButtonAddNewAccount.TabIndex = 2;
            this.ButtonAddNewAccount.Text = "Add new account";
            this.ButtonAddNewAccount.UseVisualStyleBackColor = true;
            this.ButtonAddNewAccount.Click += new System.EventHandler(this.ButtonAddNewAccount_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Facebook account(s):";
            // 
            // CheckedListFacebookAccounts
            // 
            this.CheckedListFacebookAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckedListFacebookAccounts.CheckOnClick = true;
            this.CheckedListFacebookAccounts.ContextMenuStrip = this.MenuContextAccounts;
            this.CheckedListFacebookAccounts.FormattingEnabled = true;
            this.CheckedListFacebookAccounts.HorizontalScrollbar = true;
            this.CheckedListFacebookAccounts.IntegralHeight = false;
            this.CheckedListFacebookAccounts.Location = new System.Drawing.Point(2, 40);
            this.CheckedListFacebookAccounts.Margin = new System.Windows.Forms.Padding(2);
            this.CheckedListFacebookAccounts.Name = "CheckedListFacebookAccounts";
            this.CheckedListFacebookAccounts.Size = new System.Drawing.Size(367, 794);
            this.CheckedListFacebookAccounts.Sorted = true;
            this.CheckedListFacebookAccounts.TabIndex = 1;
            this.CheckedListFacebookAccounts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListFacebookAccounts_ItemCheck);
            this.CheckedListFacebookAccounts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CheckedListFacebookAccounts_MouseDown);
            // 
            // MenuContextAccounts
            // 
            this.MenuContextAccounts.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MenuContextAccounts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuEditAccount,
            this.MenuDeleteAccount});
            this.MenuContextAccounts.Name = "MenuContextAccounts";
            this.MenuContextAccounts.Size = new System.Drawing.Size(186, 80);
            // 
            // MenuEditAccount
            // 
            this.MenuEditAccount.Name = "MenuEditAccount";
            this.MenuEditAccount.Size = new System.Drawing.Size(185, 38);
            this.MenuEditAccount.Text = "Edit";
            this.MenuEditAccount.Click += new System.EventHandler(this.MenuEditAccount_Click);
            // 
            // MenuDeleteAccount
            // 
            this.MenuDeleteAccount.Name = "MenuDeleteAccount";
            this.MenuDeleteAccount.Size = new System.Drawing.Size(185, 38);
            this.MenuDeleteAccount.Text = "Delete";
            this.MenuDeleteAccount.Click += new System.EventHandler(this.MenuDeleteAccount_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.TextSearchGroupPage);
            this.splitContainer2.Panel1.Controls.Add(this.ButtonAddNewGroupPage);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.CheckedListGroupPage);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.TextMessageLog);
            this.splitContainer2.Panel2.Controls.Add(this.LabelInformation);
            this.splitContainer2.Panel2.Controls.Add(this.LinkLabelAddImage);
            this.splitContainer2.Panel2.Controls.Add(this.ListImages);
            this.splitContainer2.Panel2.Controls.Add(this.label8);
            this.splitContainer2.Panel2.Controls.Add(this.ButtonStartPosting);
            this.splitContainer2.Panel2.Controls.Add(this.ButtonAddTime);
            this.splitContainer2.Panel2.Controls.Add(this.ListSpecificTimes);
            this.splitContainer2.Panel2.Controls.Add(this.TimePickerSpecificTime);
            this.splitContainer2.Panel2.Controls.Add(this.RadioSpecificTimes);
            this.splitContainer2.Panel2.Controls.Add(this.label7);
            this.splitContainer2.Panel2.Controls.Add(this.NumericRepeatTotalPosts);
            this.splitContainer2.Panel2.Controls.Add(this.label6);
            this.splitContainer2.Panel2.Controls.Add(this.label4);
            this.splitContainer2.Panel2.Controls.Add(this.NumericRepeatMinutes);
            this.splitContainer2.Panel2.Controls.Add(this.RadioRepeat);
            this.splitContainer2.Panel2.Controls.Add(this.TextContent);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Size = new System.Drawing.Size(859, 886);
            this.splitContainer2.SplitterDistance = 422;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // TextSearchGroupPage
            // 
            this.TextSearchGroupPage.Location = new System.Drawing.Point(7, 40);
            this.TextSearchGroupPage.Name = "TextSearchGroupPage";
            this.TextSearchGroupPage.Size = new System.Drawing.Size(351, 31);
            this.TextSearchGroupPage.TabIndex = 4;
            this.TextSearchGroupPage.TextChanged += new System.EventHandler(this.TextSearchGroupPage_TextChanged);
            // 
            // ButtonAddNewGroupPage
            // 
            this.ButtonAddNewGroupPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonAddNewGroupPage.Location = new System.Drawing.Point(-1, 838);
            this.ButtonAddNewGroupPage.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonAddNewGroupPage.Name = "ButtonAddNewGroupPage";
            this.ButtonAddNewGroupPage.Size = new System.Drawing.Size(420, 48);
            this.ButtonAddNewGroupPage.TabIndex = 6;
            this.ButtonAddNewGroupPage.Text = "Add new group(s) / page(s)";
            this.ButtonAddNewGroupPage.UseVisualStyleBackColor = true;
            this.ButtonAddNewGroupPage.Click += new System.EventHandler(this.ButtonAddNewGroupPage_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select Group(s) / Page(s):";
            // 
            // CheckedListGroupPage
            // 
            this.CheckedListGroupPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckedListGroupPage.CheckOnClick = true;
            this.CheckedListGroupPage.ContextMenuStrip = this.MenuContextGroupsPages;
            this.CheckedListGroupPage.FormattingEnabled = true;
            this.CheckedListGroupPage.HorizontalScrollbar = true;
            this.CheckedListGroupPage.IntegralHeight = false;
            this.CheckedListGroupPage.Location = new System.Drawing.Point(2, 76);
            this.CheckedListGroupPage.Margin = new System.Windows.Forms.Padding(2);
            this.CheckedListGroupPage.Name = "CheckedListGroupPage";
            this.CheckedListGroupPage.Size = new System.Drawing.Size(418, 758);
            this.CheckedListGroupPage.Sorted = true;
            this.CheckedListGroupPage.TabIndex = 5;
            this.CheckedListGroupPage.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListGroupPage_ItemCheck);
            this.CheckedListGroupPage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CheckedListGroupPage_MouseDown);
            // 
            // MenuContextGroupsPages
            // 
            this.MenuContextGroupsPages.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MenuContextGroupsPages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuSelectAllDisplayGroupPage,
            this.MenuDeleteGroupPage});
            this.MenuContextGroupsPages.Name = "MenuContextGroupsPages";
            this.MenuContextGroupsPages.Size = new System.Drawing.Size(357, 80);
            // 
            // MenuSelectAllDisplayGroupPage
            // 
            this.MenuSelectAllDisplayGroupPage.Name = "MenuSelectAllDisplayGroupPage";
            this.MenuSelectAllDisplayGroupPage.Size = new System.Drawing.Size(356, 38);
            this.MenuSelectAllDisplayGroupPage.Text = "Select all display items";
            this.MenuSelectAllDisplayGroupPage.Click += new System.EventHandler(this.MenuSelectAllDisplayGroupPage_Click);
            // 
            // MenuDeleteGroupPage
            // 
            this.MenuDeleteGroupPage.Name = "MenuDeleteGroupPage";
            this.MenuDeleteGroupPage.Size = new System.Drawing.Size(356, 38);
            this.MenuDeleteGroupPage.Text = "Delete";
            this.MenuDeleteGroupPage.Click += new System.EventHandler(this.MenuDeleteGroupPage_Click);
            // 
            // TextMessageLog
            // 
            this.TextMessageLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextMessageLog.Location = new System.Drawing.Point(3, 707);
            this.TextMessageLog.Multiline = true;
            this.TextMessageLog.Name = "TextMessageLog";
            this.TextMessageLog.ReadOnly = true;
            this.TextMessageLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextMessageLog.Size = new System.Drawing.Size(429, 179);
            this.TextMessageLog.TabIndex = 17;
            // 
            // LabelInformation
            // 
            this.LabelInformation.AutoSize = true;
            this.LabelInformation.Location = new System.Drawing.Point(8, 671);
            this.LabelInformation.Name = "LabelInformation";
            this.LabelInformation.Size = new System.Drawing.Size(0, 25);
            this.LabelInformation.TabIndex = 16;
            // 
            // LinkLabelAddImage
            // 
            this.LinkLabelAddImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LinkLabelAddImage.AutoSize = true;
            this.LinkLabelAddImage.Location = new System.Drawing.Point(323, 216);
            this.LinkLabelAddImage.Name = "LinkLabelAddImage";
            this.LinkLabelAddImage.Size = new System.Drawing.Size(114, 25);
            this.LinkLabelAddImage.TabIndex = 15;
            this.LinkLabelAddImage.TabStop = true;
            this.LinkLabelAddImage.Text = "Add image";
            this.LinkLabelAddImage.Click += new System.EventHandler(this.LinkLabelAddImage_Click);
            // 
            // ListImages
            // 
            this.ListImages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListImages.LargeImageList = this.ImageListPosting;
            this.ListImages.Location = new System.Drawing.Point(8, 244);
            this.ListImages.Name = "ListImages";
            this.ListImages.Size = new System.Drawing.Size(426, 97);
            this.ListImages.TabIndex = 14;
            this.ListImages.UseCompatibleStateImageBehavior = false;
            // 
            // ImageListPosting
            // 
            this.ImageListPosting.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ImageListPosting.ImageSize = new System.Drawing.Size(64, 64);
            this.ImageListPosting.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 216);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(158, 25);
            this.label8.TabIndex = 13;
            this.label8.Text = "Images to post:";
            // 
            // ButtonStartPosting
            // 
            this.ButtonStartPosting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonStartPosting.Location = new System.Drawing.Point(3, 653);
            this.ButtonStartPosting.Name = "ButtonStartPosting";
            this.ButtonStartPosting.Size = new System.Drawing.Size(431, 48);
            this.ButtonStartPosting.TabIndex = 12;
            this.ButtonStartPosting.Text = "Start posting";
            this.ButtonStartPosting.UseVisualStyleBackColor = true;
            this.ButtonStartPosting.Click += new System.EventHandler(this.ButtonStartPosting_Click);
            // 
            // ButtonAddTime
            // 
            this.ButtonAddTime.Enabled = false;
            this.ButtonAddTime.Location = new System.Drawing.Point(254, 502);
            this.ButtonAddTime.Name = "ButtonAddTime";
            this.ButtonAddTime.Size = new System.Drawing.Size(104, 31);
            this.ButtonAddTime.TabIndex = 11;
            this.ButtonAddTime.Text = "Add";
            this.ButtonAddTime.UseVisualStyleBackColor = true;
            this.ButtonAddTime.Click += new System.EventHandler(this.ButtonAddTime_Click);
            // 
            // ListSpecificTimes
            // 
            this.ListSpecificTimes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListSpecificTimes.Enabled = false;
            this.ListSpecificTimes.FormattingEnabled = true;
            this.ListSpecificTimes.ItemHeight = 25;
            this.ListSpecificTimes.Location = new System.Drawing.Point(47, 538);
            this.ListSpecificTimes.Name = "ListSpecificTimes";
            this.ListSpecificTimes.Size = new System.Drawing.Size(387, 104);
            this.ListSpecificTimes.Sorted = true;
            this.ListSpecificTimes.TabIndex = 10;
            // 
            // TimePickerSpecificTime
            // 
            this.TimePickerSpecificTime.Enabled = false;
            this.TimePickerSpecificTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.TimePickerSpecificTime.Location = new System.Drawing.Point(47, 502);
            this.TimePickerSpecificTime.Name = "TimePickerSpecificTime";
            this.TimePickerSpecificTime.ShowUpDown = true;
            this.TimePickerSpecificTime.Size = new System.Drawing.Size(200, 31);
            this.TimePickerSpecificTime.TabIndex = 9;
            // 
            // RadioSpecificTimes
            // 
            this.RadioSpecificTimes.AutoSize = true;
            this.RadioSpecificTimes.Location = new System.Drawing.Point(8, 467);
            this.RadioSpecificTimes.Name = "RadioSpecificTimes";
            this.RadioSpecificTimes.Size = new System.Drawing.Size(199, 29);
            this.RadioSpecificTimes.TabIndex = 8;
            this.RadioSpecificTimes.Text = "At specific times";
            this.TooltipController.SetToolTip(this.RadioSpecificTimes, "Will post at the specific defined times.\r\nPlease keep application running if this" +
        " option is enabled.");
            this.RadioSpecificTimes.UseVisualStyleBackColor = true;
            this.RadioSpecificTimes.CheckedChanged += new System.EventHandler(this.RadioSpecificTimes_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Enabled = false;
            this.label7.Location = new System.Drawing.Point(280, 420);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 25);
            this.label7.TabIndex = 7;
            this.label7.Text = "post(s)";
            // 
            // NumericRepeatTotalPosts
            // 
            this.NumericRepeatTotalPosts.Enabled = false;
            this.NumericRepeatTotalPosts.Location = new System.Drawing.Point(178, 418);
            this.NumericRepeatTotalPosts.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericRepeatTotalPosts.Name = "NumericRepeatTotalPosts";
            this.NumericRepeatTotalPosts.Size = new System.Drawing.Size(96, 31);
            this.NumericRepeatTotalPosts.TabIndex = 6;
            this.NumericRepeatTotalPosts.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(42, 420);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 25);
            this.label6.TabIndex = 5;
            this.label6.Text = "until enough";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(147, 384);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "minute(s)";
            // 
            // NumericRepeatMinutes
            // 
            this.NumericRepeatMinutes.Enabled = false;
            this.NumericRepeatMinutes.Location = new System.Drawing.Point(47, 382);
            this.NumericRepeatMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericRepeatMinutes.Name = "NumericRepeatMinutes";
            this.NumericRepeatMinutes.Size = new System.Drawing.Size(94, 31);
            this.NumericRepeatMinutes.TabIndex = 3;
            this.NumericRepeatMinutes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // RadioRepeat
            // 
            this.RadioRepeat.AutoSize = true;
            this.RadioRepeat.Location = new System.Drawing.Point(8, 347);
            this.RadioRepeat.Name = "RadioRepeat";
            this.RadioRepeat.Size = new System.Drawing.Size(247, 29);
            this.RadioRepeat.TabIndex = 2;
            this.RadioRepeat.Text = "Repeat posting every";
            this.TooltipController.SetToolTip(this.RadioRepeat, "Will post after a specific minutes, limit to maximum posts per group or page for " +
        "each account");
            this.RadioRepeat.UseVisualStyleBackColor = true;
            this.RadioRepeat.CheckedChanged += new System.EventHandler(this.RadioRepeat_CheckedChanged);
            // 
            // TextContent
            // 
            this.TextContent.AcceptsReturn = true;
            this.TextContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextContent.Location = new System.Drawing.Point(8, 40);
            this.TextContent.Multiline = true;
            this.TextContent.Name = "TextContent";
            this.TextContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextContent.Size = new System.Drawing.Size(426, 173);
            this.TextContent.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Content to post:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.GridGroupResult);
            this.tabPage2.Controls.Add(this.ButtonFindGroup);
            this.tabPage2.Controls.Add(this.TextKeyword);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Location = new System.Drawing.Point(8, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1238, 892);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Group(s) Search";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.splitContainer3);
            this.tabPage4.Location = new System.Drawing.Point(8, 39);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1238, 892);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Settings";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.label5);
            this.splitContainer3.Size = new System.Drawing.Size(1238, 892);
            this.splitContainer3.SplitterDistance = 411;
            this.splitContainer3.SplitterWidth = 3;
            this.splitContainer3.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(286, 25);
            this.label5.TabIndex = 3;
            this.label5.Text = "Select Facebook account(s):";
            // 
            // OpenFileDialogImages
            // 
            this.OpenFileDialogImages.Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp";
            this.OpenFileDialogImages.Multiselect = true;
            this.OpenFileDialogImages.SupportMultiDottedExtensions = true;
            // 
            // TimerCounter
            // 
            this.TimerCounter.Interval = 60000;
            this.TimerCounter.Tick += new System.EventHandler(this.TimerCounter_Tick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 25);
            this.label9.TabIndex = 0;
            this.label9.Text = "Keyword:";
            // 
            // TextKeyword
            // 
            this.TextKeyword.Location = new System.Drawing.Point(115, 13);
            this.TextKeyword.Name = "TextKeyword";
            this.TextKeyword.Size = new System.Drawing.Size(812, 31);
            this.TextKeyword.TabIndex = 1;
            // 
            // ButtonFindGroup
            // 
            this.ButtonFindGroup.Location = new System.Drawing.Point(933, 6);
            this.ButtonFindGroup.Name = "ButtonFindGroup";
            this.ButtonFindGroup.Size = new System.Drawing.Size(105, 45);
            this.ButtonFindGroup.TabIndex = 2;
            this.ButtonFindGroup.Text = "Find";
            this.ButtonFindGroup.UseVisualStyleBackColor = true;
            // 
            // GridGroupResult
            // 
            this.GridGroupResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridGroupResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridGroupResult.Location = new System.Drawing.Point(13, 57);
            this.GridGroupResult.Name = "GridGroupResult";
            this.GridGroupResult.RowTemplate.Height = 33;
            this.GridGroupResult.Size = new System.Drawing.Size(1209, 823);
            this.GridGroupResult.TabIndex = 3;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1254, 939);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Tools";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.MenuContextAccounts.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.MenuContextGroupsPages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumericRepeatTotalPosts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericRepeatMinutes)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridGroupResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox CheckedListFacebookAccounts;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox CheckedListGroupPage;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button ButtonAddNewAccount;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ButtonAddNewGroupPage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip MenuContextAccounts;
        private System.Windows.Forms.ToolStripMenuItem MenuDeleteAccount;
        private System.Windows.Forms.ToolStripMenuItem MenuEditAccount;
        private System.Windows.Forms.TextBox TextSearchGroupPage;
        private System.Windows.Forms.TextBox TextContent;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown NumericRepeatTotalPosts;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown NumericRepeatMinutes;
        private System.Windows.Forms.RadioButton RadioRepeat;
        private System.Windows.Forms.RadioButton RadioSpecificTimes;
        private System.Windows.Forms.Button ButtonAddTime;
        private System.Windows.Forms.ListBox ListSpecificTimes;
        private System.Windows.Forms.DateTimePicker TimePickerSpecificTime;
        private System.Windows.Forms.Button ButtonStartPosting;
        private System.Windows.Forms.ToolTip TooltipController;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListView ListImages;
        private System.Windows.Forms.ImageList ImageListPosting;
        private System.Windows.Forms.LinkLabel LinkLabelAddImage;
        private System.Windows.Forms.OpenFileDialog OpenFileDialogImages;
        private System.Windows.Forms.ContextMenuStrip MenuContextGroupsPages;
        private System.Windows.Forms.ToolStripMenuItem MenuDeleteGroupPage;
        private System.Windows.Forms.Label LabelInformation;
        private System.Windows.Forms.ToolStripMenuItem MenuSelectAllDisplayGroupPage;
        private System.Windows.Forms.TextBox TextMessageLog;
        private System.Windows.Forms.Timer TimerCounter;
        private System.Windows.Forms.DataGridView GridGroupResult;
        private System.Windows.Forms.Button ButtonFindGroup;
        private System.Windows.Forms.TextBox TextKeyword;
        private System.Windows.Forms.Label label9;
    }
}