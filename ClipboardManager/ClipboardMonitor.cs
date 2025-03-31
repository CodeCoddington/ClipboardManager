using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardManager
{
    public class ClipboardMonitor
    {
        private Thread monitorThread;
        private ClipboardManager_mini miniForm;

        public ClipboardMonitor(ClipboardManager_mini form)
        {
            miniForm = form;
            monitorThread = new Thread(MonitorClipboard);
            monitorThread.SetApartmentState(ApartmentState.STA);
            monitorThread.Start();
        }

        private void MonitorClipboard()
        {
            while (true)
            {
                Thread.Sleep(250);
                string currClipType;
                string currClipText;

                // Determine the current clipboard content type and text
                if (Clipboard.ContainsText())
                {
                    currClipType = GlobalVars.CLIP_TYPE_TEXT;
                    currClipText = Clipboard.GetText();
                }
                else if (Clipboard.GetDataObject() != null)
                {
                    currClipType = GlobalVars.CLIP_TYPE_NONTEXT;
                    currClipText = null;
                }
                else
                {
                    currClipType = GlobalVars.CLIP_TYPE_TEXT;
                    currClipText = string.Empty;
                }

                // Check if the clipboard content has changed
                if (HasClipboardChanged(GlobalVars.lastClipType, currClipType, GlobalVars.lastClipText, currClipText))
                {
                    // Update the global variables
                    GlobalVars.currClipType = currClipType;
                    GlobalVars.currClipText = currClipText;

                    // Call CheckClipboardUpdate method on the main form
                    miniForm.CheckClipboardUpdate(GlobalVars.lastClipType, currClipType, GlobalVars.lastClipText, currClipText);

                    // Update the last clipboard state
                    GlobalVars.lastClipType = currClipType;
                    GlobalVars.lastClipText = currClipText;
                }
            }
        }

        private bool HasClipboardChanged(string lastType, string currType, string lastText, string currText)
        {
            return lastType != currType || lastText != currText;
        }
    }
}
