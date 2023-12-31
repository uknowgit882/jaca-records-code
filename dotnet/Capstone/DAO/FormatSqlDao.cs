﻿using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

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
                ErrorLog.WriteLog("Trying to get format", $"For {description}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }

        /// <summary>
        /// Gets the formats associated for this record
        /// </summary>
        /// <param name="discogId"></param>
        /// <returns>List of formats. Only the type should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Format> GetFormatsByDiscogsId(int discogId)
        {
            List<Format> output = new List<Format>();
            string sql = "SELECT type " +
                "FROM formats " +
                "JOIN records_formats ON formats.format_id = records_formats.format_id " +
                "JOIN records ON records_formats.discogs_id = records.discogs_id " +
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
                ErrorLog.WriteLog("Trying to get format by discogsId", $"For {discogId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns how many formats are in the entire database.
        /// </summary>
        /// <returns>Int number of formats</returns>
        /// <exception cref="DaoException"></exception>
        public int GetFormatCount()
        {
            int output = 0;

            string sql = "SELECT count(format_id) AS count " +
                "FROM formats ";
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
                ErrorLog.WriteLog("Trying to get format count", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many formats are associated with this user. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns>Int number of formats</returns>
        /// <exception cref="DaoException"></exception>
        public int GetFormatCountByUsername(string username, bool isPremium)
        {
            List<int> output = new List<int>();

            string sql = "SELECT count(type) AS count " +
                "FROM formats " +
                "JOIN records_formats ON formats.format_id = records_formats.format_id " +
                "JOIN records ON records_formats.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND is_premium = @isPremium AND formats.is_active = 1 " +
                "GROUP BY formats.type";
//            select count(type) AS count
//from formats
//JOIN records_formats ON formats.format_id = records_formats.format_id
//                JOIN records ON records_formats.discogs_id = records.discogs_id
//                JOIN libraries ON records.discogs_id = libraries.discogs_id
//                WHERE username = 'jakel' AND is_premium = 1 AND formats.is_active = 1

//                group by formats.type

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
                        output.Add(Convert.ToInt32(reader["count"]));
                    }
                    return output.Count;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get format count", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns the record count by formats type in this user's library. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns>Dictionary of key, format type, value, count of records</returns>
        /// <exception cref="DaoException"></exception>
        public Dictionary<string, int> GetFormatAndRecordCountByUsername(string username, bool isPremium)
        {
            Dictionary<string, int> output = new Dictionary<string, int>();

            string sql = "SELECT formats.type, count(records.discogs_id) AS record_count " +
                "FROM formats " +
                "JOIN records_formats ON formats.format_id = records_formats.format_id " +
                "JOIN records ON records_formats.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND is_premium = @isPremium AND records.is_active = 1 " +
                "GROUP BY formats.type";
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
                        output[Convert.ToString(reader["type"])] = Convert.ToInt32(reader["record_count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get record count by type for user", $"{username} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns the record count by format type in the entire database. Active users only.
        /// </summary>
        /// <returns>Dictionary of key, format type, value, count of records</returns>
        /// <exception cref="DaoException"></exception>
        public Dictionary<string, int> GetFormatAndRecordCount()
        {
            Dictionary<string, int> output = new Dictionary<string, int>();

            string sql = "SELECT type, count(records.discogs_id) AS record_count " +
                "FROM formats " +
                "JOIN records_formats ON formats.format_id = records_formats.format_id " +
                "JOIN records ON records_formats.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE records.is_active = 1 " +
                "GROUP BY type";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output[Convert.ToString(reader["type"])] = Convert.ToInt32(reader["record_count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get record count by format for whole database", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
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
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add format", $"For {description}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
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
