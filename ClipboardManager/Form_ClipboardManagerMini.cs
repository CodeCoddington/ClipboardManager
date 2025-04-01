using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardManager
{
    public partial class ClipboardManager_mini : Form
    {
        //---CLASS VARS---
        // Clip Types
        private string lastClipType = null;
        private string currClipType = null;
        private const string CLIP_TYPE_NONTEXT = "CLIP_TYPE_NONTEXT";
        private const string CLIP_TYPE_TEXT = "CLIP_TYPE_TEXT";

        // Text
        private string lastClipText;
        private string currClipText;

        // Bools
        private bool clipChanged = false;
        private bool passedFilterTest = false;
        private bool clipIsNullOrBlank = false;

        // Change Types
        private string clipChangeType = string.Empty;
        private const string CHANGE_TYPE_TEXT_TEXT = "CHANGE_TYPE_TEXT_TEXT";
        private const string CHANGE_TYPE_NONTEXT_TEXT = "CHANGE_TYPE_NONTEXT_TEXT";
        private const string CHANGE_TYPE_TEXT_NONTEXT = "CHANGE_TYPE_TEXT_NONTEXT";

        // Filter
        private List<string> filterList = new List<string> { "PROMPT:", "RESPONSE:" };

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
            lastClipType = CLIP_TYPE_NONTEXT;
            lastClipText = string.Empty;
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

        //---MAIN LOOP---
        private async void StartClipboardMonitor()
        {
            await MonitorClipboard();
        }

        private async Task MonitorClipboard()
        {
            while (true)
            {
                await Task.Delay(250);

                // Determine the current clipboard content type and text
                ReadClipContents();

                // Check if the clipboard content has changed
                CheckClipChange();

                // Filter test (only tests if currClipType is text and currClipText is not null or blank)
                RunFilterTest();

                // Perform actions based on results
                ActionOnResults();

                // Update vars for next check
                UpdateVarsForNextCheck();
            }
        }

        private void ReadClipContents()
        {
            if (Clipboard.ContainsText())
            {
                currClipType = CLIP_TYPE_TEXT;
                currClipText = Clipboard.GetText();
                if (currClipText == string.Empty)
                {
                    clipIsNullOrBlank = true;
                }
            }
            else if (Clipboard.GetDataObject() != null)
            {
                currClipType = CLIP_TYPE_NONTEXT;
                currClipText = null;
                clipIsNullOrBlank = false;
            }
            else
            {
                currClipType = CLIP_TYPE_TEXT;
                currClipText = string.Empty;
                clipIsNullOrBlank = true;
            }
        }

        private void CheckClipChange()
        {
            clipChanged = true;
            if (lastClipType == CLIP_TYPE_NONTEXT && currClipType == CLIP_TYPE_TEXT)
            {
                clipChangeType = CHANGE_TYPE_NONTEXT_TEXT;
            }
            else if (lastClipType == CLIP_TYPE_TEXT && currClipType == CLIP_TYPE_TEXT && lastClipText != currClipText)
            {
                clipChangeType = CHANGE_TYPE_TEXT_TEXT;
            }
            else if (lastClipType == CLIP_TYPE_TEXT && currClipType == CLIP_TYPE_NONTEXT)
            {
                clipChangeType = CHANGE_TYPE_TEXT_NONTEXT;
            }
            else
            {
                clipChanged = false;
            }
        }

        private void RunFilterTest()
        {
            passedFilterTest = true;
            if (currClipType == CLIP_TYPE_TEXT && !clipIsNullOrBlank)
            {
                if (filterList.Any(filter => currClipText.Contains(filter)))
                {
                    passedFilterTest = false;
                }
            }
        }

        private void ActionOnResults()
        {
            if (clipChanged)
            {
                Console.WriteLine($"Clip Changed = {clipChanged}");
                Console.WriteLine($"Change Type = {clipChangeType}");
                Console.WriteLine($"Filter Result = {passedFilterTest}");
                Console.WriteLine();
                Console.WriteLine($"Toggle Green = {clipChanged}");
                Console.WriteLine($"Show Red = {!passedFilterTest}");
                Console.WriteLine($"Show Blank = {clipIsNullOrBlank}");
                Console.WriteLine($"Show Boat = {currClipType == CLIP_TYPE_NONTEXT}");
                Console.WriteLine($"Show Text = {currClipType == CLIP_TYPE_TEXT && passedFilterTest}");
            }
        }

        private void UpdateVarsForNextCheck()
        {
            lastClipType = currClipType;
            lastClipText = currClipText;
            clipChanged = false;
            passedFilterTest = false;
            clipIsNullOrBlank = false;
            clipChangeType = null;
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
