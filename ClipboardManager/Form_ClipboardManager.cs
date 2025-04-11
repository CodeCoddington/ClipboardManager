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
    public partial class form_clipboardManager : Form
    {
        //---CONSTRUCTOR---
        public form_clipboardManager()
        {
            InitializeComponent();
            InitializeEvents();
            InitializeGlobalVars();
        }

        private void InitializeEvents()
        {
            // Form
            this.Load += Form_clipboardManager_Load;
            this.Shown += Form_clipboardManager_Shown;

            // Buttons and related
            btn_miniToMedium.Click += Btn_smallToMedium_Click;
            pb_miniToMedium_buttonFace.Click += Pb_smallToMedium_buttonFace_Click;
        }

        private void InitializeGlobalVars()
        {
            // Sets lastClipType and lastClipText global vars based on SQLite data
            InitializeLastClipGlobals();

            // Populates filter list from SQL
            PopulateFilterList();
        }

        //---FORM LOAD / SHOWN---
        private void Form_clipboardManager_Load(object sender, EventArgs e)
        {
            InitializeControlProperties();
            SetFormSize(formSizeMini); // Mini
            SetFormText("");
            SetStartingLocation();
            UpdateClipList();
            if (clipList.Count > 0)
            {
                PopulatePanelWithClipList();
            }
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

            // Add to list - SQL
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

        private void SetFormText(string text)
        {
            this.Text = text;
        }

        private void SetStartingLocation()
        {
            // Position in bottom-right corner of primary screen
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(workingArea.Right - this.Width, workingArea.Bottom - this.Height);
        }

        // Generate dynamic controls
        private void PopulatePanelWithClipList()
        {
            // Clear any existing controls in the panel
            panel.Controls.Clear();

            // Starting position for the first RichTextBox
            int topPosition = 6;

            for (int i = 0; i < clipList.Count; i++)
            {
                string clipText = clipList[i];

                int x = 6;
                int offset = 12;

                int rtbWidth = 261;
                int rtbHeight = 83;

                int tbWidth = 75;
                int tbHeight = 36;

                int btnWidth = 75;
                int btnHeight = 36;

                int pbWidth = 36;
                int pbHeight = 36;

                // Create a new RichTextBox for storing text
                RichTextBox richTextBox = new RichTextBox
                {
                    Name = $"rtb_{i}",                                                          // Set the name
                    Size = new Size(rtbWidth, rtbHeight),                                       // Set the size
                    ReadOnly = true,                                                            // Make it read-only
                    Text = clipText.Length > 50 ? clipText.Substring(0, 50) + "..." : clipText, // Set the text to clipText (50 char max)
                    Location = new Point(x, topPosition),                                       // Set its location in the panel
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,         // Ensure responsiveness
                    Tag = false                                                                 // Denotes if pinned
                };

                // Create a new TextBox for storing list index
                TextBox textBox = new TextBox
                {
                    Name = $"tb_{i}",                                                       // Set the name
                    Size = new Size(tbWidth, tbHeight),                                     // Set the size
                    ReadOnly = true,                                                        // Make it read-only
                    Font = new Font("Calibri", 13F, FontStyle.Regular),                     // Set the font
                    Text = i.ToString(),                                                    // Set text to list index
                    Location = new Point(x + rtbWidth + offset, topPosition),               // Set its location in the panel
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,     // Ensure responsiveness
                    Tag = false                                                             // Denotes if pinned
                };

                // Create a new Button for pasting
                Button button = new Button
                {
                    Name = $"btn_{i}",                                                                  // Set the name
                    Size = new Size(btnWidth, btnHeight),                                               // Set the Size
                    BackColor = Color.White,                                                            // Set the BackColor
                    Font = new Font("Calibri", 13F, FontStyle.Bold),                                    // Set the font
                    Text = "Paste",                                                                     // Set the text to 'Paste'
                    Location = new Point(x + rtbWidth + offset, topPosition + (rtbHeight - btnHeight)), // Set its location in the panel
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,                 // Ensure responsiveness
                    Tag = false                                                                         // Denotes if pinned
                };

                // Create a new PictureBox for storing the pinned state
                PictureBox pictureBox = new PictureBox
                {
                    Name = $"pb_{i}",
                    Size = new Size(pbWidth, pbHeight),
                    Location = new Point(x + rtbWidth + offset + tbWidth + offset, topPosition + (rtbHeight - btnHeight)),
                    BackgroundImage = Properties.Resources.Pin_Unpinned_36,
                    BackgroundImageLayout = ImageLayout.None,
                    Tag = false
                };

                // Add controls to the panel
                panel.Controls.Add(richTextBox);
                panel.Controls.Add(textBox);
                panel.Controls.Add(button);
                panel.Controls.Add(pictureBox);

                // Update the top position for the next RichTextBox
                topPosition += richTextBox.Height + offset; // Add some spacing between controls
            }
        }

        private async void Form_clipboardManager_Shown(object sender, EventArgs e)
        {
            await Task.Delay(500);
            await StartClipboardMonitorAsync();
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
            // Change form size
            SetFormSize(formSizeMedium);
        }
    }
}
