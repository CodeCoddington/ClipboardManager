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
                else if (currClipType == CLIP_TYPE_NONTEXT)
                {
                    indicator = pb_clipTypeNonText; // Show type = NonText (Blue with boat)
                }
                else if (currClipType == CLIP_TYPE_TEXT)
                {
                    indicator = pb_clipTypeText; // Show type = Text (Grey with text)

                    // Check SQL to see if clipText already exists in the database.
                    int alreadyExistingClipOrder = ReturnClipOrder_IfTextFound(currClipText);
                    
                    // If we found the text,
                    if (alreadyExistingClipOrder > -1)
                    {
                        // And if the clipOrder of the text is not already at zero (top of list),
                        if (alreadyExistingClipOrder > 0)
                        {
                            // Attempt to reorder
                            string reorderResult = ReorderClipLog(alreadyExistingClipOrder);
                            if (reorderResult != "0")
                            {
                                MessageBox.Show($"Reordering ClipLog failed with the following exception:\r\n\r\n" +
                                    $"'{reorderResult}'");
                            }
                        }
                    }
                    // If we did NOT find the text,
                    else
                    {

                    }
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
