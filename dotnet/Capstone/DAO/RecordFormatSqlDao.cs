using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

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
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to get format", $"For {discogsId}, {formatId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
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
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add format", $"For {discogsId}, {formatId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }

    }
}
