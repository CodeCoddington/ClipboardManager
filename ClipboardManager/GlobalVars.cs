using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardManager
{
    public partial class form_clipboardManager : Form
    {
        // Global Hook
        private GlobalHook _globalHook;

        // Clip Types
        private string lastClipType = null;
        private string currClipType = null;
        private const string CLIP_TYPE_NONTEXT = "CLIP_TYPE_NONTEXT";
        private const string CLIP_TYPE_TEXT = "CLIP_TYPE_TEXT";

        // SQL
        // Values
        private int maxUnpinnedRecords = 100;

        // Errors
        private const string SQL_SUCCESS = "SQL_SUCCESS";
        private const string SQL_ERR_REORDER = "SQL_ERR_REORDER";
        private const string SQL_ERR_ADD = "SQL_ERR_ADD";
        private const string SQL_ERR_DELETE = "SQL_ERR_DELETE";
        private const string SQL_ERR_FLIPPIN = "SQL_ERR_FLIPPIN";
        private const string SQL_ERR_CLEAN = "SLQ_ERR_CLEAN";
        private const string SQL_ERR_CLEAR = "SQL_ERR_CLEAR";

        // Text
        private string lastClipText;
        private string currClipText;

        // Bools
        private bool clipChanged = false;
        private bool firstCycleAfterGrow = false;
        private bool passedFilterTest = false;
        private bool clipIsNullOrBlank = false;
        private bool clipHasValidText = false;

        // Lists
        private List<string> filterList = new List<string>();
        private List<(string, bool)> clipList = new List<(string, bool)>();

        // Form sizes - will add more as we continue design.
        private Size formSizeMini = new Size(180, 233);
        private Size formSizeMedium = new Size(575, 710);

        // Dynamic control vars
        int topPosition = 6;
        const int X = 6;
        const int OFFSET = 12;

        const int RTB_W = 261;
        const int RTB_H = 100;

        const int BTN_W = 75;
        const int BTN_H = 36;

        const int PB_W = 36;
        const int PB_H = 36;

        int currTabIndex = 0;
        int maxTabIndex = 0;

        // Form dragging
        private bool isDragging = false;
        private Point startPoint = new Point(0, 0);

        // Flow control
        bool eventsEnabled = true;
    }
}