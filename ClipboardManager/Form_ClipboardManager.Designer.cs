﻿namespace ClipboardManager
{
    partial class form_clipboardManager
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
            this.gb_clipIndicator = new System.Windows.Forms.GroupBox();
            this.pb_clipTypeFilteredText = new System.Windows.Forms.PictureBox();
            this.pb_clipTypeText = new System.Windows.Forms.PictureBox();
            this.pb_clipTypeNonText = new System.Windows.Forms.PictureBox();
            this.pb_clipTypeBlank = new System.Windows.Forms.PictureBox();
            this.pb_clipChangedIndicator = new System.Windows.Forms.PictureBox();
            this.pb_miniToMedium_buttonFace = new System.Windows.Forms.PictureBox();
            this.btn_miniToMedium = new System.Windows.Forms.Button();
            this.pb_clipTypeBox = new System.Windows.Forms.PictureBox();
            this.pb_clipChangeBox = new System.Windows.Forms.PictureBox();
            this.gb_clipboard = new System.Windows.Forms.GroupBox();
            this.panel = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.gb_clipIndicator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeFilteredText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeNonText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeBlank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipChangedIndicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_miniToMedium_buttonFace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipChangeBox)).BeginInit();
            this.gb_clipboard.SuspendLayout();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_clipIndicator
            // 
            this.gb_clipIndicator.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.gb_clipIndicator.Controls.Add(this.pb_clipTypeFilteredText);
            this.gb_clipIndicator.Controls.Add(this.pb_clipTypeText);
            this.gb_clipIndicator.Controls.Add(this.pb_clipTypeNonText);
            this.gb_clipIndicator.Controls.Add(this.pb_clipTypeBlank);
            this.gb_clipIndicator.Controls.Add(this.pb_clipChangedIndicator);
            this.gb_clipIndicator.Controls.Add(this.pb_miniToMedium_buttonFace);
            this.gb_clipIndicator.Controls.Add(this.btn_miniToMedium);
            this.gb_clipIndicator.Controls.Add(this.pb_clipTypeBox);
            this.gb_clipIndicator.Controls.Add(this.pb_clipChangeBox);
            this.gb_clipIndicator.Font = new System.Drawing.Font("Calibri", 13F, System.Drawing.FontStyle.Bold);
            this.gb_clipIndicator.ForeColor = System.Drawing.Color.White;
            this.gb_clipIndicator.Location = new System.Drawing.Point(0, 0);
            this.gb_clipIndicator.Name = "gb_clipIndicator";
            this.gb_clipIndicator.Size = new System.Drawing.Size(61, 190);
            this.gb_clipIndicator.TabIndex = 0;
            this.gb_clipIndicator.TabStop = false;
            this.gb_clipIndicator.Text = "Ind";
            // 
            // pb_clipTypeFilteredText
            // 
            this.pb_clipTypeFilteredText.BackColor = System.Drawing.SystemColors.Control;
            this.pb_clipTypeFilteredText.Image = global::ClipboardManager.Properties.Resources.CircleIndicator_FilteredText;
            this.pb_clipTypeFilteredText.Location = new System.Drawing.Point(10, 84);
            this.pb_clipTypeFilteredText.Name = "pb_clipTypeFilteredText";
            this.pb_clipTypeFilteredText.Size = new System.Drawing.Size(41, 41);
            this.pb_clipTypeFilteredText.TabIndex = 3;
            this.pb_clipTypeFilteredText.TabStop = false;
            // 
            // pb_clipTypeText
            // 
            this.pb_clipTypeText.BackColor = System.Drawing.SystemColors.Control;
            this.pb_clipTypeText.Image = global::ClipboardManager.Properties.Resources.CircleIndicator_Text;
            this.pb_clipTypeText.Location = new System.Drawing.Point(10, 84);
            this.pb_clipTypeText.Name = "pb_clipTypeText";
            this.pb_clipTypeText.Size = new System.Drawing.Size(41, 41);
            this.pb_clipTypeText.TabIndex = 3;
            this.pb_clipTypeText.TabStop = false;
            // 
            // pb_clipTypeNonText
            // 
            this.pb_clipTypeNonText.BackColor = System.Drawing.SystemColors.Control;
            this.pb_clipTypeNonText.Image = global::ClipboardManager.Properties.Resources.CircleIndicator_NonText;
            this.pb_clipTypeNonText.Location = new System.Drawing.Point(10, 84);
            this.pb_clipTypeNonText.Name = "pb_clipTypeNonText";
            this.pb_clipTypeNonText.Size = new System.Drawing.Size(41, 41);
            this.pb_clipTypeNonText.TabIndex = 3;
            this.pb_clipTypeNonText.TabStop = false;
            // 
            // pb_clipTypeBlank
            // 
            this.pb_clipTypeBlank.BackColor = System.Drawing.SystemColors.Control;
            this.pb_clipTypeBlank.Image = global::ClipboardManager.Properties.Resources.CircleIndicator_Blank;
            this.pb_clipTypeBlank.Location = new System.Drawing.Point(10, 84);
            this.pb_clipTypeBlank.Name = "pb_clipTypeBlank";
            this.pb_clipTypeBlank.Size = new System.Drawing.Size(41, 41);
            this.pb_clipTypeBlank.TabIndex = 3;
            this.pb_clipTypeBlank.TabStop = false;
            // 
            // pb_clipChangedIndicator
            // 
            this.pb_clipChangedIndicator.BackColor = System.Drawing.SystemColors.Control;
            this.pb_clipChangedIndicator.Image = global::ClipboardManager.Properties.Resources.ClipChanged_Indicator;
            this.pb_clipChangedIndicator.Location = new System.Drawing.Point(10, 29);
            this.pb_clipChangedIndicator.Name = "pb_clipChangedIndicator";
            this.pb_clipChangedIndicator.Size = new System.Drawing.Size(41, 41);
            this.pb_clipChangedIndicator.TabIndex = 3;
            this.pb_clipChangedIndicator.TabStop = false;
            // 
            // pb_miniToMedium_buttonFace
            // 
            this.pb_miniToMedium_buttonFace.BackColor = System.Drawing.SystemColors.Control;
            this.pb_miniToMedium_buttonFace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_miniToMedium_buttonFace.Image = global::ClipboardManager.Properties.Resources.MiniToMedium_Arrows;
            this.pb_miniToMedium_buttonFace.Location = new System.Drawing.Point(16, 141);
            this.pb_miniToMedium_buttonFace.Name = "pb_miniToMedium_buttonFace";
            this.pb_miniToMedium_buttonFace.Size = new System.Drawing.Size(28, 37);
            this.pb_miniToMedium_buttonFace.TabIndex = 2;
            this.pb_miniToMedium_buttonFace.TabStop = false;
            // 
            // btn_miniToMedium
            // 
            this.btn_miniToMedium.BackColor = System.Drawing.SystemColors.Control;
            this.btn_miniToMedium.Font = new System.Drawing.Font("Calibri", 30F, System.Drawing.FontStyle.Bold);
            this.btn_miniToMedium.ForeColor = System.Drawing.Color.Black;
            this.btn_miniToMedium.Location = new System.Drawing.Point(4, 133);
            this.btn_miniToMedium.Name = "btn_miniToMedium";
            this.btn_miniToMedium.Size = new System.Drawing.Size(53, 53);
            this.btn_miniToMedium.TabIndex = 1;
            this.btn_miniToMedium.UseVisualStyleBackColor = false;
            // 
            // pb_clipTypeBox
            // 
            this.pb_clipTypeBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_clipTypeBox.Image = global::ClipboardManager.Properties.Resources.ControlBox_Only2;
            this.pb_clipTypeBox.Location = new System.Drawing.Point(4, 78);
            this.pb_clipTypeBox.Name = "pb_clipTypeBox";
            this.pb_clipTypeBox.Size = new System.Drawing.Size(53, 53);
            this.pb_clipTypeBox.TabIndex = 0;
            this.pb_clipTypeBox.TabStop = false;
            // 
            // pb_clipChangeBox
            // 
            this.pb_clipChangeBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_clipChangeBox.Image = global::ClipboardManager.Properties.Resources.ControlBox_Only2;
            this.pb_clipChangeBox.Location = new System.Drawing.Point(4, 23);
            this.pb_clipChangeBox.Name = "pb_clipChangeBox";
            this.pb_clipChangeBox.Size = new System.Drawing.Size(53, 53);
            this.pb_clipChangeBox.TabIndex = 0;
            this.pb_clipChangeBox.TabStop = false;
            // 
            // gb_clipboard
            // 
            this.gb_clipboard.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.gb_clipboard.Controls.Add(this.panel);
            this.gb_clipboard.Font = new System.Drawing.Font("Calibri", 13F, System.Drawing.FontStyle.Bold);
            this.gb_clipboard.ForeColor = System.Drawing.Color.White;
            this.gb_clipboard.Location = new System.Drawing.Point(68, 0);
            this.gb_clipboard.Name = "gb_clipboard";
            this.gb_clipboard.Size = new System.Drawing.Size(401, 673);
            this.gb_clipboard.TabIndex = 0;
            this.gb_clipboard.TabStop = false;
            this.gb_clipboard.Text = "Clipboard";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.textBox1);
            this.panel.Controls.Add(this.pictureBox1);
            this.panel.Controls.Add(this.button1);
            this.panel.Controls.Add(this.richTextBox1);
            this.panel.Font = new System.Drawing.Font("Calibri", 13F);
            this.panel.Location = new System.Drawing.Point(6, 23);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(390, 647);
            this.panel.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textBox1.Location = new System.Drawing.Point(273, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(37, 29);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "100";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ClipboardManager.Properties.Resources.Pin_Pinned_36;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(354, 53);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(36, 36);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Calibri", 13F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(273, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 36);
            this.button1.TabIndex = 1;
            this.button1.Text = "Paste";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Calibri", 12F);
            this.richTextBox1.Location = new System.Drawing.Point(6, 6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(261, 83);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW";
            // 
            // form_clipboardManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(469, 671);
            this.Controls.Add(this.gb_clipboard);
            this.Controls.Add(this.gb_clipIndicator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(77, 228);
            this.Name = "form_clipboardManager";
            this.gb_clipIndicator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeFilteredText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeNonText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeBlank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipChangedIndicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_miniToMedium_buttonFace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipTypeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_clipChangeBox)).EndInit();
            this.gb_clipboard.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_clipIndicator;
        private System.Windows.Forms.PictureBox pb_clipChangeBox;
        private System.Windows.Forms.Button btn_miniToMedium;
        private System.Windows.Forms.PictureBox pb_miniToMedium_buttonFace;
        private System.Windows.Forms.PictureBox pb_clipTypeBox;
        private System.Windows.Forms.PictureBox pb_clipTypeText;
        private System.Windows.Forms.PictureBox pb_clipTypeNonText;
        private System.Windows.Forms.PictureBox pb_clipTypeBlank;
        private System.Windows.Forms.PictureBox pb_clipChangedIndicator;
        private System.Windows.Forms.PictureBox pb_clipTypeFilteredText;
        private System.Windows.Forms.GroupBox gb_clipboard;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

