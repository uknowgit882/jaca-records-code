using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Data.SqlClient;
using System.Globalization;

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

        private Genre MapRowToGenre(SqlDataReader reader)
        {
            Genre genre = new Genre();
            genre.Genre_Id = Convert.ToInt32(reader["genre_id"]);
            genre.Name = Convert.ToString(reader["name"]);
            return genre;

        }
    }
}
