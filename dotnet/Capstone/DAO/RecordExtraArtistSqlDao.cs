using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class RecordExtraArtistSqlDao : IRecordsExtraArtistsDao
    {
        private readonly string connectionString;
        public RecordExtraArtistSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public bool GetRecordExtraArtistByRecordIdAndExtraArtistId(int discogsId, int extraArtistId)
        {
            string sql = "SELECT records_extra_artists_id " +
                "FROM records_extra_artists " +
                "WHERE discogs_id = @discogsId AND extra_artist_id = @extraArtistId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@extraArtistId", extraArtistId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int foundDiscogId = Convert.ToInt32(reader["records_extra_artists_id"]);

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
        /// Supply the recordId. Then find the artist ID and supply that here too.
        /// This adds to the Extra Artist join table, separate from the main artist.
        /// Either it adds it and is successful or errors out
        /// </summary>
        /// <param name="discogsId">From the record</param>
        /// <param name="extraArtistId">From the artist table - use GetArtist</param>
        /// <returns>True if successful</returns>
        /// <exception cref="DaoException"></exception>
        public bool AddRecordExtraArtist(int discogsId, int extraArtistId)
        {
            if (GetRecordExtraArtistByRecordIdAndExtraArtistId(discogsId, extraArtistId))
            {
                return false;
            }

            string sql = "INSERT INTO records_extra_artists (discogs_id, extra_artist_id) " +
                "OUTPUT INSERTED.records_extra_artists_id " +
                "VALUES (@discogsId, @extraArtistId);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@extraArtistId", extraArtistId);
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
