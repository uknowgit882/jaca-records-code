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
    public class RecordArtistSqlDao : IRecordsArtistsDao
    {
        private readonly string connectionString;
        public RecordArtistSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public bool GetRecordArtistByRecordIdAndArtistId(int discogsId, int artistId)
        {
            string sql = "SELECT records_artists_id " +
                "FROM records_artists " +
                "WHERE discogs_id = @discogsId AND artist_id = @artistId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@artistId", artistId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int foundDiscogId = Convert.ToInt32(reader["records_artists_id"]);

                        if (foundDiscogId != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to get artist", $"For {discogsId}, {artistId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
            return false;
        }

        /// <summary>
        /// Supply the recordId. Then find the artist ID and supply that here too.
        /// Either it adds it and is successful or errors out
        /// </summary>
        /// <param name="discogsId">From the record</param>
        /// <param name="artistId">From the artist table - use GetArtist</param>
        /// <returns>True if successful</returns>
        /// <exception cref="DaoException"></exception>
        public bool AddRecordArtist(int discogsId, int artistId)
        {
            if (GetRecordArtistByRecordIdAndArtistId(discogsId, artistId))
            {
                return false;
            }
            string sql = "INSERT INTO records_artists (discogs_id, artist_id) " +
                "OUTPUT INSERTED.records_artists_id " +
                "VALUES (@discogsId, @artistId);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@artistId", artistId);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add artist", $"For {discogsId}, {artistId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }

    }
}
