using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;


namespace ClipboardManager
{
    using System;
    using System.Windows.Forms;
    using Gma.System.MouseKeyHook;

    public class GlobalHook : IDisposable
    {
        private IKeyboardMouseEvents _globalHook;

        public event EventHandler ShortcutPressed;

        public void SubscribeGlobalHook()
        {
            // Create the global hook
            _globalHook = Hook.GlobalEvents();

            // Listen for key presses
            _globalHook.KeyDown += GlobalHookKeyDown;
        }

        private void GlobalHookKeyDown(object sender, KeyEventArgs e)
        {
            // Check if the desired key combination is pressed (e.g., Ctrl + Shift + C)
            if (e.Control && e.Shift && e.KeyCode == Keys.C)
            {
                // Raise the ShortcutPressed event
                ShortcutPressed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            // Unsubscribe from the global hook
            if (_globalHook != null)
            {
                _globalHook.KeyDown -= GlobalHookKeyDown;
                _globalHook.Dispose();
            }
        }
    }
}
