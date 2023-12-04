using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class FormatSqlDao : IFormatsDao
    {
        private readonly string connectionString;
        public FormatSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Checks the database if the format "type" already exists
        /// "type" in the database is a unique field
        /// In the incoming Record object, this is under the Descriptions list
        /// </summary>
        /// <param name="description">In the database, named "type"</param>
        /// <returns>String "type" of format</returns>
        /// <exception cref="DaoException"></exception>
        public string GetFormat(string description)
        {
            string output = null;
            string sql = "SELECT type " +
                "FROM formats " +
                "WHERE type = @type";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@type", description);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = Convert.ToString(reader["type"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }
        public bool AddFormat(string description)
        {
            string checkedArtist = GetFormat(description);

            if (checkedArtist != null)
            {
                return false;
            }
            string sql = "INSERT INTO formats (type) " +
                "OUTPUT INSERTED.format_id " +
                "VALUES (@type);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@type", description);
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
