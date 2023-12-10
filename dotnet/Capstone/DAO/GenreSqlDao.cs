using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Capstone.DAO
{
    public class GenreSqlDao : IGenresDao
    {
        private readonly string connectionString;

        public GenreSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Genre GetGenre(string genre)
        {
            Genre output = null;
            string sql = "SELECT genre_id, name FROM genres " +
                "WHERE name = @name;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", genre);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToGenre(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get genre", $"For {genre}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the genres associated for this record
        /// </summary>
        /// <param name="discogId"></param>
        /// <returns>List of genres. Only the name should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<string> GetGenresByDiscogsId(int discogId)
        {
            List<string> output = new List<string>();
            string sql = "SELECT genres.name " +
                "FROM genres " +
                "JOIN records_genres ON genres.genre_id = records_genres.genre_id " +
                "JOIN records ON records_genres.discogs_id = records.discogs_id " +
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
                        output.Add(Convert.ToString(reader["name"]));
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get genre by id", $"For {discogId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }
        /// <summary>
        /// Returns how many genres are in the entire database.
        /// </summary>
        /// <returns>Int number of genres</returns>
        /// <exception cref="DaoException"></exception>
        public int GetGenreCount()
        {
            int output = 0;

            string sql = "SELECT count(genre_id) AS count " +
                "FROM genres ";
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
                ErrorLog.WriteLog("Trying to get total genre count", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many genres are associated with this user. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns>Int number of genres</returns>
        /// <exception cref="DaoException"></exception>
        public int GetGenreCountByUsername(string username, bool isPremium)
        {
            int output = 0;

            string sql = "SELECT count(genres.genre_id) AS count " +
                "FROM genres " +
                "JOIN records_genres ON genres.genre_id = records_genres.genre_id " +
                "JOIN records ON records_genres.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND is_premium = @isPremium AND is_active = 1 ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
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
                ErrorLog.WriteLog("Trying to get users genre", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns the record count by genre in this user's library. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns>Dictionary of key, genre name, value, count of records</returns>
        /// <exception cref="DaoException"></exception>
        public Dictionary<string, int> GetGenreAndRecordCountByUsername(string username, bool isPremium)
        {
            Dictionary<string, int> output = new Dictionary<string, int>();

            string sql = "SELECT genre.name, count(records.discogs_id) AS record_count " +
                "FROM genres " +
                "JOIN records_genres ON genres.genre_id = records_genres.genre_id " +
                "JOIN records ON records_genres.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND is_premium = @isPremium AND records.is_active = 1 " +
                "GROUP BY genre.name";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output[Convert.ToString(reader["name"])] = Convert.ToInt32(reader["record_count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get record count by genre for user", $"{username} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns the record count by genres in the entire database. Active users only.
        /// </summary>
        /// <returns>Dictionary of key, genre name, value, count of records</returns>
        /// <exception cref="DaoException"></exception>
        public Dictionary<string, int> GetGenreAndRecordCount()
        {
            Dictionary<string, int> output = new Dictionary<string, int>();

            string sql = "SELECT name, count(records.discogs_id) AS record_count " +
                "FROM genres " +
                "JOIN records_genres ON genres.genre_id = records_genres.genre_id " +
                "JOIN records ON records_genres.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE records.is_active = 1 " +
                "GROUP BY name";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output[Convert.ToString(reader["name"])] = Convert.ToInt32(reader["record_count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get record count by genre for whole database", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }


        public bool AddGenre(string genre)
        {
            Genre checkedGenre = GetGenre(genre);

            if (checkedGenre != null)
            {
                return false;
            }
            string sql = "INSERT INTO genres (name) " +
                "OUTPUT INSERTED.genre_id " +
                "VALUES (@name);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", genre);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add genre", $"For {genre}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Exception occurred", ex);
            }
        }


        /// <summary>
        /// Ignore this method for now. Wrote it and don't need it. Might come in handy for pulling information from multiple discogIds at once, in aggregate
        /// </summary>
        /// <param name="discogsIds"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        //public List<string> GetGenreByDiscogsId(List<int> discogsIds)
        //{
        //    List<string> output = new List<string>();

        //    // if the incoming list has nothing in it, return empty list 
        //    if(discogsIds.Count == 0 || discogsIds == null)
        //    {
        //        return output;
        //    }

        //    string sql = "SELECT name " +
        //        "FROM genres " +
        //        "JOIN records_genres ON genres.genre_id = records_genres.genre_id " +
        //        "WHERE discogs_id IN ({0})";

        //    // this seems to be some kind of for loop that makes the number of tags needed
        //    // got this from stack overflow...
        //    string[] paramNames = discogsIds.Select((s, i) => "@tag" + i.ToString()).ToArray();
        //    string inClause = string.Join(", ", paramNames);

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(string.Format(sql, inClause), conn);

        //            // continued stack overflow idea
        //            for(int i = 0; i < paramNames.Length; i++)
        //            {
        //                cmd.Parameters.AddWithValue(paramNames[i], discogsIds[i]);
        //            }
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                output.Add(Convert.ToString(reader["name"]));
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("Sql exception occurred", ex);
        //    }
        //    return output;
        //}

        private Genre MapRowToGenre(SqlDataReader reader)
        {
            Genre genre = new Genre();
            genre.Genre_Id = Convert.ToInt32(reader["genre_id"]);
            genre.Name = Convert.ToString(reader["name"]);
            return genre;

        }
    }
}
