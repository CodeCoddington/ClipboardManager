using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace ClipboardManager
{
    public partial class form_clipboardManager : Form
    {
        private static readonly string dbPath = @"G:\Public\Vince J\Programming\GitHub\Repos\ClipboardManager\ClipboardManager\SQLiteDb\ClipboardManager.db";

        private static readonly string connString = $"DataSource={dbPath}";

        // Table and Column names
        // FilteredStrings Table
        private static readonly string FilteredStrings = "FilteredStrings";
        private static readonly string String = "String";

        // ClipLog Table
        private static readonly string ClipLog = "ClipLog";
        private static readonly string Id = "Id";
        private static readonly string OriginalTimestamp = "OriginalTimestamp";
        private static readonly string LatestChangeTimestamp = "LatestChangeTimestamp";
        private static readonly string ClipOrder = "ClipOrder";
        private static readonly string ClipType = "ClipType";
        private static readonly string ClipText = "ClipText";

        private List<string> filterList = new List<string>();

        // Methods for setting lastClipType and lastClipText global vars
        private void GetPreviousClipData()
        {
            int clipLogRowCount = GetClipLogRowCount();
            if (clipLogRowCount == 0)
            {
                lastClipType = CLIP_TYPE_NONTEXT;
                lastClipText = string.Empty;
            }
            else
            {
                SetPreviousClipData_FromSql();
            }
        }

        private int GetClipLogRowCount()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                string query = $"SELECT COUNT(*) FROM {ClipLog}";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    return rowCount;
                }
            }
        }

        private void SetPreviousClipData_FromSql()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                string query = $"SELECT {ClipType}, {ClipText} FROM {ClipLog} WHERE {ClipOrder} = 0";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lastClipType = reader[$"{ClipType}"].ToString();
                            lastClipText = reader[$"{ClipText}"].ToString();
                        }
                    }
                }
            }
        }

        // Method for populating filter llist.
        private void PopulateFilterList()
        {
            filterList.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                string query = $"SELECT {String} FROM {FilteredStrings}";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string filterString = reader[$"{String}"].ToString();
                            filterList.Add(filterString);
                        }
                    }
                }
            }
        }

        // Check SQL to see if clipText already exists in the database.
        private int ReturnId_IfTextFound(string text)
        {
            int rowId;

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                string query = $"SELECT Id FROM ClipLog WHERE ClipText = @Text";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Text", text);
                    object result = cmd.ExecuteScalar();

                    rowId = result != null && int.TryParse(result.ToString(), out int id) ? id : -1;
                }
            }
            return rowId; // Should return Id if the text exists and -1 if the text does not.
        }

        // If so change the clipOrder so that that entry moves back up to the top
    }
}
