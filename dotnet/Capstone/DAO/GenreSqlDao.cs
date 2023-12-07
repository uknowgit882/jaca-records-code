using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

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
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the genres associated for this record for this specific user
        /// </summary>
        /// <param name="discogId"></param>
        /// <param name="username"></param>
        /// <returns>List of genres for this specific user. Only the name should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<string> GetGenresByDiscogsIdAndUsername(int discogId, string username)
        {
            List<string> output = new List<string>();
            string sql = "SELECT genres.name " +
                "FROM genres " +
                "JOIN records_genres ON genres.genre_id = records_genres.genre_id " +
                "JOIN records ON records_genres.discogs_id = records.discogs_id " +
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
                        output.Add(Convert.ToString(reader["name"]));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
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
            catch (Exception e)
            {
                throw new DaoException("Exception occurred", e);
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
