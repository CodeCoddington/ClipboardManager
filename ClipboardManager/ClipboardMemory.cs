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
        private void InitializeLastClipGlobals()
        {
            int clipLogRowCount = GetClipLogRowCount();
            if (clipLogRowCount == 0)
            {
                lastClipType = CLIP_TYPE_NONTEXT;
                lastClipText = string.Empty;
                return;
            }

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


        // Method for populating filter list.
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
        private int ReturnClipOrder_IfTextFound(string text)
        {
            int rowClipOrder;

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                string query = $"SELECT {ClipOrder} FROM ClipLog WHERE ClipText = @Text";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Text", text);
                    object result = cmd.ExecuteScalar();

                    rowClipOrder = result != null && int.TryParse(result.ToString(), out int clipOrder) ? clipOrder : -1;
                }
            }
            return rowClipOrder; // Should return ClipOrder if the text exists and -1 if the text does not.
        }

        // If so change the clipOrder so that that entry moves back up to the top
        private string ReorderClipLog(int originalClipOrder)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Step 1: If the ClipOrder is already 0, no reordering is needed
                        if (originalClipOrder == 0)
                        {
                            transaction.Rollback(); // Exit the transaction early
                            return "0"; // Zero indicates success
                        }

                        // Step 2: Temporarily set the ClipOrder of the found text to -1
                        string setTemporaryOrderQuery = "UPDATE ClipLog SET ClipOrder = -1 WHERE ClipOrder = @ClipOrder";
                        using (SQLiteCommand tempUpdateCmd = new SQLiteCommand(setTemporaryOrderQuery, conn, transaction))
                        {
                            tempUpdateCmd.Parameters.AddWithValue("@ClipOrder", originalClipOrder);
                            tempUpdateCmd.ExecuteNonQuery();
                        }

                        // Step 3: Increment the ClipOrder of all records from 0 to (originalClipOrder - 1)
                        string incrementOrderQuery = @"
                        UPDATE ClipLog
                        SET ClipOrder = ClipOrder + 1
                        WHERE ClipOrder >= 0 AND ClipOrder < @OriginalClipOrder";
                        using (SQLiteCommand incrementCmd = new SQLiteCommand(incrementOrderQuery, conn, transaction))
                        {
                            incrementCmd.Parameters.AddWithValue("@OriginalClipOrder", originalClipOrder);
                            incrementCmd.ExecuteNonQuery();
                        }

                        // Step 4: Update the ClipOrder of the found text to 0
                        string setFinalOrderQuery = "UPDATE ClipLog SET ClipOrder = 0 WHERE ClipOrder = -1";
                        using (SQLiteCommand finalUpdateCmd = new SQLiteCommand(setFinalOrderQuery, conn, transaction))
                        {
                            finalUpdateCmd.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Return
                        return "0"; // Zero indicates success
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction if any error occurs
                        transaction.Rollback();
                        return (ex.Message);
                    }
                }
            }
        }



    }
}
