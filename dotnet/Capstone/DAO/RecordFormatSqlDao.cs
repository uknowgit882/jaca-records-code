using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class RecordFormatSqlDao : IRecordsFormatsDao
    {
        private readonly string connectionString;
        public RecordFormatSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Supply the recordId. Then find the format ID and supply that here too.
        /// Either it adds it and is successful or errors out
        /// </summary>
        /// <param name="recordId">From the record</param>
        /// <param name="formatId">From the format table - use GetFormat</param>
        /// <returns>True if successful</returns>
        /// <exception cref="DaoException"></exception>
        public bool AddRecordFormat(int recordId, int formatId)
        {
            string sql = "INSERT INTO records_formats (record_id, format_id) " +
                "OUTPUT INSERTED.records_formats_id " +
                "VALUES (@recordId, @formatId);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recordId", recordId);
                    cmd.Parameters.AddWithValue("@formatId", formatId);
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
