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

                // Look for pinned records first
                string query = $"SELECT ClipType, ClipText FROM ClipLog WHERE UnpinnedClipOrder = (SELECT MIN(UnpinnedClipOrder) FROM ClipLog)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lastClipType = reader[$"ClipType"].ToString();
                            lastClipText = reader[$"ClipText"].ToString();
                            return;
                        }
                    }
                }

                // If pinned record not found, look for unpinned record
                query = $"SELECT ClipType, ClipText FROM ClipLog WHERE PinnedClipOrder = (SELECT MAX(PinnedClipOrder) FROM ClipLog";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lastClipType = reader[$"ClipType"].ToString();
                            lastClipText = reader[$"ClipText"].ToString();
                        }
                    }
                }
            }
        }

        private void InitializeClipLists()
        {
            unpinnedClipList.Clear();
            pinnedClipList.Clear();

            int clipLogRowCount = GetClipLogRowCount();
            if (clipLogRowCount == 0)
            {
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                string query = @"SELECT ClipText FROM ClipLog WHERE Pinned = 1 ORDER BY PinnedClipOrder ASC";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string clipText = reader["ClipText"].ToString();
                            if (!string.IsNullOrEmpty(clipText))
                            {
                                pinnedClipList.Add(clipText);
                            }
                        }
                    }
                }

                query = @"SELECT ClipText FROM ClipLog ORDER BY UnpinnedClipOrder DESC";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string clipText = reader["ClipText"].ToString();
                            if (!string.IsNullOrEmpty(clipText))
                            {
                                unpinnedClipList.Add(clipText);
                            }
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

                string query = $"SELECT COUNT(*) FROM ClipLog";

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

                string query = $"SELECT String FROM FilteredStrings";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string filterString = reader[$"String"].ToString();
                            filterList.Add(filterString);
                        }
                    }
                }
            }
        }

        //---ASYNC SQL--- RUNS DURING LOOP
        // Check SQL to see if clipText already exists in the database.
        private async Task<int> ReturnClipOrder_IfTextFoundAsync(string text)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                await conn.OpenAsync(); // Open the connection asynchronously

                string query = "SELECT Pinned, UnpinnedClipOrder, PinnedClipOrder FROM ClipLog WHERE ClipText = @Text";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Text", text);

                    using (SQLiteDataReader reader = (SQLiteDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            bool isPinned = reader.GetInt32(reader.GetOrdinal("Pinned")) == 1;
                            int unpinned = reader["UnpinnedClipOrder"] != DBNull.Value
                                ? int.TryParse(reader["UnpinnedClipOrder"].ToString(), out int u) ? u : 0
                                : 0;
                            int pinned = reader["PinnedClipOrder"] != DBNull.Value
                                ? int.TryParse(reader["PinnedClipOrder"].ToString(), out int p) ? p : 0
                                : 0;

                            return isPinned ? pinned : unpinned;
                        }
                    }
                }
            }

            return 0; // Return 0 if no matching row is found
        }

        // If so change the clipOrder so that that entry moves back up to the top
        private async Task<string> ReorderClipLogAsync(int clipOrder)
        {
            if (clipOrder == 0)
            {
                Console.WriteLine("Invalid clipOrder: Could not get max (abs value) ClipOrder");
                return SQL_ERR_REORDER;
            }

            bool isPinned = clipOrder < 0;
            string columnName = isPinned ? "PinnedClipOrder" : "UnpinnedClipOrder";
            string getMaxClipOrderQuery = $"SELECT {(isPinned ? "MIN" : "MAX")}({columnName}) FROM ClipLog";
            string updateClipOrderQuery = $"UPDATE ClipLog SET {columnName} = @NewClipOrder WHERE {columnName} = @CurrentClipOrder";

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                await conn.OpenAsync();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Step 1: Get the current extreme ClipOrder
                        int currentExtremeClipOrder = 0;

                        using (SQLiteCommand getMaxCmd = new SQLiteCommand(getMaxClipOrderQuery, conn, transaction))
                        {
                            object result = await getMaxCmd.ExecuteScalarAsync();
                            if (result == DBNull.Value || result == null || !int.TryParse(result.ToString(), out currentExtremeClipOrder))
                            {
                                transaction.Rollback();
                                Console.WriteLine("No records found in the database.");
                                return SQL_ERR_REORDER;
                            }
                        }

                        // Step 2: Update the target record's ClipOrder to the new extreme value
                        int newClipOrder = isPinned ? currentExtremeClipOrder - 1 : currentExtremeClipOrder + 1;

                        using (SQLiteCommand updateCmd = new SQLiteCommand(updateClipOrderQuery, conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@NewClipOrder", newClipOrder);
                            updateCmd.Parameters.AddWithValue("@CurrentClipOrder", clipOrder);

                            int rowsAffected = await updateCmd.ExecuteNonQueryAsync();
                            if (rowsAffected == 0)
                            {
                                transaction.Rollback();
                                Console.WriteLine("Failed to update ClipOrder. No rows were affected.");
                                return SQL_ERR_REORDER;
                            }
                        }

                        // Commit the transaction
                        transaction.Commit();
                        return SQL_SUCCESS;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error during reordering: {ex.Message}");
                        return SQL_ERR_REORDER;
                    }
                }
            }
        }

        // If the text is not found, this method will add it (Always add new entries as unpinned).
        private async Task<string> AddNewClipLogEntryAsync()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                await conn.OpenAsync(); // Open the connection asynchronously

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Step 1: Get the current maximum ClipOrder
                        string getMaxClipOrderQuery = "SELECT MAX(UnpinnedClipOrder) FROM ClipLog";
                        int maxClipOrder = 0; // Default to 0 if the table is empty

                        using (SQLiteCommand getMaxCmd = new SQLiteCommand(getMaxClipOrderQuery, conn, transaction))
                        {
                            object result = await getMaxCmd.ExecuteScalarAsync(); // Execute the query asynchronously
                            if (result != DBNull.Value && result != null && int.TryParse(result.ToString(), out int clipOrder))
                            {
                                maxClipOrder = clipOrder;
                            }
                        }

                        // Step 2: Insert the new record with ClipOrder = maxClipOrder + 1
                        string insertQuery = @"
                        INSERT INTO ClipLog (UnpinnedClipOrder, ClipType, ClipText)
                        VALUES (@ClipOrder, @ClipType, @ClipText)";
                        using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn, transaction))
                        {
                            insertCmd.Parameters.AddWithValue("@ClipOrder", maxClipOrder + 1);
                            insertCmd.Parameters.AddWithValue("@ClipType", currClipType);
                            insertCmd.Parameters.AddWithValue("@ClipText", currClipText);
                            await insertCmd.ExecuteNonQueryAsync(); // Execute the insertion asynchronously
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Return success
                        return SQL_SUCCESS;
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction if any error occurs
                        transaction.Rollback();
                        Console.WriteLine($"Failed to add new ClipLog entry: {ex.Message}");
                        return SQL_ERR_ADD;
                    }
                }
            }
        }

        // Get number of records from ClipLog
        private async Task<int> GetNumberOfUnpinnedRecordsAsync()
        {
            int recordCount = -1; // Default value in case of failure
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                await conn.OpenAsync(); // Open the connection asynchronously

                // Get count
                string countQuery = @"
                SELECT COUNT(*) FROM ClipLog WHERE Pinned = 0
                ";

                using (SQLiteCommand countCmd = new SQLiteCommand(countQuery, conn))
                {
                    object result = await countCmd.ExecuteScalarAsync(); // Execute the query asynchronously
                    if (result != null && int.TryParse(result.ToString(), out int numRecords))
                    {
                        recordCount = numRecords;
                    }
                }
            }

            return recordCount;
        }

        // Delete old records if we are at capacity
        private async Task<string> DeleteOldestRecordAsync()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                await conn.OpenAsync(); // Open the connection asynchronously

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Step 1: Find the record with the lowest ClipOrder
                        string getMinClipOrderQuery = "SELECT Id FROM ClipLog WHERE UnpinnedClipOrder = (SELECT MIN(UnpinnedClipOrder) FROM ClipLog)";
                        int recordIdToDelete = -1;

                        using (SQLiteCommand getMinCmd = new SQLiteCommand(getMinClipOrderQuery, conn, transaction))
                        {
                            object result = await getMinCmd.ExecuteScalarAsync(); // Execute the query asynchronously
                            if (result != null && int.TryParse(result.ToString(), out int id))
                            {
                                recordIdToDelete = id;
                            }
                        }

                        if (recordIdToDelete < 0)
                        {
                            // Roll back the transaction if any error occurs
                            transaction.Rollback();
                            Console.WriteLine("Failed to find an unpinned record to delete");
                            return SQL_ERR_DELETE;
                        }

                        // Step 2: If a record was found, delete it
                        if (recordIdToDelete != -1)
                        {
                            string deleteQuery = "DELETE FROM ClipLog WHERE Id = @Id";
                            using (SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, conn, transaction))
                            {
                                deleteCmd.Parameters.AddWithValue("@Id", recordIdToDelete);
                                await deleteCmd.ExecuteNonQueryAsync(); // Execute the deletion asynchronously
                            }
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Return success
                        return SQL_SUCCESS;
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction if any error occurs
                        transaction.Rollback();
                        Console.WriteLine("Failed to delete the oldest unpinned record");
                        return SQL_ERR_DELETE;
                    }
                }
            }
        }
    }
}
