using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipboardManager
{
    public class GlobalVars
    {
        // list - Eventually look this up from SQL
        public static List<string> filterList { get; set; }

        // Types
        public static string lastClipType { get; set; }
        public static string currClipType { get; set; }
        public const string CLIP_TYPE_BLANK = "BLANK";
        public const string CLIP_TYPE_NONTEXT = "NONTEXT";
        public const string CLIP_TYPE_TEXT = "TEXT";
        
        // Text
        public static string lastClipText { get; set; }
        public static string currClipText { get; set; }

        // Changed
        public static bool typeChanged { get; set; } = false;
        public static bool clipChanged { get; set; } = false;
    }
}
