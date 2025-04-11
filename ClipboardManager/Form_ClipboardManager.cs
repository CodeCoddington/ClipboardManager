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
            SetFormText("");
            SetStartingLocation();
            InitializeClipLists();

            // Clear any existing controls in the panel
            panel.Controls.Clear();

            if (pinnedClipList.Count > 0)
            {
                // Populate Pinned first
                PopulatePanelWithClipList(isPinned: true);
            }

            if (unpinnedClipList.Count > 0)
            {
                // Then populate unpinned
                PopulatePanelWithClipList(isPinned: false);
            }
        }

        private void InitializeClipboard()
        {
            Clipboard.Clear();
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

                // Update window text
                this.Text = "Clipboard Manager";
            }
            else
            {
                // Shrinking the form
                this.MaximumSize = size;
                this.Size = size;
                this.MinimumSize = size;

                // Update window text
                this.Text = "CM";
            }
        }

        private void SetFormText(string text)
        {
            this.Text = text;
        }

        private void SetStartingLocation()
        {
            // Position in top-left corner of primary screen
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(workingArea.Left, workingArea.Top);
        }

        // Generate dynamic controls
        private void PopulatePanelWithClipList(bool isPinned)
        {
            List<string> clipList = isPinned ? pinnedClipList : unpinnedClipList;
            if (clipList.Count == 0) return;

            // Starting position for the first row of controls
            // top position is initialized to 6 in global
            int x = 6;                                  // Left margin
            int offset = 12;                            // Space between rows

            // Fixed sizes for controls since the form is not resizable
            int rtbWidth = 261;
            int rtbHeight = 83;

            int tbWidth = 75;
            int tbHeight = 36;

            int btnWidth = 75;
            int btnHeight = 36;

            int pbWidth = 36;
            int pbHeight = 36;

            for (int i = 0; i < clipList.Count; i++)
            {
                string clipText = clipList[i];

                // Create a new RichTextBox for displaying truncated text
                RichTextBox richTextBox = new RichTextBox
                {
                    Name = $"rtb_{i}",
                    Size = new Size(rtbWidth, rtbHeight),
                    ReadOnly = true,
                    Text = clipText.Length > 50 ? clipText.Substring(0, 50) + "..." : clipText,
                    Location = new Point(x, topPosition),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left,
                    Tag = false 
                };

                // Create a new TextBox for displaying the index from the global list
                TextBox textBox = new TextBox
                {
                    Name = $"tb_{i}",
                    Size = new Size(tbWidth, tbHeight),
                    ReadOnly = true,
                    BackColor = SystemColors.ControlDark,
                    ForeColor = Color.Black,
                    Font = new Font("Calibri", 13F, FontStyle.Regular),
                    Text = i.ToString(),
                    Location = new Point(x + rtbWidth + offset, topPosition),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left,
                    Tag = false
                };

                // Create a new Button for pasting the clipboard content
                Button button = new Button
                {
                    Name = $"btn_{i}",
                    Size = new Size(btnWidth, btnHeight),
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Calibri", 13F, FontStyle.Bold),
                    Text = "Paste",
                    Location = new Point(x + rtbWidth + offset, topPosition + (rtbHeight - btnHeight)),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left,
                    Tag = false
                };

                // Add an event handler for the Paste button
                button.Click += (s, e) => HandlePasteClick(Convert.ToInt32(button.Name.Split('_')[1]));

                // Create a new PictureBox for displaying the pinned/unpinned state
                PictureBox pictureBox = new PictureBox
                {
                    Name = $"pb_{i}",
                    Size = new Size(pbWidth, pbHeight),
                    Location = new Point(x + rtbWidth + offset + tbWidth + offset, topPosition + (rtbHeight - pbHeight)),
                    BackgroundImage = Properties.Resources.Pin_Unpinned_36,
                    BackgroundImageLayout = ImageLayout.None,
                    Tag = false
                };

                // Add an event handler for the pin/unpin feature
                pictureBox.Click += (s, e) => HandlePinClick(pictureBox);

                // Add all controls to the panel
                panel.Controls.Add(richTextBox);
                panel.Controls.Add(textBox);
                panel.Controls.Add(button);
                panel.Controls.Add(pictureBox);

                // Update the top position for the next row
                topPosition += rtbHeight + offset;
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

        //---PIN CLICK---
        private void HandlePinClick(PictureBox pictureBox)
        {
            bool isPinned = (bool)pictureBox.Tag; // Get the current pin state
            pictureBox.Tag = !isPinned;           // Toggle the pin state
            pictureBox.BackgroundImage = isPinned ? Properties.Resources.Pin_Pinned_36 : Properties.Resources.Pin_Unpinned_36;

            // Now trigger the SQL pin stuff and moving controls
        }

        //---PASTE CLICK---
        private void HandlePasteClick(int index)
        {
            Button button = (Button)GetControlByName(panel, $"btn_{index}");
            bool isPinned = (bool)button.Tag;
            List<string> clipList = isPinned ? pinnedClipList : unpinnedClipList;
            string fullText = clipList[index]; // Retrieve the full text from the global list
            eventsEnabled = false;
            Clipboard.SetText(fullText);       // Set the clipboard content
            eventsEnabled = true;

            // Then off to autoIt to open the correct window and paste.
        }

        //---HELPER---
        public Control GetControlByName(Control parent, string name)
        {
            Control[] controls = parent.Controls.Find(name, true);
            return controls.Length > 0 ? controls[0] : null;
        }
    }
}
