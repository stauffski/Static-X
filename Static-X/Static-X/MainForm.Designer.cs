namespace Static_X
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.staticPanel = new System.Windows.Forms.Panel();
            this.staticPictureBox = new System.Windows.Forms.PictureBox();
            this.redCheckBox = new System.Windows.Forms.CheckBox();
            this.greenCheckBox = new System.Windows.Forms.CheckBox();
            this.blueCheckBox = new System.Windows.Forms.CheckBox();
            this.alphaCheckBox = new System.Windows.Forms.CheckBox();
            this.fpsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.detailsCheckBox = new System.Windows.Forms.CheckBox();
            this.startCheckBox = new System.Windows.Forms.CheckBox();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.absoluteCheckBox = new System.Windows.Forms.CheckBox();
            this.scanningCheckBox = new System.Windows.Forms.CheckBox();
            this.invertCheckBox = new System.Windows.Forms.CheckBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.staticPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.staticPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // staticPanel
            // 
            this.staticPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staticPanel.Controls.Add(this.staticPictureBox);
            this.staticPanel.Location = new System.Drawing.Point(0, 0);
            this.staticPanel.Name = "staticPanel";
            this.staticPanel.Size = new System.Drawing.Size(494, 367);
            this.staticPanel.TabIndex = 0;
            // 
            // staticPictureBox
            // 
            this.staticPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staticPictureBox.Location = new System.Drawing.Point(0, 0);
            this.staticPictureBox.Name = "staticPictureBox";
            this.staticPictureBox.Size = new System.Drawing.Size(494, 367);
            this.staticPictureBox.TabIndex = 0;
            this.staticPictureBox.TabStop = false;
            // 
            // redCheckBox
            // 
            this.redCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.redCheckBox.AutoSize = true;
            this.redCheckBox.Checked = true;
            this.redCheckBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.redCheckBox.Location = new System.Drawing.Point(12, 399);
            this.redCheckBox.Name = "redCheckBox";
            this.redCheckBox.Size = new System.Drawing.Size(46, 17);
            this.redCheckBox.TabIndex = 1;
            this.redCheckBox.Text = "Red";
            this.redCheckBox.ThreeState = true;
            this.redCheckBox.UseVisualStyleBackColor = true;
            this.redCheckBox.CheckStateChanged += new System.EventHandler(this.modifierCheckBox_CheckStateChanged);
            // 
            // greenCheckBox
            // 
            this.greenCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.greenCheckBox.AutoSize = true;
            this.greenCheckBox.Checked = true;
            this.greenCheckBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.greenCheckBox.Location = new System.Drawing.Point(64, 399);
            this.greenCheckBox.Name = "greenCheckBox";
            this.greenCheckBox.Size = new System.Drawing.Size(55, 17);
            this.greenCheckBox.TabIndex = 2;
            this.greenCheckBox.Text = "Green";
            this.greenCheckBox.ThreeState = true;
            this.greenCheckBox.UseVisualStyleBackColor = true;
            this.greenCheckBox.CheckStateChanged += new System.EventHandler(this.modifierCheckBox_CheckStateChanged);
            // 
            // blueCheckBox
            // 
            this.blueCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.blueCheckBox.AutoSize = true;
            this.blueCheckBox.Checked = true;
            this.blueCheckBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.blueCheckBox.Location = new System.Drawing.Point(125, 399);
            this.blueCheckBox.Name = "blueCheckBox";
            this.blueCheckBox.Size = new System.Drawing.Size(47, 17);
            this.blueCheckBox.TabIndex = 3;
            this.blueCheckBox.Text = "Blue";
            this.blueCheckBox.ThreeState = true;
            this.blueCheckBox.UseVisualStyleBackColor = true;
            this.blueCheckBox.CheckStateChanged += new System.EventHandler(this.modifierCheckBox_CheckStateChanged);
            // 
            // alphaCheckBox
            // 
            this.alphaCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.alphaCheckBox.AutoSize = true;
            this.alphaCheckBox.Checked = true;
            this.alphaCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.alphaCheckBox.Location = new System.Drawing.Point(178, 399);
            this.alphaCheckBox.Name = "alphaCheckBox";
            this.alphaCheckBox.Size = new System.Drawing.Size(53, 17);
            this.alphaCheckBox.TabIndex = 4;
            this.alphaCheckBox.Text = "Alpha";
            this.alphaCheckBox.ThreeState = true;
            this.alphaCheckBox.UseVisualStyleBackColor = true;
            this.alphaCheckBox.CheckStateChanged += new System.EventHandler(this.modifierCheckBox_CheckStateChanged);
            // 
            // fpsNumericUpDown
            // 
            this.fpsNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fpsNumericUpDown.Location = new System.Drawing.Point(368, 397);
            this.fpsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fpsNumericUpDown.Name = "fpsNumericUpDown";
            this.fpsNumericUpDown.Size = new System.Drawing.Size(59, 20);
            this.fpsNumericUpDown.TabIndex = 5;
            this.fpsNumericUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.fpsNumericUpDown.ValueChanged += new System.EventHandler(this.fpsNumericUpDown_ValueChanged);
            // 
            // detailsCheckBox
            // 
            this.detailsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsCheckBox.AutoSize = true;
            this.detailsCheckBox.Location = new System.Drawing.Point(344, 373);
            this.detailsCheckBox.Name = "detailsCheckBox";
            this.detailsCheckBox.Size = new System.Drawing.Size(88, 17);
            this.detailsCheckBox.TabIndex = 6;
            this.detailsCheckBox.Text = "Show Details";
            this.detailsCheckBox.UseVisualStyleBackColor = true;
            this.detailsCheckBox.CheckedChanged += new System.EventHandler(this.detailsCheckBox_CheckedChanged);
            // 
            // startCheckBox
            // 
            this.startCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.startCheckBox.Checked = true;
            this.startCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startCheckBox.Location = new System.Drawing.Point(433, 395);
            this.startCheckBox.Name = "startCheckBox";
            this.startCheckBox.Size = new System.Drawing.Size(58, 23);
            this.startCheckBox.TabIndex = 7;
            this.startCheckBox.Text = "Stop";
            this.startCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.startCheckBox.UseVisualStyleBackColor = true;
            this.startCheckBox.CheckedChanged += new System.EventHandler(this.startCheckBox_CheckedChanged);
            // 
            // fpsLabel
            // 
            this.fpsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(341, 400);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(27, 13);
            this.fpsLabel.TabIndex = 8;
            this.fpsLabel.Text = "FPS";
            // 
            // absoluteCheckBox
            // 
            this.absoluteCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.absoluteCheckBox.AutoSize = true;
            this.absoluteCheckBox.Location = new System.Drawing.Point(12, 376);
            this.absoluteCheckBox.Name = "absoluteCheckBox";
            this.absoluteCheckBox.Size = new System.Drawing.Size(67, 17);
            this.absoluteCheckBox.TabIndex = 9;
            this.absoluteCheckBox.Text = "Absolute";
            this.absoluteCheckBox.UseVisualStyleBackColor = true;
            this.absoluteCheckBox.CheckStateChanged += new System.EventHandler(this.modifierCheckBox_CheckStateChanged);
            // 
            // scanningCheckBox
            // 
            this.scanningCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scanningCheckBox.AutoSize = true;
            this.scanningCheckBox.Location = new System.Drawing.Point(85, 376);
            this.scanningCheckBox.Name = "scanningCheckBox";
            this.scanningCheckBox.Size = new System.Drawing.Size(71, 17);
            this.scanningCheckBox.TabIndex = 10;
            this.scanningCheckBox.Text = "Scanning";
            this.scanningCheckBox.UseVisualStyleBackColor = true;
            this.scanningCheckBox.CheckStateChanged += new System.EventHandler(this.modifierCheckBox_CheckStateChanged);
            // 
            // invertCheckBox
            // 
            this.invertCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.invertCheckBox.AutoSize = true;
            this.invertCheckBox.Location = new System.Drawing.Point(160, 376);
            this.invertCheckBox.Name = "invertCheckBox";
            this.invertCheckBox.Size = new System.Drawing.Size(53, 17);
            this.invertCheckBox.TabIndex = 11;
            this.invertCheckBox.Text = "Invert";
            this.invertCheckBox.UseVisualStyleBackColor = true;
            this.invertCheckBox.CheckStateChanged += new System.EventHandler(this.modifierCheckBox_CheckStateChanged);
            // 
            // resetButton
            // 
            this.resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resetButton.Location = new System.Drawing.Point(433, 369);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(58, 23);
            this.resetButton.TabIndex = 12;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 421);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.invertCheckBox);
            this.Controls.Add(this.scanningCheckBox);
            this.Controls.Add(this.absoluteCheckBox);
            this.Controls.Add(this.fpsLabel);
            this.Controls.Add(this.startCheckBox);
            this.Controls.Add(this.detailsCheckBox);
            this.Controls.Add(this.fpsNumericUpDown);
            this.Controls.Add(this.alphaCheckBox);
            this.Controls.Add(this.blueCheckBox);
            this.Controls.Add(this.greenCheckBox);
            this.Controls.Add(this.redCheckBox);
            this.Controls.Add(this.staticPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connection Lost";
            this.staticPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.staticPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel staticPanel;
        private System.Windows.Forms.CheckBox redCheckBox;
        private System.Windows.Forms.CheckBox greenCheckBox;
        private System.Windows.Forms.CheckBox blueCheckBox;
        private System.Windows.Forms.CheckBox alphaCheckBox;
        private System.Windows.Forms.NumericUpDown fpsNumericUpDown;
        private System.Windows.Forms.CheckBox detailsCheckBox;
        private System.Windows.Forms.CheckBox startCheckBox;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.PictureBox staticPictureBox;
        private System.Windows.Forms.CheckBox absoluteCheckBox;
        private System.Windows.Forms.CheckBox scanningCheckBox;
        private System.Windows.Forms.CheckBox invertCheckBox;
        private System.Windows.Forms.Button resetButton;
    }
}

