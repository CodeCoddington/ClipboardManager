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

        // Text
        private string lastClipText;
        private string currClipText;

        // Bools
        private bool clipChanged = false;
        private bool passedFilterTest = false;
        private bool clipIsNullOrBlank = false;

        // Lists
        private List<Control> typeIndicators = new List<Control>();
        private List<string> filterList = new List<string>();
        private List<(string, bool)> clipList = new List<(string, bool)>();

        // Form sizes - will add more as we continue design.
        private Size formSizeMini = new Size(77, 228);
        private Size formSizeMedium = new Size(500, 710);

        // Controls
        int topPosition = 6;

        // Flow control
        bool eventsEnabled = true;
    }
}