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

        public Genre GetGenre(Genre genre)
        {
            Genre output = null;
            string sql = "SELECT genre_id, name, is_active, created_date, updated_date FROM genres " +
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
        public bool AddGenre(Genre genre)
        {
            Genre checkedGenre = GetGenre(genre);

            if (checkedGenre != null)
            {
                return false;
            }
            string sql = "INSERT INTO genres (name, is_active) " +
                "OUTPUT INSERTED.genre_id " +
                "VALUES (@name, @is_active);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", genre.Name);
                    cmd.Parameters.AddWithValue("@is_active", genre.IsActive);
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
            genre.IsActive = Convert.ToBoolean(reader["is_active"]);
            genre.Created_Date = Convert.ToDateTime(reader["created_date"]);
            genre.Updated_Date = Convert.ToDateTime(reader["updated_date"]);
            return genre;

        }
    }
}
