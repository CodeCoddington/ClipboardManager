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
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace ClipboardManager
{
    public partial class form_clipboardManager : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        //---CONSTRUCTOR---
        public form_clipboardManager()
        {
            InitializeComponent();
            InitializeGlobalHook();
            InitializeEvents();
            InitializeGlobalVars();
        }

        private void InitializeGlobalHook()
        {
            // Subscribe to global hooks
            _globalHook = new GlobalHook();
            _globalHook.ShortcutPressed += GlobalHook_ShortcutPressed;
            _globalHook.SubscribeGlobalHook();
        }

        private void InitializeEvents()
        {
            // Form
            this.Load += Form_clipboardManager_Load;
            this.Shown += Form_clipboardManager_Shown;
            this.Activated += Form_clipboardManager_Activated;
            this.Deactivate += Form_clipboardManager_Deactivate;
            this.FormClosing += Form_clipboardManager_FormClosing;

            // Buttons and related
            btn_changeFormSize.Click += Btn_changeFormSize_Click;
            pb_sizeChange_buttonFace.Click += Pb_sizeChange_buttonFace_Click;

            // PictureBox for dragging
            pb_orthogonalArrow.MouseDown += Pb_orthogonalArrow_MouseDown;
            pb_orthogonalArrow.MouseMove += Pb_orthogonalArrow_MouseMove;
            pb_orthogonalArrow.MouseUp += Pb_orthogonalArrow_MouseUp;

            // Actions
            btn_clearAll.Click += Btn_clearAll_Click;
            btn_textMod_0.Click += StringModifier_Clicked;
            btn_textMod_1.Click += StringModifier_Clicked;
            btn_textMod_2.Click += StringModifier_Clicked;
            btn_textMod_3.Click += StringModifier_Clicked;
            btn_textMod_4.Click += StringModifier_Clicked;
            btn_textMod_5.Click += StringModifier_Clicked;
            btn_textMod_6.Click += StringModifier_Clicked;
            btn_textMod_7.Click += StringModifier_Clicked;
        }

        private void InitializeGlobalVars()
        {
            // Sets lastClipType and lastClipText global vars based on SQLite data
            InitializeLastClipGlobals();

            // Populates filter list from SQL
            PopulateFilterList();
        }

        //---OVERRIDES---
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_SYSKEYDOWN = 0x0104;

            if (panel.Controls.Count > 0)
            {
                if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
                {
                    switch (keyData)
                    {
                        case Keys.Up:
                            currTabIndex = Math.Max(1, currTabIndex - 1);
                            FocusOrSelectControlByTabIndex(panel, currTabIndex);
                            return true; // Mark event as handled
                        case Keys.Down:
                            currTabIndex = Math.Min(maxTabIndex, currTabIndex + 1);
                            FocusOrSelectControlByTabIndex(panel, currTabIndex);
                            return true;
                        case Keys.C:
                            if (currTabIndex > 0)
                            {
                                // Get copy button name
                                Button copy = GetControlByName(panel, $"copyBtn_{currTabIndex - 1}") as Button;
                                copy.PerformClick();
                            }
                            return true;
                        case Keys.P:
                            if (currTabIndex > 0)
                            {
                                // Get pin picture box name
                                PictureBox pin = GetControlByName(panel, $"pb_{currTabIndex - 1}") as PictureBox;
                                PictureBox_Click(pin, EventArgs.Empty);
                            }
                            return true;
                        // end cases
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData); // Allow other keys to work as normal
        }

        //---FORM LOAD / SHOWN---
        private void Form_clipboardManager_Load(object sender, EventArgs e)
        {
            InitializeControlProperties();
            SetFormSize(formSizeMini); // Mini
            SetStartingLocation();
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
            pb_clipType.BackgroundImage = null;

            // Initialize picture on grow / shrink button
            pb_sizeChange_buttonFace.BackgroundImage = Properties.Resources.MiniToMedium_Arrows_Mirrored_28x37;

            // Hide clipboard groupbox
            gb_clipboard.Visible = false;

            // Action buttons

            // Initialize to non-visible
            btn_clearAll.Visible = false;
            gb_actions.Visible = false;
        }

        private void SetStartingLocation()
        {
            // Position in top-left corner of primary screen
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(workingArea.Left, workingArea.Top);
        }

        private async void Form_clipboardManager_Shown(object sender, EventArgs e)
        {
            await Task.Delay(500);
            await StartAutoItWindowMonitorAsync();
            await StartClipboardMonitorAsync();
        }

        private async Task StartAutoItWindowMonitorAsync()
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

            // Wait for process to start
            while (!ProcessExists("ClipboardManager_WindowWatcher"))
            {
                await Task.Delay(250);
            }
        }

        //---FORM_ENTER_AND_LEAVE---
        private void Form_clipboardManager_Deactivate(object sender, EventArgs e)
        {
            if (this.Size == formSizeMedium)
            {
                ChangeFormSize();
            }
        }

        private void Form_clipboardManager_Activated(object sender, EventArgs e)
        {
            if (this.Size == formSizeMini)
            {
                ChangeFormSize();
            }
        }

        //---CLICK AND DRAG---
        private void Pb_orthogonalArrow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Only handle left mouse button
            {
                isDragging = true;
                int pictureBoxX = pb_orthogonalArrow.Location.X;
                int pictureBoxY = pb_orthogonalArrow.Location.Y;

                int arrowCenterX = pictureBoxX - 22;
                int arrowCenterY = pictureBoxY + 4;

                startPoint = new Point(arrowCenterX, arrowCenterY); // Record the starting point relative to the PictureBox
            }
        }

        private void Pb_orthogonalArrow_MouseMove(object sender, MouseEventArgs e)
        {
            int offsetX = 0;
            int offsetY = 0;

            if (isDragging)
            {
                // Calculate the new position of the form
                Point currentScreenPosition = PointToScreen(e.Location);
                Location = new Point(currentScreenPosition.X - startPoint.X + offsetX, currentScreenPosition.Y - startPoint.Y + offsetY);
            }
        }

        private void Pb_orthogonalArrow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Only handle left mouse button
            {
                isDragging = false; // Stop dragging
            }
        }

        private async Task Toggle_pb_clipChangedIndicator()
        {
            pb_clipChangedIndicator.Visible = true;
            pb_orthogonalArrow.BackColor = Color.FromArgb(255, 0, 204, 0);
            await Task.Delay(500);
            pb_clipChangedIndicator.Visible = false;
            pb_orthogonalArrow.BackColor = SystemColors.ControlDark;
        }

        //---CHANGE FORM SIZE---
        private void Pb_sizeChange_buttonFace_Click(object sender, EventArgs e)
        {
            btn_changeFormSize.PerformClick();
        }

        private void Btn_changeFormSize_Click(object sender, EventArgs e)
        {
            ChangeFormSize();
        }

        private void ChangeFormSize()
        {
            // Set vars
            bool growing = gb_clipboard.Visible == false;

            Size newFormSize = growing ? formSizeMedium : formSizeMini;

            // Change button face
            pb_sizeChange_buttonFace.BackgroundImage = null;
            pb_sizeChange_buttonFace.BackgroundImage = growing ?
            Properties.Resources.MediumToMini_Arrows_Mirrored_28x37 : Properties.Resources.MiniToMedium_Arrows_Mirrored_28x37;

            // Load clipboard data if needed
            if (growing)
            {
                // Change form size and then refresh the form
                firstCycleAfterGrow = true; // Gets set false during UpdateVarsForNextCheck()
                SetFormSize(newFormSize);
                gb_clipboard.Visible = true;
                HandleActionControls(growing);
                RefreshForm();
            }
            else
            {
                // Clear the form controls and then change the size
                ClearPanel();
                gb_clipboard.Visible = false;
                SetFormSize(newFormSize);
                HandleActionControls(growing);
            }
        }

        private void HandleActionControls(bool growing)
        {
            gb_actions.Visible = growing;
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

        public void RefreshForm()
        {
            SuspendDrawing(this);
            UpdateClipLists();
            RefreshPanel();
            ResumeDrawing(this);
        }

        public void ClearPanel()
        {
            SuspendDrawing(this);
            panel.Controls.Clear();
            ResumeDrawing(this);
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
                btn_clearAll.Visible = true;
            }
            else
            {
                btn_clearAll.Visible = false;
            }

            eventsEnabled = true;
        }   

        // Generate dynamic controls
        private void PopulatePanelWithClipList()
        {
            // Vars for positioning and sizing of dynmic controls are global
            // Reset set count

            string clipboardText = Clipboard.GetText();

            for (int i = 0; i < clipList.Count; i++)
            {
                string clipText = clipList[i].Item1;
                bool isPinned = clipList[i].Item2;

                // Create a new RichTextBox for displaying truncated text
                RichTextBox richTextBox = new RichTextBox
                {
                    Name = $"rtb_{i}",
                    Size = new Size(RTB_W, RTB_H),
                    ReadOnly = true,
                    Font = new Font("Calibri", 12F, FontStyle.Regular),
                    Text = clipText,
                    BackColor = clipText == clipboardText ? Color.LightCyan : SystemColors.Window,
                    Location = new Point(X, topPosition),
                    TabStop = false,
                    TabIndex = 0,
                    Tag = isPinned
                };

                // Create a copy to clipboard button
                Button copyButton = new Button()
                {
                    Name = $"copyBtn_{i}",
                    Size = new Size(BTN_W, BTN_H),
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Calibri", 13F, FontStyle.Bold),
                    Text = "Copy",
                    TabStop = false,
                    TabIndex = 0,
                    Location = new Point(X + RTB_W + OFFSET, topPosition + (RTB_H / 2) - BTN_H - (OFFSET / 4)),
                    Tag = i
                };

                copyButton.Click += CopyButton_Click;

                // Create a new Button for pasting the clipboard content
                Button pasteButton = new Button
                {
                    Name = $"pasteButton_{i}",
                    Size = new Size(BTN_W, BTN_H),
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Calibri", 13F, FontStyle.Bold),
                    Text = "Paste",
                    TabStop = true,
                    TabIndex = i + 1,
                    Location = new Point(X + RTB_W + OFFSET, topPosition + (RTB_H / 2) + (OFFSET / 4)),
                    Tag = i
                };

                // Add an event handler for the Paste button
                pasteButton.Click += PasteButton_Click;
                pasteButton.GotFocus += PasteButton_GotFocus;
                pasteButton.LostFocus += PasteButton_LostFocus;

                // Create a new PictureBox for displaying the pinned/unpinned state
                PictureBox pictureBox = new PictureBox
                {
                    Name = $"pb_{i}",
                    Size = new Size(PB_W, PB_H),
                    Location = new Point(X + RTB_W + OFFSET + BTN_W + OFFSET, topPosition + (RTB_H / 2) - (PB_H / 2)),
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
                topPosition += RTB_H + OFFSET;

                maxTabIndex = i + 1;
            }

            currTabIndex = 1;
            FocusOrSelectControlByTabIndex(panel, currTabIndex, true);
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

            RefreshForm();
        }

        //---PASTE FOCUS---
        private void PasteButton_GotFocus(object sender, EventArgs e)
        {
            Button pasteButton = sender as Button;
            pasteButton.BackColor = Color.LightCyan;
        }

        private void PasteButton_LostFocus(object sender, EventArgs e)
        {
            Button pasteButton = sender as Button;
            pasteButton.BackColor = Color.White;
        }

        //---GLOBAL_HOOK---
        private void GlobalHook_ShortcutPressed(object sender, EventArgs e)
        {
            // Activate the form
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        //---FORM CLOSING---
        private async void Form_clipboardManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up your clipboard orders
            string result = await CleanClipOrdersAsync();
            if (result == SQL_ERR_CLEAN)
            {
                MessageBox.Show("Error cleaning database", "Database Clean Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Kill any related processes
            foreach (Process p in Process.GetProcessesByName("ClipboardManager_WindowWatcher"))
            {
                p.Kill();
            }

            // Dispose of the global hook
            _globalHook?.Dispose();
        }

        private Control GetControlByName(Control parent, string name)
        {
            Control[] controls = parent.Controls.Find(name, true);
            return controls.Length > 0 ? controls[0] : null;
        }

        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }

        public bool ProcessExists(string processName)
        {
            return Process.GetProcessesByName(processName).Length > 0;
        }

        public void FocusOrSelectControlByTabIndex(Control container, int targetTabIndex, bool select = false)
        {
            foreach (Control control in container.Controls)
            {
                if (control.TabStop && control.TabIndex == targetTabIndex)
                {
                    if (!select)
                    {
                        control.Focus(); // Set focus to the control
                    }
                    else
                    {
                        control.Select(); // Select control
                    }
                    break;
                }
            }
        }

        public void ConsoleCustom_WriteLine(string msg = "")
        {
            string header = "[CONSOLE]:";
            Console.WriteLine($"{header} {msg}");
        }

        public int? GetFocusedControlTabIndex(Control parent, bool recursive = false)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Focused)
                {
                    return control.TabStop ? control.TabIndex : (int?)null;
                }

                // Recursively check child controls
                if (recursive)
                {
                    if (control.HasChildren)
                    {
                        int? tabIndex = GetFocusedControlTabIndex(control, recursive);
                        if (tabIndex.HasValue)
                        {
                            return tabIndex;
                        }
                    }
                }
            }

            return null;
        }
    }
}
