using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace ClipboardManager
{
    public partial class form_clipboardManager : Form
    {
        private async Task StartClipboardMonitorAsync()
        {
            await MonitorClipboard();
        }

        private async Task MonitorClipboard()
        {
            while (ProcessExists("ClipboardManager_WindowWatcher"))
            {
                await Task.Delay(250);

                if (!eventsEnabled) continue;

                // Determine the current clipboard content type and text
                ReadClipContents();

                // Check if the clipboard content has changed
                CheckClipChange();

                // Filter test (only tests if currClipType is text and currClipText is not null or blank)
                RunFilterTest();

                // Perform actions based on results
                await UpdateIndicatorAsync();

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
                if (string.IsNullOrEmpty(currClipText) || string.IsNullOrWhiteSpace(currClipText))
                {
                    clipIsNullOrBlank = true;
                }
            }
            else if (ClipboardContainsNonTextData())
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

        private bool ClipboardContainsNonTextData()
        {
            // Check for standard text data formats
            if (Clipboard.ContainsData(DataFormats.Text) || Clipboard.ContainsData(DataFormats.UnicodeText))
            {
                return false; // Only text is present
            }

            // Check for other data formats
            foreach (string format in Clipboard.GetDataObject().GetFormats())
            {
                if (format != DataFormats.Text && format != DataFormats.UnicodeText)
                {
                    return true; // Found non-text data
                }
            }

            return false; // No non-text data found
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

        private async Task UpdateIndicatorAsync()
        {
            if (clipChanged)
            {
                // Turn on clipChange indicator
                Toggle_pb_clipChangedIndicatorOn();
                await Task.Delay(500);

                // Set the correct indicator to show and then call method to show it.
                if (clipIsNullOrBlank)
                {
                    pb_clipType.BackgroundImage = Properties.Resources.CircleIndicator_Blank; // Show type = Blank (White)
                    clipHasValidText = false;
                }
                else if (!passedFilterTest)
                {
                    pb_clipType.BackgroundImage = Properties.Resources.CircleIndicator_FilteredText; // Show type = Filtered (Red)
                    clipHasValidText = false;
                }
                else if (currClipType == CLIP_TYPE_NONTEXT)
                {
                    pb_clipType.BackgroundImage = Properties.Resources.CircleIndicator_NonText; // Show type = NonText (Blue with boat)
                    clipHasValidText = false;
                }
                else if (currClipType == CLIP_TYPE_TEXT)
                {
                    pb_clipType.BackgroundImage = Properties.Resources.CircleIndicator_Text; // Show type = Text (Grey with text)
                    clipHasValidText = true;
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected clipboard type: {currClipType}");
                }
            }

            // Check whether to run database and text tasks
            await CheckConditionsToPerformTextActionsAsync();

            // Turn off clip change indicator if needed
            if (pb_clipChangedIndicator.Visible == true)
            {
                Toggle_pb_clipChangedIndicatorOff();
            }
        }

        private async Task CheckConditionsToPerformTextActionsAsync()
        {
            if (clipChanged || firstCycleAfterGrow)
            {
                // Then perform text functions if the form is the larger size
                if (clipHasValidText && gb_clipboard.Visible)
                {
                    await DatabaseOperations_OnTextAsync();
                }
            }
        }

        private async Task DatabaseOperations_OnTextAsync()
        {
            // Check SQL to see if clipText already exists in the database.
            int alreadyExistingClipOrder = await ReturnClipOrder_IfTextFoundAsync(currClipText);

            // If we found the text,
            if (alreadyExistingClipOrder != 0)
            {
                // Attempt to reorder
                string reorderResult = await ReorderClipLogAsync(alreadyExistingClipOrder);
                if (reorderResult == SQL_ERR_REORDER)
                {
                    MessageBox.Show($"Reordering ClipLog failed.", "Reorder Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            // If we did NOT find the text,
            else
            {
                string addResult = await AddNewClipLogEntryAsync();
                if (addResult == SQL_ERR_ADD)
                {
                    MessageBox.Show($"Adding to ClipLog failed.", "Insert Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }

            // Check to see if we have maxxed out the database
            int clipLogCount = await GetNumberOfUnpinnedRecordsAsync();
            if (clipLogCount < 0)
            {
                MessageBox.Show($"Getting ClipLog count failed.", "Count Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If we're maxxed, delete the oldest record (lowest ClipCount int)
            if (clipLogCount > maxUnpinnedRecords)
            {
                string result = await DeleteOldestRecordAsync();
                if (result == SQL_ERR_DELETE)
                {
                    MessageBox.Show("There was an issue deleting the oldest record from the database. Closing application.");
                    this.Close();
                }
            }

            RefreshForm();
        }

        private void UpdateVarsForNextCheck()
        {
            lastClipType = currClipType;
            lastClipText = currClipText;
            clipChanged = false;
            firstCycleAfterGrow = false;
            passedFilterTest = false;
            clipIsNullOrBlank = false;
        }
    }
}
