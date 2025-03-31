namespace ClipboardManager
{
    partial class ClipboardManager_Mini
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClipboardManager_Mini));
            this.gb_cmSmall = new System.Windows.Forms.GroupBox();
            this.btn_smallToMedium = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.gb_cmSmall.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_cmSmall
            // 
            this.gb_cmSmall.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.gb_cmSmall.Controls.Add(this.pictureBox3);
            this.gb_cmSmall.Controls.Add(this.btn_smallToMedium);
            this.gb_cmSmall.Controls.Add(this.pictureBox2);
            this.gb_cmSmall.Controls.Add(this.pictureBox1);
            this.gb_cmSmall.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold);
            this.gb_cmSmall.ForeColor = System.Drawing.Color.White;
            this.gb_cmSmall.Location = new System.Drawing.Point(0, 0);
            this.gb_cmSmall.Name = "gb_cmSmall";
            this.gb_cmSmall.Size = new System.Drawing.Size(61, 135);
            this.gb_cmSmall.TabIndex = 0;
            this.gb_cmSmall.TabStop = false;
            this.gb_cmSmall.Text = "Mini";
            // 
            // btn_smallToMedium
            // 
            this.btn_smallToMedium.BackColor = System.Drawing.SystemColors.Control;
            this.btn_smallToMedium.Font = new System.Drawing.Font("Calibri", 30F, System.Drawing.FontStyle.Bold);
            this.btn_smallToMedium.ForeColor = System.Drawing.Color.Black;
            this.btn_smallToMedium.Location = new System.Drawing.Point(4, 77);
            this.btn_smallToMedium.Name = "btn_smallToMedium";
            this.btn_smallToMedium.Size = new System.Drawing.Size(53, 53);
            this.btn_smallToMedium.TabIndex = 1;
            this.btn_smallToMedium.UseVisualStyleBackColor = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox2.Location = new System.Drawing.Point(10, 29);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(41, 41);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ClipboardManager.Properties.Resources.ControlBox_Only2;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(4, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(53, 53);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox3.BackgroundImage = global::ClipboardManager.Properties.Resources.MiniToMedium_Arrows;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox3.Location = new System.Drawing.Point(16, 85);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(28, 37);
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // ClipboardManager_Small
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(61, 135);
            this.Controls.Add(this.gb_cmSmall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClipboardManager_Small";
            this.Text = "CM";
            this.gb_cmSmall.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_cmSmall;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_smallToMedium;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}

