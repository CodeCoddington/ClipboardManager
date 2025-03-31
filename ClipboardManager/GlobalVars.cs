using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipboardManager
{
    public class GlobalVars
    {
        public const string CLIPTYPE_BLANK = "BLANK";
        public const string CLIPTYPE_NONTEXT = "NONTEXT";
        public const string CLIPTYTPE_TEXT = "TEXT";

        public static string lastClipType { get; set; }
        public static string currClipType { get; set; }

        public static string lastClipText { get; set; }
        public static string currClipText { get; set; }
    }
}
