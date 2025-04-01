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

        // Filter list - Get from SQL
        private List<string> filterList = new List<string> { "PROMPT:", "RESPONSE:" };

        // Indicator list
        private List<Control> typeIndicators = new List<Control>();

        // Form sizes - will add more as we continue design.
        private Size formSizeMini = new Size(77, 228);

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
            SetFormSize(formSizeMini); // Mini
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

            // Add to list
            typeIndicators.Add(pb_clipTypeText);
            typeIndicators.Add(pb_clipTypeFilteredText);
            typeIndicators.Add(pb_clipTypeBlank);
            typeIndicators.Add(pb_clipTypeNonText);
        }

        private void SetFormSize(Size size)
        {
            this.MinimumSize = size;
            this.Size = size;
            this.MaximumSize = size;
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
                currClipText = Clipboard.GetText().TrimEnd(new char[] { '\r', '\n' });
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
            clipChanged = currClipType != lastClipType || currClipText != lastClipText;
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

        private async void ActionOnResults()
        {
            if (clipChanged)
            {
                // Set the correct indicator to show and then call method to show it.
                Control indicator;
                if (clipIsNullOrBlank)
                {
                    indicator = pb_clipTypeBlank; // Show type = Blank (White)
                }
                else if (!passedFilterTest)
                {
                    indicator = pb_clipTypeFilteredText; // Show type = Filtered (Red)
                }
                else if (currClipType == CLIP_TYPE_TEXT)
                {
                    indicator = pb_clipTypeText; // Show type = Text (Grey with text)
                }
                else if (currClipType == CLIP_TYPE_NONTEXT)
                {
                    indicator = pb_clipTypeNonText; // Show type = NonText (Blue with boat)
                }
                else
                {
                    indicator = pb_clipTypeBox;
                }
                ShowClipTypeIndicator((PictureBox)indicator);

                // Toggle green clip indicator to show that clipboard has changed
                await Toggle_pb_clipChangedIndicator();
            }
        }

        private void UpdateVarsForNextCheck()
        {
            lastClipType = currClipType;
            lastClipText = currClipText;
            clipChanged = false;
            passedFilterTest = false;
            clipIsNullOrBlank = false;
        }

        //---ACTION METHODS---
        private void ShowClipTypeIndicator(PictureBox indicator)
        {
            foreach (PictureBox pictureBox in typeIndicators)
            {
                pictureBox.Visible = pictureBox == indicator;
            }
        }

        private async Task Toggle_pb_clipChangedIndicator()
        {
            pb_clipChangedIndicator.Visible = true;
            await Task.Delay(500);
            pb_clipChangedIndicator.Visible = false;
        }

        //---ENLARGE FORM---
        private void Pb_smallToMedium_buttonFace_Click(object sender, EventArgs e)
        {
            btn_miniToMedium.PerformClick();
        }

        private void Btn_smallToMedium_Click(object sender, EventArgs e)
        {
            // Will grow the form to a larger size with more features.
            throw new NotImplementedException();
        }
    }
}
