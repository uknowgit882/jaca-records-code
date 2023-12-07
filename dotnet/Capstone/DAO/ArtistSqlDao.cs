using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class ArtistSqlDao : IArtistsDao
    {
        private readonly string connectionString;
        public ArtistSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Artist GetArtist(Artist artist)
        {
            Artist output = null;
            string sql = "SELECT artist_id, name " +
                "FROM artists " +
                "WHERE name = @name";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", artist.Name);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToArtist(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }

        /// <summary>
        /// Gets the artists associated for this record for this specific user
        /// </summary>
        /// <param name="discogId"></param>
        /// <param name="username"></param>
        /// <returns>List of artists for this specific user. Only the name should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Artist> GetArtistsByDiscogsIdAndUsername(int discogId, string username)
        {
            List<Artist> output = new List<Artist>();
            string sql = "SELECT name " +
                "FROM artists " +
                "JOIN records_artists ON artists.artist_id = records_artists.artist_id " +
                "JOIN records ON records_artists.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE records.discogs_id = @discogId AND username = @username";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogId", discogId);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Artist row = new Artist();
                        row.Name = Convert.ToString(reader["name"]);
                        output.Add(row);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the EXTRA artists associated for this record for this specific user
        /// </summary>
        /// <param name="discogId"></param>
        /// <param name="username"></param>
        /// <returns>List of extra artists for this specific user. Only the name should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Artist> GetExtraArtistsByDiscogsIdAndUsername(int discogId, string username)
        {
            List<Artist> output = new List<Artist>();
            string sql = "SELECT artists.name " +
                "FROM artists " +
                "JOIN records_extra_artists ON artists.artist_id = records_extra_artists.extra_artist_id " +
                "JOIN records ON records_extra_artists.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE records.discogs_id = @discogId AND username = @username";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogId", discogId);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Artist row = new Artist();
                        row.Name = Convert.ToString(reader["name"]);
                        output.Add(row);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }
        public bool AddArtist(Artist artist)
        {
            Artist checkedArtist = GetArtist(artist);

            if (checkedArtist != null)
            {
                return false;
            }
            string sql = "INSERT INTO artists (name) " +
                "OUTPUT INSERTED.artist_id " +
                "VALUES (@name);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", artist.Name);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new DaoException("exception occurred", e);
            }
        }


        private Artist MapRowToArtist(SqlDataReader reader)
        {
            Artist output = new Artist();
            output.Artist_Id = Convert.ToInt32(reader["artist_id"]);
            output.Name = Convert.ToString(reader["name"]);
            return output;
        }
    }
}
