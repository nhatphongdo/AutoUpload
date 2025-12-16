namespace AutoUpload.Windows
{
    partial class SunfrogProduct
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GroupBoxProduct = new System.Windows.Forms.GroupBox();
            this.CheckBoxIsDefault = new System.Windows.Forms.CheckBox();
            this.ButtonChangeSide = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.NumericUpDownPrice = new System.Windows.Forms.NumericUpDown();
            this.PictureBoxProductSample = new System.Windows.Forms.PictureBox();
            this.ButtonClearSelectedColors = new System.Windows.Forms.Button();
            this.FlowLayoutProductColors = new System.Windows.Forms.FlowLayoutPanel();
            this.TooltipProduct = new System.Windows.Forms.ToolTip(this.components);
            this.GroupBoxProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxProductSample)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBoxProduct
            // 
            this.GroupBoxProduct.Controls.Add(this.CheckBoxIsDefault);
            this.GroupBoxProduct.Controls.Add(this.ButtonChangeSide);
            this.GroupBoxProduct.Controls.Add(this.label1);
            this.GroupBoxProduct.Controls.Add(this.NumericUpDownPrice);
            this.GroupBoxProduct.Controls.Add(this.PictureBoxProductSample);
            this.GroupBoxProduct.Controls.Add(this.ButtonClearSelectedColors);
            this.GroupBoxProduct.Controls.Add(this.FlowLayoutProductColors);
            this.GroupBoxProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBoxProduct.Location = new System.Drawing.Point(0, 0);
            this.GroupBoxProduct.Name = "GroupBoxProduct";
            this.GroupBoxProduct.Size = new System.Drawing.Size(600, 556);
            this.GroupBoxProduct.TabIndex = 0;
            this.GroupBoxProduct.TabStop = false;
            this.TooltipProduct.SetToolTip(this.GroupBoxProduct, "You can choose an image above to see sample result");
            // 
            // CheckBoxIsDefault
            // 
            this.CheckBoxIsDefault.AutoSize = true;
            this.CheckBoxIsDefault.Location = new System.Drawing.Point(6, 30);
            this.CheckBoxIsDefault.Name = "CheckBoxIsDefault";
            this.CheckBoxIsDefault.Size = new System.Drawing.Size(134, 29);
            this.CheckBoxIsDefault.TabIndex = 6;
            this.CheckBoxIsDefault.Text = "Is Default";
            this.CheckBoxIsDefault.UseVisualStyleBackColor = true;
            this.CheckBoxIsDefault.CheckedChanged += new System.EventHandler(this.CheckBoxIsDefault_CheckedChanged);
            // 
            // ButtonChangeSide
            // 
            this.ButtonChangeSide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonChangeSide.Location = new System.Drawing.Point(368, 403);
            this.ButtonChangeSide.Name = "ButtonChangeSide";
            this.ButtonChangeSide.Size = new System.Drawing.Size(178, 46);
            this.ButtonChangeSide.TabIndex = 5;
            this.ButtonChangeSide.Text = "Change side";
            this.TooltipProduct.SetToolTip(this.ButtonChangeSide, "You can choose a color and an image above to see sample result");
            this.ButtonChangeSide.UseVisualStyleBackColor = true;
            this.ButtonChangeSide.Click += new System.EventHandler(this.ButtonChangeSide_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(320, 459);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Price:";
            this.TooltipProduct.SetToolTip(this.label1, "You can choose a color and an image above to see sample result");
            // 
            // NumericUpDownPrice
            // 
            this.NumericUpDownPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumericUpDownPrice.DecimalPlaces = 2;
            this.NumericUpDownPrice.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumericUpDownPrice.Location = new System.Drawing.Point(396, 457);
            this.NumericUpDownPrice.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NumericUpDownPrice.Name = "NumericUpDownPrice";
            this.NumericUpDownPrice.Size = new System.Drawing.Size(201, 31);
            this.NumericUpDownPrice.TabIndex = 3;
            this.TooltipProduct.SetToolTip(this.NumericUpDownPrice, "You can choose a color and an image above to see sample result");
            // 
            // PictureBoxProductSample
            // 
            this.PictureBoxProductSample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBoxProductSample.Location = new System.Drawing.Point(320, 30);
            this.PictureBoxProductSample.Name = "PictureBoxProductSample";
            this.PictureBoxProductSample.Size = new System.Drawing.Size(274, 367);
            this.PictureBoxProductSample.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxProductSample.TabIndex = 2;
            this.PictureBoxProductSample.TabStop = false;
            this.TooltipProduct.SetToolTip(this.PictureBoxProductSample, "You can choose a color and an image above to see sample result");
            // 
            // ButtonClearSelectedColors
            // 
            this.ButtonClearSelectedColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClearSelectedColors.Location = new System.Drawing.Point(320, 504);
            this.ButtonClearSelectedColors.Name = "ButtonClearSelectedColors";
            this.ButtonClearSelectedColors.Size = new System.Drawing.Size(274, 46);
            this.ButtonClearSelectedColors.TabIndex = 1;
            this.ButtonClearSelectedColors.Text = "Select None";
            this.TooltipProduct.SetToolTip(this.ButtonClearSelectedColors, "You can choose a color and an image above to see sample result");
            this.ButtonClearSelectedColors.UseVisualStyleBackColor = true;
            this.ButtonClearSelectedColors.Click += new System.EventHandler(this.ButtonClearSelectedColors_Click);
            // 
            // FlowLayoutProductColors
            // 
            this.FlowLayoutProductColors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FlowLayoutProductColors.AutoScroll = true;
            this.FlowLayoutProductColors.Location = new System.Drawing.Point(6, 65);
            this.FlowLayoutProductColors.Name = "FlowLayoutProductColors";
            this.FlowLayoutProductColors.Size = new System.Drawing.Size(308, 485);
            this.FlowLayoutProductColors.TabIndex = 0;
            this.TooltipProduct.SetToolTip(this.FlowLayoutProductColors, "You can choose a color and an image above to see sample result");
            // 
            // TooltipProduct
            // 
            this.TooltipProduct.IsBalloon = true;
            this.TooltipProduct.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TooltipProduct.ToolTipTitle = "Auto Upload";
            // 
            // SunfrogProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GroupBoxProduct);
            this.Name = "SunfrogProduct";
            this.Size = new System.Drawing.Size(600, 556);
            this.GroupBoxProduct.ResumeLayout(false);
            this.GroupBoxProduct.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxProductSample)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBoxProduct;
        private System.Windows.Forms.FlowLayoutPanel FlowLayoutProductColors;
        private System.Windows.Forms.Button ButtonClearSelectedColors;
        private System.Windows.Forms.PictureBox PictureBoxProductSample;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip TooltipProduct;
        public System.Windows.Forms.NumericUpDown NumericUpDownPrice;
        private System.Windows.Forms.Button ButtonChangeSide;
        public System.Windows.Forms.CheckBox CheckBoxIsDefault;
    }
}
