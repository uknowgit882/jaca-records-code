using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class RecordGenreSqlDao : IRecordsGenresDao
    {
        private readonly string connectionString;
        public RecordGenreSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public bool GetRecordGenreByRecordIdAndGenreId(int discogsId, int genreId)
        {
            string sql = "SELECT records_genres_id " +
                "FROM records_genres " +
                "WHERE discogs_id = @discogsId AND genre_id = @genreId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@genreId", genreId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int foundDiscogId = Convert.ToInt32(reader["records_genres_id"]);

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
        /// Supply the recordId. Then find the genre ID and supply that here too.
        /// Either it adds it and is successful or errors out
        /// </summary>
        /// <param name="discogsId">From the record</param>
        /// <param name="genreId">From the genre table - use GetGenre</param>
        /// <returns>True if successful</returns>
        /// <exception cref="DaoException"></exception>
        public bool AddRecordGenre(int discogsId, int genreId)
        {
            if(GetRecordGenreByRecordIdAndGenreId(discogsId, genreId))
            {
                return false;
            }

            string sql = "INSERT INTO records_genres (discogs_id, genre_id) " +
                "OUTPUT INSERTED.records_genres_id " +
                "VALUES (@discogsId, @genreId);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@genreId", genreId);
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
