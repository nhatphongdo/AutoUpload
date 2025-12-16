namespace AutoUpload.Manager
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
            this.ButtonChecksum = new System.Windows.Forms.Button();
            this.TextBoxChecksum = new System.Windows.Forms.TextBox();
            this.OpenFileDialogChecksum = new System.Windows.Forms.OpenFileDialog();
            this.TextBoxPsdSource = new System.Windows.Forms.TextBox();
            this.ButtonSelectPsdSource = new System.Windows.Forms.Button();
            this.ButtonCreateMockup = new System.Windows.Forms.Button();
            this.BackgroundWorkerCreateMockup = new System.ComponentModel.BackgroundWorker();
            this.ProgressBarCreateMockup = new System.Windows.Forms.ProgressBar();
            this.ButtonTestMockups = new System.Windows.Forms.Button();
            this.OpenFileDialogPhotoshop = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NumericUpDownDesignLayer = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownShirtColorLayer = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownShirtTextureLayer = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.NumericUpDownLaceColorLayer = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.NumericUpDownDesignX = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.NumericUpDownDesignY = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownDesignWidth = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownDesignHeight = new System.Windows.Forms.NumericUpDown();
            this.ButtonLoadPsd = new System.Windows.Forms.Button();
            this.RichTextBoxPsdLayers = new System.Windows.Forms.RichTextBox();
            this.ColorDialogDesign = new System.Windows.Forms.ColorDialog();
            this.ButtonShirtColor = new System.Windows.Forms.Button();
            this.ButtonLaceColor = new System.Windows.Forms.Button();
            this.ButtonDesignBackground = new System.Windows.Forms.Button();
            this.ButtonLoadTipsHeader = new System.Windows.Forms.Button();
            this.ButtonAesKeys = new System.Windows.Forms.Button();
            this.TextBoxAesKey = new System.Windows.Forms.TextBox();
            this.TextBoxAesInitialVector = new System.Windows.Forms.TextBox();
            this.ButtonCreateThumbnail = new System.Windows.Forms.Button();
            this.ButtonFillDesign = new System.Windows.Forms.Button();
            this.ButtonTestAll = new System.Windows.Forms.Button();
            this.ButtonTestFillAll = new System.Windows.Forms.Button();
            this.BackgroundWorkerTestMockups = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownShirtColorLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownShirtTextureLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownLaceColorLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonChecksum
            // 
            this.ButtonChecksum.Location = new System.Drawing.Point(6, 7);
            this.ButtonChecksum.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonChecksum.Name = "ButtonChecksum";
            this.ButtonChecksum.Size = new System.Drawing.Size(84, 24);
            this.ButtonChecksum.TabIndex = 0;
            this.ButtonChecksum.Text = "Checksum";
            this.ButtonChecksum.UseVisualStyleBackColor = true;
            this.ButtonChecksum.Click += new System.EventHandler(this.ButtonChecksum_Click);
            // 
            // TextBoxChecksum
            // 
            this.TextBoxChecksum.Location = new System.Drawing.Point(94, 11);
            this.TextBoxChecksum.Margin = new System.Windows.Forms.Padding(2);
            this.TextBoxChecksum.Name = "TextBoxChecksum";
            this.TextBoxChecksum.ReadOnly = true;
            this.TextBoxChecksum.Size = new System.Drawing.Size(402, 20);
            this.TextBoxChecksum.TabIndex = 1;
            // 
            // TextBoxPsdSource
            // 
            this.TextBoxPsdSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxPsdSource.Location = new System.Drawing.Point(110, 98);
            this.TextBoxPsdSource.Margin = new System.Windows.Forms.Padding(2);
            this.TextBoxPsdSource.Name = "TextBoxPsdSource";
            this.TextBoxPsdSource.Size = new System.Drawing.Size(524, 20);
            this.TextBoxPsdSource.TabIndex = 3;
            // 
            // ButtonSelectPsdSource
            // 
            this.ButtonSelectPsdSource.Location = new System.Drawing.Point(6, 95);
            this.ButtonSelectPsdSource.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonSelectPsdSource.Name = "ButtonSelectPsdSource";
            this.ButtonSelectPsdSource.Size = new System.Drawing.Size(100, 24);
            this.ButtonSelectPsdSource.TabIndex = 2;
            this.ButtonSelectPsdSource.Text = "Select source";
            this.ButtonSelectPsdSource.UseVisualStyleBackColor = true;
            this.ButtonSelectPsdSource.Click += new System.EventHandler(this.ButtonSelectPsdSource_Click);
            // 
            // ButtonCreateMockup
            // 
            this.ButtonCreateMockup.Location = new System.Drawing.Point(82, 317);
            this.ButtonCreateMockup.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonCreateMockup.Name = "ButtonCreateMockup";
            this.ButtonCreateMockup.Size = new System.Drawing.Size(71, 46);
            this.ButtonCreateMockup.TabIndex = 6;
            this.ButtonCreateMockup.Text = "Create Mockups";
            this.ButtonCreateMockup.UseVisualStyleBackColor = true;
            this.ButtonCreateMockup.Click += new System.EventHandler(this.ButtonCreateMockup_Click);
            // 
            // BackgroundWorkerCreateMockup
            // 
            this.BackgroundWorkerCreateMockup.WorkerReportsProgress = true;
            this.BackgroundWorkerCreateMockup.WorkerSupportsCancellation = true;
            this.BackgroundWorkerCreateMockup.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerCreateMockup_DoWork);
            this.BackgroundWorkerCreateMockup.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorkerCreateMockup_ProgressChanged);
            this.BackgroundWorkerCreateMockup.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerCreateMockup_RunWorkerCompleted);
            // 
            // ProgressBarCreateMockup
            // 
            this.ProgressBarCreateMockup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBarCreateMockup.Location = new System.Drawing.Point(8, 371);
            this.ProgressBarCreateMockup.Margin = new System.Windows.Forms.Padding(2);
            this.ProgressBarCreateMockup.Name = "ProgressBarCreateMockup";
            this.ProgressBarCreateMockup.Size = new System.Drawing.Size(624, 22);
            this.ProgressBarCreateMockup.TabIndex = 7;
            // 
            // ButtonTestMockups
            // 
            this.ButtonTestMockups.Location = new System.Drawing.Point(8, 404);
            this.ButtonTestMockups.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonTestMockups.Name = "ButtonTestMockups";
            this.ButtonTestMockups.Size = new System.Drawing.Size(73, 46);
            this.ButtonTestMockups.TabIndex = 9;
            this.ButtonTestMockups.Text = "Test Mockups";
            this.ButtonTestMockups.UseVisualStyleBackColor = true;
            this.ButtonTestMockups.Click += new System.EventHandler(this.ButtonTestMockups_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 126);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Design layer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 150);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Shirt color layer";
            // 
            // NumericUpDownDesignLayer
            // 
            this.NumericUpDownDesignLayer.Location = new System.Drawing.Point(110, 125);
            this.NumericUpDownDesignLayer.Margin = new System.Windows.Forms.Padding(2);
            this.NumericUpDownDesignLayer.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.NumericUpDownDesignLayer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumericUpDownDesignLayer.Name = "NumericUpDownDesignLayer";
            this.NumericUpDownDesignLayer.Size = new System.Drawing.Size(118, 20);
            this.NumericUpDownDesignLayer.TabIndex = 13;
            this.NumericUpDownDesignLayer.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // NumericUpDownShirtColorLayer
            // 
            this.NumericUpDownShirtColorLayer.Location = new System.Drawing.Point(110, 149);
            this.NumericUpDownShirtColorLayer.Margin = new System.Windows.Forms.Padding(2);
            this.NumericUpDownShirtColorLayer.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.NumericUpDownShirtColorLayer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumericUpDownShirtColorLayer.Name = "NumericUpDownShirtColorLayer";
            this.NumericUpDownShirtColorLayer.Size = new System.Drawing.Size(118, 20);
            this.NumericUpDownShirtColorLayer.TabIndex = 14;
            this.NumericUpDownShirtColorLayer.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // NumericUpDownShirtTextureLayer
            // 
            this.NumericUpDownShirtTextureLayer.Location = new System.Drawing.Point(110, 173);
            this.NumericUpDownShirtTextureLayer.Margin = new System.Windows.Forms.Padding(2);
            this.NumericUpDownShirtTextureLayer.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.NumericUpDownShirtTextureLayer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumericUpDownShirtTextureLayer.Name = "NumericUpDownShirtTextureLayer";
            this.NumericUpDownShirtTextureLayer.Size = new System.Drawing.Size(118, 20);
            this.NumericUpDownShirtTextureLayer.TabIndex = 16;
            this.NumericUpDownShirtTextureLayer.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 174);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Shirt texture layer";
            // 
            // NumericUpDownLaceColorLayer
            // 
            this.NumericUpDownLaceColorLayer.Location = new System.Drawing.Point(110, 195);
            this.NumericUpDownLaceColorLayer.Margin = new System.Windows.Forms.Padding(2);
            this.NumericUpDownLaceColorLayer.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.NumericUpDownLaceColorLayer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumericUpDownLaceColorLayer.Name = "NumericUpDownLaceColorLayer";
            this.NumericUpDownLaceColorLayer.Size = new System.Drawing.Size(118, 20);
            this.NumericUpDownLaceColorLayer.TabIndex = 18;
            this.NumericUpDownLaceColorLayer.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 196);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Lace color layer";
            // 
            // NumericUpDownDesignX
            // 
            this.NumericUpDownDesignX.Location = new System.Drawing.Point(110, 219);
            this.NumericUpDownDesignX.Margin = new System.Windows.Forms.Padding(2);
            this.NumericUpDownDesignX.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.NumericUpDownDesignX.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.NumericUpDownDesignX.Name = "NumericUpDownDesignX";
            this.NumericUpDownDesignX.Size = new System.Drawing.Size(118, 20);
            this.NumericUpDownDesignX.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 220);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Design boundary";
            // 
            // NumericUpDownDesignY
            // 
            this.NumericUpDownDesignY.Location = new System.Drawing.Point(110, 241);
            this.NumericUpDownDesignY.Margin = new System.Windows.Forms.Padding(2);
            this.NumericUpDownDesignY.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.NumericUpDownDesignY.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.NumericUpDownDesignY.Name = "NumericUpDownDesignY";
            this.NumericUpDownDesignY.Size = new System.Drawing.Size(118, 20);
            this.NumericUpDownDesignY.TabIndex = 21;
            // 
            // NumericUpDownDesignWidth
            // 
            this.NumericUpDownDesignWidth.Location = new System.Drawing.Point(110, 264);
            this.NumericUpDownDesignWidth.Margin = new System.Windows.Forms.Padding(2);
            this.NumericUpDownDesignWidth.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.NumericUpDownDesignWidth.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.NumericUpDownDesignWidth.Name = "NumericUpDownDesignWidth";
            this.NumericUpDownDesignWidth.Size = new System.Drawing.Size(118, 20);
            this.NumericUpDownDesignWidth.TabIndex = 22;
            // 
            // NumericUpDownDesignHeight
            // 
            this.NumericUpDownDesignHeight.Location = new System.Drawing.Point(110, 287);
            this.NumericUpDownDesignHeight.Margin = new System.Windows.Forms.Padding(2);
            this.NumericUpDownDesignHeight.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.NumericUpDownDesignHeight.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.NumericUpDownDesignHeight.Name = "NumericUpDownDesignHeight";
            this.NumericUpDownDesignHeight.Size = new System.Drawing.Size(118, 20);
            this.NumericUpDownDesignHeight.TabIndex = 23;
            // 
            // ButtonLoadPsd
            // 
            this.ButtonLoadPsd.Location = new System.Drawing.Point(8, 317);
            this.ButtonLoadPsd.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonLoadPsd.Name = "ButtonLoadPsd";
            this.ButtonLoadPsd.Size = new System.Drawing.Size(71, 46);
            this.ButtonLoadPsd.TabIndex = 24;
            this.ButtonLoadPsd.Text = "Load PSD";
            this.ButtonLoadPsd.UseVisualStyleBackColor = true;
            this.ButtonLoadPsd.Click += new System.EventHandler(this.ButtonLoadPsd_Click);
            // 
            // RichTextBoxPsdLayers
            // 
            this.RichTextBoxPsdLayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBoxPsdLayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichTextBoxPsdLayers.Location = new System.Drawing.Point(237, 125);
            this.RichTextBoxPsdLayers.Margin = new System.Windows.Forms.Padding(2);
            this.RichTextBoxPsdLayers.Name = "RichTextBoxPsdLayers";
            this.RichTextBoxPsdLayers.Size = new System.Drawing.Size(397, 240);
            this.RichTextBoxPsdLayers.TabIndex = 25;
            this.RichTextBoxPsdLayers.Text = "";
            // 
            // ColorDialogDesign
            // 
            this.ColorDialogDesign.AnyColor = true;
            this.ColorDialogDesign.FullOpen = true;
            // 
            // ButtonShirtColor
            // 
            this.ButtonShirtColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonShirtColor.BackColor = System.Drawing.Color.Black;
            this.ButtonShirtColor.ForeColor = System.Drawing.Color.White;
            this.ButtonShirtColor.Location = new System.Drawing.Point(491, 404);
            this.ButtonShirtColor.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonShirtColor.Name = "ButtonShirtColor";
            this.ButtonShirtColor.Size = new System.Drawing.Size(69, 46);
            this.ButtonShirtColor.TabIndex = 26;
            this.ButtonShirtColor.Text = "Shirt Color";
            this.ButtonShirtColor.UseVisualStyleBackColor = false;
            this.ButtonShirtColor.Click += new System.EventHandler(this.ButtonShirtColor_Click);
            // 
            // ButtonLaceColor
            // 
            this.ButtonLaceColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonLaceColor.BackColor = System.Drawing.Color.Black;
            this.ButtonLaceColor.ForeColor = System.Drawing.Color.White;
            this.ButtonLaceColor.Location = new System.Drawing.Point(563, 404);
            this.ButtonLaceColor.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonLaceColor.Name = "ButtonLaceColor";
            this.ButtonLaceColor.Size = new System.Drawing.Size(69, 46);
            this.ButtonLaceColor.TabIndex = 27;
            this.ButtonLaceColor.Text = "Lace Color";
            this.ButtonLaceColor.UseVisualStyleBackColor = false;
            this.ButtonLaceColor.Click += new System.EventHandler(this.ButtonLaceColor_Click);
            // 
            // ButtonDesignBackground
            // 
            this.ButtonDesignBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonDesignBackground.BackColor = System.Drawing.Color.Transparent;
            this.ButtonDesignBackground.ForeColor = System.Drawing.Color.White;
            this.ButtonDesignBackground.Location = new System.Drawing.Point(419, 404);
            this.ButtonDesignBackground.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonDesignBackground.Name = "ButtonDesignBackground";
            this.ButtonDesignBackground.Size = new System.Drawing.Size(69, 46);
            this.ButtonDesignBackground.TabIndex = 28;
            this.ButtonDesignBackground.Text = "Design Background";
            this.ButtonDesignBackground.UseVisualStyleBackColor = false;
            this.ButtonDesignBackground.Click += new System.EventHandler(this.ButtonDesignBackground_Click);
            // 
            // ButtonLoadTipsHeader
            // 
            this.ButtonLoadTipsHeader.Location = new System.Drawing.Point(156, 317);
            this.ButtonLoadTipsHeader.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonLoadTipsHeader.Name = "ButtonLoadTipsHeader";
            this.ButtonLoadTipsHeader.Size = new System.Drawing.Size(71, 46);
            this.ButtonLoadTipsHeader.TabIndex = 29;
            this.ButtonLoadTipsHeader.Text = "Load TIPS header";
            this.ButtonLoadTipsHeader.UseVisualStyleBackColor = true;
            this.ButtonLoadTipsHeader.Click += new System.EventHandler(this.ButtonLoadTipsHeader_Click);
            // 
            // ButtonAesKeys
            // 
            this.ButtonAesKeys.Location = new System.Drawing.Point(6, 41);
            this.ButtonAesKeys.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonAesKeys.Name = "ButtonAesKeys";
            this.ButtonAesKeys.Size = new System.Drawing.Size(84, 24);
            this.ButtonAesKeys.TabIndex = 30;
            this.ButtonAesKeys.Text = "AES Key";
            this.ButtonAesKeys.UseVisualStyleBackColor = true;
            this.ButtonAesKeys.Click += new System.EventHandler(this.ButtonAesKeys_Click);
            // 
            // TextBoxAesKey
            // 
            this.TextBoxAesKey.Location = new System.Drawing.Point(94, 41);
            this.TextBoxAesKey.Margin = new System.Windows.Forms.Padding(2);
            this.TextBoxAesKey.Name = "TextBoxAesKey";
            this.TextBoxAesKey.ReadOnly = true;
            this.TextBoxAesKey.Size = new System.Drawing.Size(402, 20);
            this.TextBoxAesKey.TabIndex = 31;
            // 
            // TextBoxAesInitialVector
            // 
            this.TextBoxAesInitialVector.Location = new System.Drawing.Point(94, 60);
            this.TextBoxAesInitialVector.Margin = new System.Windows.Forms.Padding(2);
            this.TextBoxAesInitialVector.Name = "TextBoxAesInitialVector";
            this.TextBoxAesInitialVector.ReadOnly = true;
            this.TextBoxAesInitialVector.Size = new System.Drawing.Size(402, 20);
            this.TextBoxAesInitialVector.TabIndex = 32;
            // 
            // ButtonCreateThumbnail
            // 
            this.ButtonCreateThumbnail.Location = new System.Drawing.Point(85, 404);
            this.ButtonCreateThumbnail.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonCreateThumbnail.Name = "ButtonCreateThumbnail";
            this.ButtonCreateThumbnail.Size = new System.Drawing.Size(73, 46);
            this.ButtonCreateThumbnail.TabIndex = 33;
            this.ButtonCreateThumbnail.Text = "Create thumbnail";
            this.ButtonCreateThumbnail.UseVisualStyleBackColor = true;
            this.ButtonCreateThumbnail.Click += new System.EventHandler(this.ButtonCreateThumbnail_Click);
            // 
            // ButtonFillDesign
            // 
            this.ButtonFillDesign.Location = new System.Drawing.Point(162, 404);
            this.ButtonFillDesign.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonFillDesign.Name = "ButtonFillDesign";
            this.ButtonFillDesign.Size = new System.Drawing.Size(73, 46);
            this.ButtonFillDesign.TabIndex = 34;
            this.ButtonFillDesign.Text = "Fill design";
            this.ButtonFillDesign.UseVisualStyleBackColor = true;
            this.ButtonFillDesign.Click += new System.EventHandler(this.ButtonFillDesign_Click);
            // 
            // ButtonTestAll
            // 
            this.ButtonTestAll.Location = new System.Drawing.Point(9, 454);
            this.ButtonTestAll.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonTestAll.Name = "ButtonTestAll";
            this.ButtonTestAll.Size = new System.Drawing.Size(73, 46);
            this.ButtonTestAll.TabIndex = 35;
            this.ButtonTestAll.Text = "Test All";
            this.ButtonTestAll.UseVisualStyleBackColor = true;
            this.ButtonTestAll.Click += new System.EventHandler(this.ButtonTestAll_Click);
            // 
            // ButtonTestFillAll
            // 
            this.ButtonTestFillAll.Location = new System.Drawing.Point(85, 454);
            this.ButtonTestFillAll.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonTestFillAll.Name = "ButtonTestFillAll";
            this.ButtonTestFillAll.Size = new System.Drawing.Size(73, 46);
            this.ButtonTestFillAll.TabIndex = 36;
            this.ButtonTestFillAll.Text = "Test Fill All";
            this.ButtonTestFillAll.UseVisualStyleBackColor = true;
            this.ButtonTestFillAll.Click += new System.EventHandler(this.ButtonTestFillAll_Click);
            // 
            // BackgroundWorkerTestMockups
            // 
            this.BackgroundWorkerTestMockups.WorkerReportsProgress = true;
            this.BackgroundWorkerTestMockups.WorkerSupportsCancellation = true;
            this.BackgroundWorkerTestMockups.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerTestMockups_DoWork);
            this.BackgroundWorkerTestMockups.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorkerTestMockups_ProgressChanged);
            this.BackgroundWorkerTestMockups.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerTestMockups_RunWorkerCompleted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(644, 509);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 37;
            this.pictureBox1.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 509);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ButtonTestFillAll);
            this.Controls.Add(this.ButtonTestAll);
            this.Controls.Add(this.ButtonFillDesign);
            this.Controls.Add(this.ButtonCreateThumbnail);
            this.Controls.Add(this.TextBoxAesInitialVector);
            this.Controls.Add(this.TextBoxAesKey);
            this.Controls.Add(this.ButtonAesKeys);
            this.Controls.Add(this.ButtonLoadTipsHeader);
            this.Controls.Add(this.ButtonDesignBackground);
            this.Controls.Add(this.ButtonLaceColor);
            this.Controls.Add(this.ButtonShirtColor);
            this.Controls.Add(this.RichTextBoxPsdLayers);
            this.Controls.Add(this.ButtonLoadPsd);
            this.Controls.Add(this.NumericUpDownDesignHeight);
            this.Controls.Add(this.NumericUpDownDesignWidth);
            this.Controls.Add(this.NumericUpDownDesignY);
            this.Controls.Add(this.NumericUpDownDesignX);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.NumericUpDownLaceColorLayer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NumericUpDownShirtTextureLayer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NumericUpDownShirtColorLayer);
            this.Controls.Add(this.NumericUpDownDesignLayer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonTestMockups);
            this.Controls.Add(this.ProgressBarCreateMockup);
            this.Controls.Add(this.ButtonCreateMockup);
            this.Controls.Add(this.TextBoxPsdSource);
            this.Controls.Add(this.ButtonSelectPsdSource);
            this.Controls.Add(this.TextBoxChecksum);
            this.Controls.Add(this.ButtonChecksum);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownShirtColorLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownShirtTextureLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownLaceColorLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDesignHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonChecksum;
        private System.Windows.Forms.TextBox TextBoxChecksum;
        private System.Windows.Forms.OpenFileDialog OpenFileDialogChecksum;
        private System.Windows.Forms.TextBox TextBoxPsdSource;
        private System.Windows.Forms.Button ButtonSelectPsdSource;
        private System.Windows.Forms.Button ButtonCreateMockup;
        private System.ComponentModel.BackgroundWorker BackgroundWorkerCreateMockup;
        private System.Windows.Forms.ProgressBar ProgressBarCreateMockup;
        private System.Windows.Forms.Button ButtonTestMockups;
        private System.Windows.Forms.OpenFileDialog OpenFileDialogPhotoshop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NumericUpDownDesignLayer;
        private System.Windows.Forms.NumericUpDown NumericUpDownShirtColorLayer;
        private System.Windows.Forms.NumericUpDown NumericUpDownShirtTextureLayer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumericUpDownLaceColorLayer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown NumericUpDownDesignX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NumericUpDownDesignY;
        private System.Windows.Forms.NumericUpDown NumericUpDownDesignWidth;
        private System.Windows.Forms.NumericUpDown NumericUpDownDesignHeight;
        private System.Windows.Forms.Button ButtonLoadPsd;
        private System.Windows.Forms.RichTextBox RichTextBoxPsdLayers;
        private System.Windows.Forms.ColorDialog ColorDialogDesign;
        private System.Windows.Forms.Button ButtonShirtColor;
        private System.Windows.Forms.Button ButtonLaceColor;
        private System.Windows.Forms.Button ButtonDesignBackground;
        private System.Windows.Forms.Button ButtonLoadTipsHeader;
        private System.Windows.Forms.Button ButtonAesKeys;
        private System.Windows.Forms.TextBox TextBoxAesKey;
        private System.Windows.Forms.TextBox TextBoxAesInitialVector;
        private System.Windows.Forms.Button ButtonCreateThumbnail;
        private System.Windows.Forms.Button ButtonFillDesign;
        private System.Windows.Forms.Button ButtonTestAll;
        private System.Windows.Forms.Button ButtonTestFillAll;
        private System.ComponentModel.BackgroundWorker BackgroundWorkerTestMockups;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

