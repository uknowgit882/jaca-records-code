using Capstone.DAO.Interfaces;
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
        public bool GetRecordFormatByRecordIdAndFormatId(int discogsId, int formatId)
        {
            string sql = "SELECT records_formats_id " +
                "FROM records_formats " +
                "WHERE discogs_id = @discogsId AND format_id = @formatId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@formatId", formatId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int foundDiscogId = Convert.ToInt32(reader["records_formats_id"]);

                        if (foundDiscogId != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new DaoException("exception occurred", e);
            }
            return false;
        }

        /// <summary>
        /// Supply the recordId. Then find the format ID and supply that here too.
        /// Either it adds it and is successful or errors out
        /// </summary>
        /// <param name="discogsId">From the record</param>
        /// <param name="formatId">From the format table - use GetFormat</param>
        /// <returns>True if successful</returns>
        /// <exception cref="DaoException"></exception>
        public bool AddRecordFormat(int discogsId, int formatId)
        {
            if (GetRecordFormatByRecordIdAndFormatId(discogsId, formatId))
            {
                return false;
            }

            string sql = "INSERT INTO records_formats (discogs_id, format_id) " +
                "OUTPUT INSERTED.records_formats_id " +
                "VALUES (@discogsId, @formatId);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
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
