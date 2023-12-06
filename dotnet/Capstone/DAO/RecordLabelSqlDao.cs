using Capstone.DAO.Interfaces;
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
        public bool GetRecordLabelByLabelIdAndGenreId(int discogsId, int labelId)
        {
            string sql = "SELECT records_labels_id " +
                "FROM records_labels " +
                "WHERE discogs_id = @discogsId AND label_id = @labelId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@labelId", labelId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int foundDiscogId = Convert.ToInt32(reader["records_labels_id"]);

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
        /// Supply the recordId. Then find the label ID and supply that here too.
        /// Either it adds it and is successful or errors out
        /// </summary>
        /// <param name="discogsId">From the record</param>
        /// <param name="labelId">From the label table - use GetLabel</param>
        /// <returns>True if successful</returns>
        /// <exception cref="DaoException"></exception>
        public bool AddRecordLabel(int discogsId, int labelId)
        {
            if (GetRecordLabelByLabelIdAndGenreId(discogsId, labelId))
            {
                return false;
            }

            string sql = "INSERT INTO records_labels (discogs_id, label_id) " +
                "OUTPUT INSERTED.records_labels_id " +
                "VALUES (@discogsId, @labelId);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
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
