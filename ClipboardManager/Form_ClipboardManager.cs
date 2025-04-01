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
            GetPreviousClipData();

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

        private async void Form_clipboardManager_Shown(object sender, EventArgs e)
        {
            await Task.Delay(500);
            StartClipboardMonitor();
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
