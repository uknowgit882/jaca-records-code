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
                ErrorLog.WriteLog("Trying to get artist information", $"{artist.Name} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }

        /// <summary>
        /// Gets the artists associated for this record
        /// </summary>
        /// <param name="discogId"></param>
        /// <returns>List of artists. Only the name should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Artist> GetArtistsByDiscogsId(int discogId)
        {
            List<Artist> output = new List<Artist>();
            string sql = "SELECT name " +
                "FROM artists " +
                "JOIN records_artists ON artists.artist_id = records_artists.artist_id " +
                "JOIN records ON records_artists.discogs_id = records.discogs_id " +
                "WHERE records.discogs_id = @discogId";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogId", discogId);
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
                ErrorLog.WriteLog("Trying to get artist information by discogsId", $"{discogId} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the EXTRA artists associated for this record
        /// </summary>
        /// <param name="discogId"></param>
        /// <returns>List of extra artists. Only the name should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Artist> GetExtraArtistsByDiscogsId(int discogId)
        {
            List<Artist> output = new List<Artist>();
            string sql = "SELECT artists.name " +
                "FROM artists " +
                "JOIN records_extra_artists ON artists.artist_id = records_extra_artists.extra_artist_id " +
                "JOIN records ON records_extra_artists.discogs_id = records.discogs_id " +
                "WHERE records.discogs_id = @discogId";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogId", discogId);
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
                ErrorLog.WriteLog("Trying to get extra artist information by discogsId", $"{discogId} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns how many artists are associated with this user. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Int number of artists</returns>
        /// <exception cref="DaoException"></exception>
        public int GetArtistCountByUsername(string username)
        {
            int output = 0;

            string sql = "SELECT count(artists.artist_id) AS count " +
                "FROM artists " +
                "JOIN records_artists ON artists.artist_id = records_artists.artist_id " +
                "JOIN records ON records_artists.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND is_active = 1 ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = Convert.ToInt32(reader["count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get artist count for user", $"{username} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many extra artists (I guess featured/supporting artists) are associated with this user. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Int number of extra artists</returns>
        /// <exception cref="DaoException"></exception>
        public int GetExtraArtistCountByUsername(string username)
        {
            int output = 0;

            string sql = "SELECT count(artists.artist_id) AS count " +
                "FROM artists " +
                "JOIN records_extra_artists ON artists.artist_id = records_extra_artists.artist_id " +
                "JOIN records ON records_extra_artists.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND is_active = 1 ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = Convert.ToInt32(reader["count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get extra artist count for user", $"{username} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many artists (includes extra artists) are in the entire database.
        /// </summary>
        /// <returns>Int number of artists</returns>
        /// <exception cref="DaoException"></exception>
        public int GetArtistCount()
        {
            int output = 0;

            string sql = "SELECT count(artist_id) AS count " +
                "FROM artists ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = Convert.ToInt32(reader["count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get artist count, total", $"Failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        // I don't think I need this..
        ///// <summary>
        ///// Gets the artists associated for this record for this specific user
        ///// </summary>
        ///// <param name="discogId"></param>
        ///// <param name="username"></param>
        ///// <returns>List of artists for this specific user. Only the name should be sent to the front end - use JSONIgnore on the other properties</returns>
        ///// <exception cref="DaoException"></exception>
        //public List<Artist> GetArtistsByDiscogsIdAndUsername(int discogId, string username)
        //{
        //    List<Artist> output = new List<Artist>();
        //    string sql = "SELECT name " +
        //        "FROM artists " +
        //        "JOIN records_artists ON artists.artist_id = records_artists.artist_id " +
        //        "JOIN records ON records_artists.discogs_id = records.discogs_id " +
        //        "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
        //        "WHERE records.discogs_id = @discogId AND username = @username";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@discogId", discogId);
        //            cmd.Parameters.AddWithValue("@username", username);
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                Artist row = new Artist();
        //                row.Name = Convert.ToString(reader["name"]);
        //                output.Add(row);
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("Sql exception occurred", ex);
        //    }
        //    return output;
        //}

        // I don't think I need this..
        ///// <summary>
        ///// Gets the EXTRA artists associated for this record for this specific user
        ///// </summary>
        ///// <param name="discogId"></param>
        ///// <param name="username"></param>
        ///// <returns>List of extra artists for this specific user. Only the name should be sent to the front end - use JSONIgnore on the other properties</returns>
        ///// <exception cref="DaoException"></exception>
        //public List<Artist> GetExtraArtistsByDiscogsIdAndUsername(int discogId, string username)
        //{
        //    List<Artist> output = new List<Artist>();
        //    string sql = "SELECT artists.name " +
        //        "FROM artists " +
        //        "JOIN records_extra_artists ON artists.artist_id = records_extra_artists.extra_artist_id " +
        //        "JOIN records ON records_extra_artists.discogs_id = records.discogs_id " +
        //        "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
        //        "WHERE records.discogs_id = @discogId AND username = @username";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@discogId", discogId);
        //            cmd.Parameters.AddWithValue("@username", username);
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                Artist row = new Artist();
        //                row.Name = Convert.ToString(reader["name"]);
        //                output.Add(row);
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("Sql exception occurred", ex);
        //    }
        //    return output;
        //}

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
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Adding artist", $"{artist.Name} add failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
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
