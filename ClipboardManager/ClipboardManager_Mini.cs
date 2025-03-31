using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardManager
{
    public partial class ClipboardManager_Mini: Form
    {
        //---CONSTRUCTOR---
        public ClipboardManager_Mini()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            // Form
            this.Load += ClipboardManager_Mini_Load;
            this.Shown += ClipboardManager_Small_Shown;

            // Buttons and related
            btn_miniToMedium.Click += Btn_smallToMedium_Click;
            pb_miniToMedium_buttonFace.Click += Pb_smallToMedium_buttonFace_Click;
        }

        //---FORM LOAD / SHOWN---
        private void ClipboardManager_Mini_Load(object sender, EventArgs e)
        {
            InitializeControlProperties();
            SetStartingLocation();
        }

        private void InitializeControlProperties()
        {
            // Initialize indicators to non-visible. Only one should be set to visible at a time.
            pb_clipChangeBox.Visible = true;
            pb_clipChangedIndicator.Visible = false;

            pb_clipTypeBox.Visible = true;
            pb_clipTypeBlank.Visible = false;
            pb_clipTypeNonText.Visible = false;
            pb_clipTypeText.Visible = false;
        }

        private void SetStartingLocation()
        {
            // Position in bottom-right corner of primary screen
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(workingArea.Right - this.Width, workingArea.Bottom - this.Height);
        }

        private async void ClipboardManager_Small_Shown(object sender, EventArgs e)
        {
            await Task.Delay(500);
            // Start clipboard class
        }

        // Update images based on clipboard
        private void ShowClipboardUpdate()
        {

        }

        //---SHOW LARGER FORM---
        private void Pb_smallToMedium_buttonFace_Click(object sender, EventArgs e)
        {
            btn_miniToMedium.PerformClick();
        }

        private void Btn_smallToMedium_Click(object sender, EventArgs e)
        {
            // Will launch a larger form with more features.
            throw new NotImplementedException();
        }
    }
}
