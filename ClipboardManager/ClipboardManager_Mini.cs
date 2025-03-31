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
            InitializeGlobalVars();
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

        private void InitializeGlobalVars()
        {
            // Eventually lookup from SQLite
            GlobalVars.filterList = new List<string>();
            GlobalVars.filterList.Add("PROMPT:");
            GlobalVars.filterList.Add("RESPONSE:");

            GlobalVars.lastClipType = GlobalVars.CLIP_TYPE_BLANK;
            GlobalVars.lastClipText = string.Empty;
            Clipboard.SetText("HelloWorld!");
            GlobalVars.currClipText = "HelloWorld!";
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

        
        

        //---UPDATE METHODS---
        public bool TypeChange(string lastClipType, string currClipType)
        {
            return currClipType != lastClipType;
        }

        public bool TextToTextChange(string lastClipType, string currClipType, string lastClipText, string currClipText)
        {
            return lastClipType == GlobalVars.CLIP_TYPE_TEXT && currClipType == GlobalVars.CLIP_TYPE_TEXT && lastClipText != currClipText;
        }

        public bool NonTextToTextChange(string lastClipType, string currClipType)
        {
            return lastClipType == GlobalVars.CLIP_TYPE_NONTEXT && currClipType == GlobalVars.CLIP_TYPE_TEXT;
        }

        public bool TextToNonTextChange(string lastClipType, string currClipType)
        {
            return lastClipType == GlobalVars.CLIP_TYPE_TEXT && currClipType == GlobalVars.CLIP_TYPE_NONTEXT;
        }

        public bool CurrClipIsEmptyOrEmptyString(string currClipType)
        {
            return currClipType == GlobalVars.CLIP_TYPE_BLANK;
        }

        public bool PassedTextFilter(string lastClipType, string currClipType, string lastClipText, string currClipText)
        {
            bool passedTextFilter = true;
            return ((TextToTextChange(lastClipType, currClipType, lastClipText, currClipText) && currClipText.Contains(GlobalVars.filterList.Any()) || (NonTextToTextChange(lastClipType, currClipType) && currClipText.Contains(GlobalVars.filterList.Any()));

        }

        public async void ChecklipboardUpdate()
        {
            bool passedTextFilter = PassedTextFilter();
            await ShowClipboardUpdate(GlobalVars.clipChanged, passedTextFilter);
        }

        public async Task ShowClipboardUpdate(bool clipChanged, bool passedTextFilter)
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
