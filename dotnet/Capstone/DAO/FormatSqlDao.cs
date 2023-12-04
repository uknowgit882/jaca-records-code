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
        /// <returns>Format object with ID and type, under "Name"</returns>
        /// <exception cref="DaoException"></exception>
        public Format GetFormat(string description)
        {
            Format output = null;
            string sql = "SELECT format_id, type " +
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
                        output = MapRowToFormat(reader);
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
            Format checkedArtist = GetFormat(description);

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
                    
                    Convert.ToInt32(cmd.ExecuteScalar());

                    return true;
                }
            }
            catch (Exception e)
            {
                throw new DaoException("exception occurred", e);
            }
        }
        private Format MapRowToFormat(SqlDataReader reader)
        {
            Format output = new Format();
            output.Format_Id = Convert.ToInt32(reader["format_id"]);
            output.Name = Convert.ToString(reader["type"]);
            return output;
        }
    }
}
