namespace AutoUpload.Windows
{
    partial class TeespringProduct
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
            this.GroupBoxProduct.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GroupBoxProduct.Name = "GroupBoxProduct";
            this.GroupBoxProduct.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GroupBoxProduct.Size = new System.Drawing.Size(300, 289);
            this.GroupBoxProduct.TabIndex = 0;
            this.GroupBoxProduct.TabStop = false;
            this.TooltipProduct.SetToolTip(this.GroupBoxProduct, "You can choose an image above to see sample result");
            // 
            // CheckBoxIsDefault
            // 
            this.CheckBoxIsDefault.AutoSize = true;
            this.CheckBoxIsDefault.Location = new System.Drawing.Point(3, 16);
            this.CheckBoxIsDefault.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CheckBoxIsDefault.Name = "CheckBoxIsDefault";
            this.CheckBoxIsDefault.Size = new System.Drawing.Size(71, 17);
            this.CheckBoxIsDefault.TabIndex = 9;
            this.CheckBoxIsDefault.Text = "Is Default";
            this.CheckBoxIsDefault.UseVisualStyleBackColor = true;
            this.CheckBoxIsDefault.CheckedChanged += new System.EventHandler(this.CheckBoxIsDefault_CheckedChanged);
            // 
            // ButtonChangeSide
            // 
            this.ButtonChangeSide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonChangeSide.Location = new System.Drawing.Point(184, 210);
            this.ButtonChangeSide.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ButtonChangeSide.Name = "ButtonChangeSide";
            this.ButtonChangeSide.Size = new System.Drawing.Size(89, 24);
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
            this.label1.Location = new System.Drawing.Point(160, 239);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
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
            this.NumericUpDownPrice.Location = new System.Drawing.Point(198, 238);
            this.NumericUpDownPrice.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.NumericUpDownPrice.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NumericUpDownPrice.Name = "NumericUpDownPrice";
            this.NumericUpDownPrice.Size = new System.Drawing.Size(100, 20);
            this.NumericUpDownPrice.TabIndex = 3;
            this.TooltipProduct.SetToolTip(this.NumericUpDownPrice, "You can choose a color and an image above to see sample result");
            // 
            // PictureBoxProductSample
            // 
            this.PictureBoxProductSample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBoxProductSample.Location = new System.Drawing.Point(160, 16);
            this.PictureBoxProductSample.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PictureBoxProductSample.Name = "PictureBoxProductSample";
            this.PictureBoxProductSample.Size = new System.Drawing.Size(137, 191);
            this.PictureBoxProductSample.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxProductSample.TabIndex = 2;
            this.PictureBoxProductSample.TabStop = false;
            this.TooltipProduct.SetToolTip(this.PictureBoxProductSample, "You can choose a color and an image above to see sample result");
            // 
            // ButtonClearSelectedColors
            // 
            this.ButtonClearSelectedColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClearSelectedColors.Location = new System.Drawing.Point(160, 262);
            this.ButtonClearSelectedColors.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ButtonClearSelectedColors.Name = "ButtonClearSelectedColors";
            this.ButtonClearSelectedColors.Size = new System.Drawing.Size(137, 24);
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
            this.FlowLayoutProductColors.Location = new System.Drawing.Point(3, 34);
            this.FlowLayoutProductColors.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FlowLayoutProductColors.Name = "FlowLayoutProductColors";
            this.FlowLayoutProductColors.Size = new System.Drawing.Size(154, 252);
            this.FlowLayoutProductColors.TabIndex = 0;
            this.TooltipProduct.SetToolTip(this.FlowLayoutProductColors, "You can choose a color and an image above to see sample result");
            // 
            // TooltipProduct
            // 
            this.TooltipProduct.IsBalloon = true;
            this.TooltipProduct.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TooltipProduct.ToolTipTitle = "Auto Upload";
            // 
            // TeespringProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GroupBoxProduct);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TeespringProduct";
            this.Size = new System.Drawing.Size(300, 289);
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
