namespace AutoUpload.Windows
{
    partial class FormPostFacebook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPostFacebook));
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxAccessToken = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ComboBoxFacebookPages = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxContent = new System.Windows.Forms.TextBox();
            this.ButtonPostFacebook = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.BackgroundWorkerPostFacebook = new System.ComponentModel.BackgroundWorker();
            this.ProgressBarPostFacebook = new System.Windows.Forms.ProgressBar();
            this.LinkLabelAccessToken = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Access Token";
            // 
            // TextBoxAccessToken
            // 
            this.TextBoxAccessToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAccessToken.Location = new System.Drawing.Point(102, 10);
            this.TextBoxAccessToken.Name = "TextBoxAccessToken";
            this.TextBoxAccessToken.Size = new System.Drawing.Size(670, 20);
            this.TextBoxAccessToken.TabIndex = 0;
            this.TextBoxAccessToken.TextChanged += new System.EventHandler(this.TextBoxAccessToken_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Facebook Page";
            // 
            // ComboBoxFacebookPages
            // 
            this.ComboBoxFacebookPages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComboBoxFacebookPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxFacebookPages.FormattingEnabled = true;
            this.ComboBoxFacebookPages.Location = new System.Drawing.Point(102, 54);
            this.ComboBoxFacebookPages.Name = "ComboBoxFacebookPages";
            this.ComboBoxFacebookPages.Size = new System.Drawing.Size(670, 21);
            this.ComboBoxFacebookPages.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Content";
            // 
            // TextBoxContent
            // 
            this.TextBoxContent.AcceptsReturn = true;
            this.TextBoxContent.AcceptsTab = true;
            this.TextBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxContent.Location = new System.Drawing.Point(16, 176);
            this.TextBoxContent.Multiline = true;
            this.TextBoxContent.Name = "TextBoxContent";
            this.TextBoxContent.Size = new System.Drawing.Size(756, 217);
            this.TextBoxContent.TabIndex = 3;
            // 
            // ButtonPostFacebook
            // 
            this.ButtonPostFacebook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonPostFacebook.Location = new System.Drawing.Point(616, 399);
            this.ButtonPostFacebook.Name = "ButtonPostFacebook";
            this.ButtonPostFacebook.Size = new System.Drawing.Size(75, 23);
            this.ButtonPostFacebook.TabIndex = 4;
            this.ButtonPostFacebook.Text = "Post";
            this.ButtonPostFacebook.UseVisualStyleBackColor = true;
            this.ButtonPostFacebook.Click += new System.EventHandler(this.ButtonPostFacebook_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(697, 399);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 5;
            this.ButtonClose.Text = "Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // BackgroundWorkerPostFacebook
            // 
            this.BackgroundWorkerPostFacebook.WorkerReportsProgress = true;
            this.BackgroundWorkerPostFacebook.WorkerSupportsCancellation = true;
            this.BackgroundWorkerPostFacebook.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerPostFacebook_DoWork);
            this.BackgroundWorkerPostFacebook.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorkerPostFacebook_ProgressChanged);
            this.BackgroundWorkerPostFacebook.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerPostFacebook_RunWorkerCompleted);
            // 
            // ProgressBarPostFacebook
            // 
            this.ProgressBarPostFacebook.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBarPostFacebook.Location = new System.Drawing.Point(16, 399);
            this.ProgressBarPostFacebook.Name = "ProgressBarPostFacebook";
            this.ProgressBarPostFacebook.Size = new System.Drawing.Size(594, 23);
            this.ProgressBarPostFacebook.TabIndex = 8;
            // 
            // LinkLabelAccessToken
            // 
            this.LinkLabelAccessToken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LinkLabelAccessToken.AutoSize = true;
            this.LinkLabelAccessToken.Location = new System.Drawing.Point(676, 33);
            this.LinkLabelAccessToken.Name = "LinkLabelAccessToken";
            this.LinkLabelAccessToken.Size = new System.Drawing.Size(96, 13);
            this.LinkLabelAccessToken.TabIndex = 6;
            this.LinkLabelAccessToken.TabStop = true;
            this.LinkLabelAccessToken.Text = "Get Access Token";
            this.LinkLabelAccessToken.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelAccessToken_LinkClicked);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Auto Upload";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(756, 61);
            this.label4.TabIndex = 9;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // FormPostFacebook
            // 
            this.AcceptButton = this.ButtonPostFacebook;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(784, 434);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LinkLabelAccessToken);
            this.Controls.Add(this.ProgressBarPostFacebook);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonPostFacebook);
            this.Controls.Add(this.TextBoxContent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ComboBoxFacebookPages);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextBoxAccessToken);
            this.Controls.Add(this.label1);
            this.MinimizeBox = false;
            this.Name = "FormPostFacebook";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Post Facebook";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPostFacebook_FormClosed);
            this.Load += new System.EventHandler(this.FormPostFacebook_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxAccessToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComboBoxFacebookPages;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxContent;
        private System.Windows.Forms.Button ButtonPostFacebook;
        private System.Windows.Forms.Button ButtonClose;
        private System.ComponentModel.BackgroundWorker BackgroundWorkerPostFacebook;
        private System.Windows.Forms.ProgressBar ProgressBarPostFacebook;
        private System.Windows.Forms.LinkLabel LinkLabelAccessToken;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label4;
    }
}