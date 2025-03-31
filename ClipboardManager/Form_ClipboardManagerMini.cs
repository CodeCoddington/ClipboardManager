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
    public partial class ClipboardManager_mini : Form
    {
        //---CONSTRUCTOR---
        public ClipboardManager_mini()
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
            GlobalVars.filterList = new List<string> { "", "RESPONSE:" };
            GlobalVars.lastClipType = GlobalVars.CLIP_TYPE_NONTEXT;
            GlobalVars.lastClipText = string.Empty;
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
            StartClipboardMonitor();
        }

        private void StartClipboardMonitor()
        {
            ClipboardMonitor clipboardMonitor = new ClipboardMonitor(this);
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

        public bool PassedTextFilter(string lastClipType, string currClipType, string lastClipText, string currClipText)
        {
            return (TextToTextChange(lastClipType, currClipType, lastClipText, currClipText) || NonTextToTextChange(lastClipType, currClipType))
                   && GlobalVars.filterList.Any(filter => currClipText.Contains(filter));
        }

        public async void CheckClipboardUpdate(string lastClipType, string currClipType, string lastClipText, string currClipText)
        {
            bool showClipChanged = TypeChange(lastClipType, currClipType) || TextToTextChange(lastClipType, currClipType, lastClipText, currClipText);
            bool show_pb_clipTypeFilteredText = !PassedTextFilter(lastClipType, currClipType, lastClipText, currClipText);

            bool show_pb_clipTypeNonText = !show_pb_clipTypeFilteredText && currClipType == GlobalVars.CLIP_TYPE_NONTEXT;
            bool show_pb_clipTypeBlank = !show_pb_clipTypeFilteredText && currClipType == GlobalVars.CLIP_TYPE_TEXT && string.IsNullOrEmpty(currClipText);
            bool show_pb_clipTypeText = !show_pb_clipTypeFilteredText && currClipType == GlobalVars.CLIP_TYPE_TEXT && !string.IsNullOrEmpty(currClipText);

            await ShowClipboardUpdate(showClipChanged, show_pb_clipTypeFilteredText, show_pb_clipTypeBlank, show_pb_clipTypeNonText, show_pb_clipTypeText);
        }

        private async Task ShowClipboardUpdate(bool showClipChanged, bool show_pb_clipTypeFilteredText, bool show_pb_clipTypeBlank, bool show_pb_clipTypeNonText, bool show_pb_clipTypeText)
        {
            pb_clipTypeFilteredText.Visible = show_pb_clipTypeFilteredText;
            pb_clipTypeBlank.Visible = show_pb_clipTypeBlank;
            pb_clipTypeNonText.Visible = show_pb_clipTypeNonText;
            pb_clipTypeText.Visible = show_pb_clipTypeText;

            if (showClipChanged)
            {
                pb_clipChangedIndicator.Visible = true;
                await Task.Delay(1000);
                pb_clipChangedIndicator.Visible = false;
            }
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
