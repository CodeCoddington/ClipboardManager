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
            this.pb_smallToMedium_buttonFace = new System.Windows.Forms.PictureBox();
            this.btn_smallToMedium = new System.Windows.Forms.Button();
            this.pb_detectSignal = new System.Windows.Forms.PictureBox();
            this.pb_box = new System.Windows.Forms.PictureBox();
            this.gb_cmSmall.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_smallToMedium_buttonFace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_detectSignal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_box)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_cmSmall
            // 
            this.gb_cmSmall.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.gb_cmSmall.Controls.Add(this.pb_smallToMedium_buttonFace);
            this.gb_cmSmall.Controls.Add(this.btn_smallToMedium);
            this.gb_cmSmall.Controls.Add(this.pb_detectSignal);
            this.gb_cmSmall.Controls.Add(this.pb_box);
            this.gb_cmSmall.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold);
            this.gb_cmSmall.ForeColor = System.Drawing.Color.White;
            this.gb_cmSmall.Location = new System.Drawing.Point(0, 0);
            this.gb_cmSmall.Name = "gb_cmSmall";
            this.gb_cmSmall.Size = new System.Drawing.Size(61, 135);
            this.gb_cmSmall.TabIndex = 0;
            this.gb_cmSmall.TabStop = false;
            this.gb_cmSmall.Text = "Mini";
            // 
            // pb_smallToMedium_buttonFace
            // 
            this.pb_smallToMedium_buttonFace.BackColor = System.Drawing.SystemColors.Control;
            this.pb_smallToMedium_buttonFace.BackgroundImage = global::ClipboardManager.Properties.Resources.MiniToMedium_Arrows;
            this.pb_smallToMedium_buttonFace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_smallToMedium_buttonFace.Location = new System.Drawing.Point(16, 85);
            this.pb_smallToMedium_buttonFace.Name = "pb_smallToMedium_buttonFace";
            this.pb_smallToMedium_buttonFace.Size = new System.Drawing.Size(28, 37);
            this.pb_smallToMedium_buttonFace.TabIndex = 2;
            this.pb_smallToMedium_buttonFace.TabStop = false;
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
            // pb_detectSignal
            // 
            this.pb_detectSignal.BackColor = System.Drawing.SystemColors.Control;
            this.pb_detectSignal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pb_detectSignal.BackgroundImage")));
            this.pb_detectSignal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_detectSignal.Location = new System.Drawing.Point(10, 29);
            this.pb_detectSignal.Name = "pb_detectSignal";
            this.pb_detectSignal.Size = new System.Drawing.Size(41, 41);
            this.pb_detectSignal.TabIndex = 0;
            this.pb_detectSignal.TabStop = false;
            // 
            // pb_box
            // 
            this.pb_box.BackgroundImage = global::ClipboardManager.Properties.Resources.ControlBox_Only2;
            this.pb_box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_box.Location = new System.Drawing.Point(4, 23);
            this.pb_box.Name = "pb_box";
            this.pb_box.Size = new System.Drawing.Size(53, 53);
            this.pb_box.TabIndex = 0;
            this.pb_box.TabStop = false;
            // 
            // ClipboardManager_Mini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(61, 134);
            this.Controls.Add(this.gb_cmSmall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClipboardManager_Mini";
            this.gb_cmSmall.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_smallToMedium_buttonFace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_detectSignal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_box)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_cmSmall;
        private System.Windows.Forms.PictureBox pb_detectSignal;
        private System.Windows.Forms.PictureBox pb_box;
        private System.Windows.Forms.Button btn_smallToMedium;
        private System.Windows.Forms.PictureBox pb_smallToMedium_buttonFace;
    }
}

