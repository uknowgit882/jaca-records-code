using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class FormatSqlDao: IFormatsDao
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

        /// <summary>
        /// Gets the formats associated for this record for this specific user
        /// </summary>
        /// <param name="discogId"></param>
        /// <param name="username"></param>
        /// <returns>List of formats for this specific user. Only the type should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Format> GetFormatsByDiscogsIdAndUsername(int discogId, string username)
        {
            List<Format> output = new List<Format>();
            string sql = "SELECT type " +
                "FROM formats " +
                "JOIN records_formats ON formats.format_id = records_formats.format_id " +
                "JOIN records ON records_formats.discogs_id = records.discogs_id " +
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
                        Format row = new Format();
                        // this will return the search result for the libary back to the front end
                        // with the different format types in the name field
                        row.Name = Convert.ToString(reader["type"]);
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
