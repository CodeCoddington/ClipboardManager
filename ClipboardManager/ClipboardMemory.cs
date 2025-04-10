﻿using System;
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

                string query = $"SELECT ClipType, ClipText FROM ClipLog WHERE ClipOrder = (SELECT MAX(ClipOrder) FROM ClipLog)";
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

        private void UpdateClipList()
        {
            clipList.Clear();

            int clipLogRowCount = GetClipLogRowCount();
            if (clipLogRowCount == 0)
            {
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                string query = @"SELECT ClipText FROM ClipLog ORDER BY ClipOrder DESC";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string clipText = reader["ClipText"].ToString();
                            if (!string.IsNullOrEmpty(clipText))
                            {
                                clipList.Add(clipText);
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

                string query = "SELECT ClipOrder FROM ClipLog WHERE ClipText = @Text";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Text", text);
                    object result = await cmd.ExecuteScalarAsync(); // Execute the query asynchronously

                    // Parse the result and return the ClipOrder, or -1 if not found
                    return result != null && int.TryParse(result.ToString(), out int clipOrder) ? clipOrder : -1;
                }
            }
        }

        // If so change the clipOrder so that that entry moves back up to the top
        private async Task<string> ReorderClipLogAsync(int clipOrder)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                await conn.OpenAsync();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Step 1: Get the current maximum ClipOrder
                        string getMaxClipOrderQuery = "SELECT MAX(ClipOrder) FROM ClipLog";
                        int maxClipOrder = 0;

                        using (SQLiteCommand getMaxCmd = new SQLiteCommand(getMaxClipOrderQuery, conn, transaction))
                        {
                            object result = await getMaxCmd.ExecuteScalarAsync();
                            maxClipOrder = result != null && int.TryParse(result.ToString(), out int lastClipOrder) ? lastClipOrder : 0;
                        }

                        // Optional: Handle case where maxClipOrder is 0 (e.g., empty database)
                        if (maxClipOrder == 0)
                        {
                            transaction.Rollback();
                            Console.WriteLine("Could not get max ClipOrder");
                            return SQL_ERR_REORDER;
                        }

                        // Step 2: Update the target record's ClipOrder to maxClipOrder + 1
                        string updateClipOrderQuery = "UPDATE ClipLog SET ClipOrder = @NewClipOrder WHERE ClipOrder = @CurrentClipOrder";
                        using (SQLiteCommand updateCmd = new SQLiteCommand(updateClipOrderQuery, conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@NewClipOrder", maxClipOrder + 1);
                            updateCmd.Parameters.AddWithValue("@CurrentClipOrder", clipOrder);
                            await updateCmd.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction
                        transaction.Commit();
                        return SQL_SUCCESS;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                        return SQL_ERR_REORDER;
                    }
                }
            }
        }

        // If the text is not found, this method will add it.
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
                        string getMaxClipOrderQuery = "SELECT MAX(ClipOrder) FROM ClipLog";
                        int maxClipOrder = 0; // Default to 0 if the table is empty

                        using (SQLiteCommand getMaxCmd = new SQLiteCommand(getMaxClipOrderQuery, conn, transaction))
                        {
                            object result = await getMaxCmd.ExecuteScalarAsync(); // Execute the query asynchronously
                            if (result != null && int.TryParse(result.ToString(), out int clipOrder))
                            {
                                maxClipOrder = clipOrder;
                            }
                        }

                        // Step 2: Insert the new record with ClipOrder = maxClipOrder + 1
                        string insertQuery = @"
                        INSERT INTO ClipLog (ClipOrder, ClipType, ClipText)
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
        private async Task<int> GetNumberOfRecordsAsync()
        {
            int recordCount = -1; // Default value in case of failure
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                await conn.OpenAsync(); // Open the connection asynchronously

                // Get count
                string countQuery = @"
                SELECT COUNT(*) FROM ClipLog
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
                        string getMinClipOrderQuery = "SELECT Id FROM ClipLog WHERE ClipOrder = (SELECT MIN(ClipOrder) FROM ClipLog)";
                        int recordIdToDelete = -1;

                        using (SQLiteCommand getMinCmd = new SQLiteCommand(getMinClipOrderQuery, conn, transaction))
                        {
                            object result = await getMinCmd.ExecuteScalarAsync(); // Execute the query asynchronously
                            if (result != null && int.TryParse(result.ToString(), out int id))
                            {
                                recordIdToDelete = id;
                            }
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
                        Console.WriteLine("Failed to delete the oldest record");
                        return SQL_ERR_DELETE;
                    }
                }
            }
        }
    }
}
