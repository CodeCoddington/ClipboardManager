namespace ClipboardManager
{
    partial class ClipboardManager_Small
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
            this.gb_cmSmall = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // gb_cmSmall
            // 
            this.gb_cmSmall.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.gb_cmSmall.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold);
            this.gb_cmSmall.ForeColor = System.Drawing.Color.White;
            this.gb_cmSmall.Location = new System.Drawing.Point(87, 58);
            this.gb_cmSmall.Name = "gb_cmSmall";
            this.gb_cmSmall.Size = new System.Drawing.Size(288, 203);
            this.gb_cmSmall.TabIndex = 0;
            this.gb_cmSmall.TabStop = false;
            this.gb_cmSmall.Text = "sml";
            // 
            // ClipboardManager_Small
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gb_cmSmall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClipboardManager_Small";
            this.Text = "CM";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_cmSmall;
    }
}

