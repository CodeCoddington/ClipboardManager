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
using System.Diagnostics;
using WindowsInput;
using WindowsInput.Native;

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
            this.FormClosing += Form_clipboardManager_FormClosing;

            // Buttons and related
            btn_changeFormSize.Click += Btn_changeFormSize_Click;
            pb_sizeChange_buttonFace.Click += Pb_sizeChange_buttonFace_Click;
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
            InitializeClipboard();
            InitializeControlProperties();
            SetFormSize(formSizeMini); // Mini
            SetStartingLocation();
            UpdateClipLists();
            RefreshPanel();
            SetClipboardToFirstItem();
        }

        private void InitializeClipboard()
        {
            Clipboard.Clear();
        }

        private void InitializeControlProperties()
        {
            // Hide hidden window box
            tb_hiddenCommandBox.Parent = this;
            tb_hiddenCommandBox.SendToBack();

            // Initialize indicators to non-visible. Only one should be set to visible at a time.
            pb_clipChangeBox.Visible = true;
            pb_clipChangedIndicator.Visible = false;

            pb_clipTypeBox.Visible = true;
            pb_clipTypeBlank.Visible = false;
            pb_clipTypeNonText.Visible = false;
            pb_clipTypeText.Visible = false;

            // Initialize picture on grow / shrink button
            pb_sizeChange_buttonFace.BackgroundImage = Properties.Resources.MiniToMedium_Arrows_Mirrored_28x37;

            // Add to list - SQL
            typeIndicators.Add(pb_clipTypeText);
            typeIndicators.Add(pb_clipTypeFilteredText);
            typeIndicators.Add(pb_clipTypeBlank);
            typeIndicators.Add(pb_clipTypeNonText);
        }

        private void SetFormSize(Size size)
        {
            bool growing = size == formSizeMedium;
            if (growing)
            {
                // Growing the form
                this.MinimumSize = size;
                this.Size = size;
                this.MaximumSize = size;
            }
            else
            {
                // Shrinking the form
                this.MaximumSize = size;
                this.Size = size;
                this.MinimumSize = size;
            }
        }

        private void SetStartingLocation()
        {
            // Position in top-left corner of primary screen
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(workingArea.Left, workingArea.Top);
        }

        private void SetClipboardToFirstItem()
        {
            if (clipList.Count > 0)
            {
                string clipText = clipList[0].Item1;
                Clipboard.SetText(clipText);
                RichTextBox topRtb = GetControlByName(panel, "rtb_0") as RichTextBox;
                topRtb.BackColor = Color.LightCyan;
            }
        }

        private void RefreshPanel()
        {
            // Clear any existing controls in the panel
            panel.Controls.Clear();
            topPosition = 6;

            if (clipList.Count > 0)
            {
                // Populate Pinned first
                PopulatePanelWithClipList();
            }

            eventsEnabled = true;
        }

        // Generate dynamic controls
        private void PopulatePanelWithClipList()
        {
            // Starting position for the first row of controls
            // top position is initialized to 6 in global
            int x = 6;                                  // Left margin
            int offset = 12;                            // Space between rows

            // Fixed sizes for controls since the form is not resizable
            int rtbWidth = 261;
            int rtbHeight = 100;

            int btnWidth = 75;
            int btnHeight = 36;

            int pbWidth = 36;
            int pbHeight = 36;

            string clipboardText = Clipboard.GetText();

            for (int i = 0; i < clipList.Count; i++)
            {
                string clipText = clipList[i].Item1;
                bool isPinned = clipList[i].Item2;

                // Create a new RichTextBox for displaying truncated text
                RichTextBox richTextBox = new RichTextBox
                {
                    Name = $"rtb_{i}",
                    Size = new Size(rtbWidth, rtbHeight),
                    ReadOnly = true,
                    Font = new Font("Calibri", 12F, FontStyle.Regular),
                    Text = clipText,
                    BackColor = clipText == clipboardText ? Color.LightCyan : SystemColors.Window,
                    Location = new Point(x, topPosition),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left,
                    Tag = isPinned
                };

                // Create a copy to clipboard button
                Button copyButton = new Button()
                {
                    Name = $"copyBtn_{i}",
                    Size = new Size(btnWidth, btnHeight),
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Calibri", 13F, FontStyle.Bold),
                    Text = "Copy",
                    Location = new Point(x + rtbWidth + offset, topPosition + (rtbHeight / 2) - btnHeight - (offset / 4)),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left,
                    Tag = i
                };

                copyButton.Click += CopyButton_Click;

                // Create a new Button for pasting the clipboard content
                Button pasteButton = new Button
                {
                    Name = $"pasteButton_{i}",
                    Size = new Size(btnWidth, btnHeight),
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Calibri", 13F, FontStyle.Bold),
                    Text = "Paste",
                    Location = new Point(x + rtbWidth + offset, topPosition + (rtbHeight / 2) + (offset / 4)),
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                    Tag = i
                };

                // Add an event handler for the Paste button
                pasteButton.Click += PasteButton_Click;

                // Create a new PictureBox for displaying the pinned/unpinned state
                PictureBox pictureBox = new PictureBox
                {
                    Name = $"pb_{i}",
                    Size = new Size(pbWidth, pbHeight),
                    Location = new Point(x + rtbWidth + offset + btnWidth + offset, topPosition + (rtbHeight / 2) - (pbHeight / 2)),
                    BackgroundImage = isPinned ? Properties.Resources.Pin_Pinned_36 : Properties.Resources.Pin_Unpinned_36,
                    BackgroundImageLayout = ImageLayout.None,
                    Tag = i
                };

                // Add an event handler for the pin/unpin feature
                pictureBox.Click += PictureBox_Click;

                // Add all controls to the panel
                panel.Controls.Add(richTextBox);
                panel.Controls.Add(copyButton);
                panel.Controls.Add(pasteButton);
                panel.Controls.Add(pictureBox);

                // Update the top position for the next row
                topPosition += rtbHeight + offset;
            }
        }

        private async void Form_clipboardManager_Shown(object sender, EventArgs e)
        {
            await Task.Delay(500);
            StartAutoItWindowMonitor();
            await StartClipboardMonitorAsync();
        }

        private void StartAutoItWindowMonitor()
        {
            string filePath = @"C:\Users\VinceJ\Documents\AutoIt Programs\Compiled\ClipboardManager_WindowWatcher.exe";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = false
            };

            using (Process p = new Process { StartInfo = psi })
            {
                p.Start();
            }
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

        //---CHANGE FORM SIZE---
        private void Pb_sizeChange_buttonFace_Click(object sender, EventArgs e)
        {
            btn_changeFormSize.PerformClick();
        }

        private void Btn_changeFormSize_Click(object sender, EventArgs e)
        {
            // Set vars
            bool growing = this.Size == formSizeMini;
            //Image buttonFace = growing ? 
                //Properties.Resources.MediumToMini_Arrows_Mirrored_28x37 : Properties.Resources.MiniToMedium_Arrows_Mirrored_28x37;

            Size newFormSize = growing ? formSizeMedium : formSizeMini;

            // Change button face
            pb_sizeChange_buttonFace.BackgroundImage = null;
            pb_sizeChange_buttonFace.BackgroundImage = growing ?
                Properties.Resources.MediumToMini_Arrows_Mirrored_28x37 : Properties.Resources.MiniToMedium_Arrows_Mirrored_28x37;

            // Change form size
            SetFormSize(newFormSize);
        }

        //---COPY CLICK---
        private void CopyButton_Click(object sender, EventArgs e)
        {
            Button copyButton = sender as Button;
            int index = Convert.ToInt32(copyButton.Tag);
            string clipText = clipList[index].Item1;
            Clipboard.SetText(clipText);
        }

        //---PASTE CLICK---
        private async void PasteButton_Click(object sender, EventArgs e)
        {
            Button pasteButton = sender as Button;
            await HandlePasteClickAsync(pasteButton);
        }

        private async Task HandlePasteClickAsync(Button pasteButton)
        {
            // Get rtb
            bool isPinned = clipList[Convert.ToInt32(pasteButton.Tag)].Item2;
            string fullText = clipList[Convert.ToInt32(pasteButton.Tag)].Item1; // Retrieve the full text from the global list

            // Set the clipboard content
            Clipboard.SetText(fullText);       
            await Task.Delay(100);

            // Input simulator did not work. May be able to use it for some autokey stuff though?

            // This will trigger AutoIt to perform the paste action.
            tb_hiddenCommandBox.Text = "PASTE";
        }

        //---PIN CLICK---
        private async void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            await HandlePinClickAsync(pictureBox);
        }

        private async Task HandlePinClickAsync(PictureBox pictureBox)
        {
            // Get index
            int index = Convert.ToInt32(pictureBox.Tag);

            // Get pinned status
            bool isPinned = clipList[index].Item2;

            // Get clipText
            string clipText = clipList[index].Item1;

            // Flip pinned status
            isPinned = !isPinned;

            // Set clipboard
            if (isPinned)
            {
                eventsEnabled = false; // Reset to true in RefreshPanel();
                Clipboard.SetText(clipText);
            }

            // Update SQL
            string flipSuccess = await FlipPinnedStatus(isPinned, clipText);
            if (flipSuccess == SQL_ERR_FLIPPIN)
            {
                MessageBox.Show("There was an error with pinning or unpinning. Shutting down the application.", "Pin Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            UpdateClipLists();
            RefreshPanel();
        }


        //---FORM CLOSING---

        private async void Form_clipboardManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            string result = await CleanClipOrdersAsync();
            if (result == SQL_ERR_CLEAN)
            {
                MessageBox.Show("Error cleaning database", "Database Clean Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach (Process p in Process.GetProcessesByName("ClipboardManager_WindowWatcher"))
            {
                p.Kill();
            }
        }

        //---HELPER---
        private Control GetControlByName(Control parent, string name)
        {
            Control[] controls = parent.Controls.Find(name, true);
            return controls.Length > 0 ? controls[0] : null;
        }
    }
}
