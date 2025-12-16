namespace AutoUpload.Windows
{
    partial class FormLoading
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
            this.ProgressBarLoading = new System.Windows.Forms.ProgressBar();
            this.LabelLoadingDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ProgressBarLoading
            // 
            this.ProgressBarLoading.Location = new System.Drawing.Point(34, 28);
            this.ProgressBarLoading.Name = "ProgressBarLoading";
            this.ProgressBarLoading.Size = new System.Drawing.Size(392, 45);
            this.ProgressBarLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ProgressBarLoading.TabIndex = 0;
            this.ProgressBarLoading.UseWaitCursor = true;
            this.ProgressBarLoading.Value = 100;
            // 
            // LabelLoadingDescription
            // 
            this.LabelLoadingDescription.Location = new System.Drawing.Point(34, 86);
            this.LabelLoadingDescription.Name = "LabelLoadingDescription";
            this.LabelLoadingDescription.Size = new System.Drawing.Size(392, 90);
            this.LabelLoadingDescription.TabIndex = 1;
            this.LabelLoadingDescription.Text = "Loading resources from platforms. This will take long time. Please be patient.";
            this.LabelLoadingDescription.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.LabelLoadingDescription.UseWaitCursor = true;
            // 
            // FormLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(440, 185);
            this.ControlBox = false;
            this.Controls.Add(this.LabelLoadingDescription);
            this.Controls.Add(this.ProgressBarLoading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLoading";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.UseWaitCursor = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar ProgressBarLoading;
        public System.Windows.Forms.Label LabelLoadingDescription;
    }
}