using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            while (true)
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
                await ActionOnResultsAsync();

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

        private async Task ActionOnResultsAsync()
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
                else if (currClipType == CLIP_TYPE_NONTEXT)
                {
                    indicator = pb_clipTypeNonText; // Show type = NonText (Blue with boat)
                }
                else if (currClipType == CLIP_TYPE_TEXT)
                {
                    indicator = pb_clipTypeText; // Show type = Text (Grey with text)

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
                            return;
                        }
                    }
                    // If we did NOT find the text,
                    else
                    {
                        string addResult = await AddNewClipLogEntryAsync();
                        if (addResult == SQL_ERR_ADD)
                        {
                            MessageBox.Show($"Adding to ClipLog failed.", "Insert Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
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
                        await DeleteOldestRecordAsync();
                    }

                    UpdateClipLists();
                    RefreshPanel();
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected clipboard type: {currClipType}");
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
    }
}
