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
            btn_smallToMedium.Click += Btn_smallToMedium_Click;
            pb_smallToMedium_buttonFace.Click += Pb_smallToMedium_buttonFace_Click;
        }

        //---FORM LOAD / SHOWN---
        private void ClipboardManager_Mini_Load(object sender, EventArgs e)
        {
            InitializeControlProperties();
        }

        private void InitializeControlProperties()
        {
            pb_detectSignal.Visible = false;
        }

        private async void ClipboardManager_Small_Shown(object sender, EventArgs e)
        {
            await Task.Delay(500);
        }

        private void Pb_smallToMedium_buttonFace_Click(object sender, EventArgs e)
        {
            btn_smallToMedium.PerformClick();
        }

        private void Btn_smallToMedium_Click(object sender, EventArgs e)
        {
            // Will launch a larger form with more features.
            throw new NotImplementedException();
        }
    }
}
