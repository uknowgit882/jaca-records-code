using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class RecordArtistSqlDao : IRecordsArtistsDao
    {
        private readonly string connectionString;
        public RecordArtistSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
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
            catch (Exception e)
            {
                throw new DaoException("exception occurred", e);
            }
        }

    }
}
