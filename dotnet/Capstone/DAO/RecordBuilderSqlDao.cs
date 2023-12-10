using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Capstone.DAO
{
    public class RecordBuilderSqlDao : IRecordBuilderDao
    {
        private readonly string connectionString;
        public RecordBuilderSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public RecordTableData GetRecordByDiscogsId(int discogsId)
        {
            RecordTableData output = null;
            string sql = "SELECT record_id, discogs_id, title, released, country, notes, url, discogs_date_changed, is_active " +
                "FROM records " +
                "WHERE discogs_id = @discogsId";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToRecordTableData(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Getting RecordTableData from database", $"Database Input: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }
            return output;
        }
        
        public RecordTableData GetRecordByRecordId(int recordId)
        {
            RecordTableData output = null;
            string sql = "SELECT record_id, discogs_id, country, notes, released, title, url, discogs_date_changed, is_active " +
                "FROM records " +
                "WHERE record_id = @recordId";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recordId", recordId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToRecordTableData(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Getting RecordTableData from database", $"Database Input: {recordId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns how many records are in the entire database.
        /// </summary>
        /// <returns>Int number of records</returns>
        /// <exception cref="DaoException"></exception>
        public int GetRecordCount()
        {
            int output = 0;

            string sql = "SELECT count(discogs_id) AS count " +
                "FROM records ";
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
                ErrorLog.WriteLog("Getting all records from database", $"Simple query", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns the record count by year in the database. Active users only.
        /// </summary>
        /// <returns>Dictionary of key, year, value, count of records</returns>
        /// <exception cref="DaoException"></exception>
        public Dictionary<string, int> GetYearAndRecordCount()
        {
            Dictionary<string, int> output = new Dictionary<string, int>();

            string sql = "SELECT substring(released, 1, 4) AS year_released, count (discogs_id) AS record_count " +
                "FROM records " +
                "GROUP BY released " +
                "ORDER by year_released DESC";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string yearReleasedOutput = Convert.ToString(reader["year_released"]);
                        int recordCount = Convert.ToInt32(reader["record_count"]);
                        if (output.ContainsKey(yearReleasedOutput))
                        {
                            output[yearReleasedOutput] = output[yearReleasedOutput] + recordCount;
                        }
                        else
                        {
                            output.Add(yearReleasedOutput, recordCount);
                        }
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get record count by year from database", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns the record count by country in the database. Active users only.
        /// </summary>
        /// <returns>Dictionary of key, country, value, count of records</returns>
        /// <exception cref="DaoException"></exception>
        public Dictionary<string, int> GetCountryAndRecordCount()
        {
            Dictionary<string, int> output = new Dictionary<string, int>();

            string sql = "SELECT country, count (discogs_id) AS record_count " +
                "FROM records " +
                "GROUP BY country " +
                "ORDER by country";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output[Convert.ToString(reader["country"])] = Convert.ToInt32(reader["record_count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get record count by country from database", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Adds a record to the Records table. Marked inactive by default, intentionally.
        /// Should only be activated once all dependent tables have their information loaded
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Inactivated record</returns>
        /// <exception cref="DaoException"></exception>
        public RecordTableData AddRecord(RecordClient input)
        {
            RecordTableData output = null;

            string sql = "INSERT INTO records (discogs_id, country, notes, released, title, url, discogs_date_changed, is_active) " +
                "OUTPUT INSERTED.record_id " +
                "VALUES(@discogsId, @country, @notes, @released, @title, @url, @discogsDateChanged, @isActive)";

            int newRecordId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", input.Id);
                    cmd.Parameters.AddWithValue("@country", input.Country);
                    cmd.Parameters.AddWithValue("@notes", string.IsNullOrEmpty(input.Notes) ? DBNull.Value : input.Notes);
                    cmd.Parameters.AddWithValue("@released", input.Released);
                    cmd.Parameters.AddWithValue("@title", input.Title);
                    cmd.Parameters.AddWithValue("@url", input.URI);
                    cmd.Parameters.AddWithValue("@discogsDateChanged", input.Date_Changed);
                    // default creation of record is false
                    // only changed to active when record build is complete
                    cmd.Parameters.AddWithValue("@isActive", 0);
                    
                    newRecordId = Convert.ToInt32(cmd.ExecuteScalar());

                }

                output = GetRecordByRecordId(newRecordId);

            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Adding record to database - just record table", $"{input.Id} - New record object from API", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }
            return output;
        }
        /// <summary>
        /// Used to activate the record only when the build is fully complete
        /// </summary>
        /// <param name="discogsId"></param>
        /// <returns>Fully record table's information (no joins)</returns>
        public RecordTableData ActivateRecord(int discogsId)
        {
            RecordTableData output = null;

            string sql = "UPDATE records " +
                "SET is_active = 1 " +
                "WHERE discogs_id = @discogsId";

            int numberOfRowsAffected = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    
                    numberOfRowsAffected = cmd.ExecuteNonQuery();

                    if (numberOfRowsAffected != 1)
                    {
                        throw new DaoException("The wrong number of rows were affected");
                    }
                }
                output = GetRecordByDiscogsId(discogsId);
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Activating record when full load successfully complete", $"{discogsId} - Updating record is_active status to true", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }
            return output;
        }

        /// <summary>
        /// Calling method will have checked if the database's discogs update date is different to the incoming discogs update date - if true, will call this method.
        /// This method checks if it exists. If not, add it. If yes, assume everything needs to be updated.
        /// If there is a join table, it will check the associations are there. If not, it will add associations
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public RecordTableData UpdateRecord(RecordClient input)
        {
            // double check it exists (although should always exist if you're calling this method)
            RecordTableData existenceCheck = GetRecordByDiscogsId(input.Id);
            if(existenceCheck == null)
            {
                // if it doesn't exist, try and create it and send it back
                return AddRecord(input);
            }
            
            RecordTableData output = null;

            string sql = "UPDATE records " +
                "SET country = @country, notes = @notes, released = @released, title = @title, url = @url, updated_date = @updatedDate " +
                "WHERE discogs_id = @discogsId";

            int numberOfRowsAffected = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // do the updates
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@country", input.Country);
                    cmd.Parameters.AddWithValue("@notes", input.Notes);
                    cmd.Parameters.AddWithValue("@released", input.Released);
                    cmd.Parameters.AddWithValue("@title", input.Title);
                    cmd.Parameters.AddWithValue("@url", input.URI);
                    cmd.Parameters.AddWithValue("@updatedDate", DateTime.UtcNow);

                    cmd.Parameters.AddWithValue("@discogsId", input.Id);

                    numberOfRowsAffected = cmd.ExecuteNonQuery();

                    if(numberOfRowsAffected != 1)
                    {
                        throw new DaoException("The wrong number of rows were affected");
                    }
                }
                // then get the updated record and sent it back
                return GetRecordByDiscogsId(input.Id);

            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Updating record", $"{input.Id} - When details may have changed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }
        }
        public RecordTableData UpdateRecordDiscogsDateChanged(int discogsId, DateTime discogsDateChanged)
        {
            RecordTableData output = null;

            string sql = "UPDATE records " +
                "SET discogs_date_changed = @discogsDateChanged " +
                "WHERE discogs_id = @discogsId";

            int numberOfRowsAffected = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@discogsDateChanged", discogsDateChanged);

                    numberOfRowsAffected = cmd.ExecuteNonQuery();

                    if (numberOfRowsAffected != 1)
                    {
                        throw new DaoException("The wrong number of rows were affected");
                    }
                }
                output = GetRecordByDiscogsId(discogsId);
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Updating when all other updates complete", $"{discogsId} - Updating discogs_date_changed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }
            return output;
        }
        private RecordTableData MapRowToRecordTableData(SqlDataReader reader)
        {
            RecordTableData output = new RecordTableData();
            output.Record_Id = Convert.ToInt32(reader["record_id"]);
            output.Discogs_Id = Convert.ToInt32(reader["discogs_id"]);
            output.Title = Convert.ToString(reader["title"]);
            output.Released = Convert.ToString(reader["released"]);
            output.Country = SqlUtil.NullableString(reader["country"]);
            output.Notes = SqlUtil.NullableString(reader["notes"]);
            output.URL = SqlUtil.NullableString(reader["url"]);
            output.Discogs_Date_Changed = Convert.ToDateTime(reader["discogs_date_changed"]);
            output.Is_Active = Convert.ToBoolean(reader["is_active"]);

            return output;
        }
    }
}
