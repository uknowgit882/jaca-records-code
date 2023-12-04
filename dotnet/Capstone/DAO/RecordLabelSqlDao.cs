using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class RecordLabelSqlDao : IRecordsLabelsDao
    {
        private readonly string connectionString;
        public RecordLabelSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Supply the recordId. Then find the label ID and supply that here too.
        /// Either it adds it and is successful or errors out
        /// </summary>
        /// <param name="recordId">From the record</param>
        /// <param name="labelId">From the label table - use GetLabel</param>
        /// <returns>True if successful</returns>
        /// <exception cref="DaoException"></exception>
        public bool AddRecordLabel(int recordId, int labelId)
        {
            string sql = "INSERT INTO records_labels (record_id, label_id) " +
                "OUTPUT INSERTED.records_labels_id " +
                "VALUES (@recordId, @labelId);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recordId", recordId);
                    cmd.Parameters.AddWithValue("@labelId", labelId);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new DaoException("exception occurred", e);
            }
        }

    }
}
