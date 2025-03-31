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
        //---CLASSS VARS---


        //---CONSTRUCTOR---
        public ClipboardManager_Mini()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            this.Shown += ClipboardManager_Small_Shown;
        }

        //---FORM LOAD / SHOWN---
        private async void ClipboardManager_Small_Shown(object sender, EventArgs e)
        {
            await Task.Delay(500);
        }
    }
}
