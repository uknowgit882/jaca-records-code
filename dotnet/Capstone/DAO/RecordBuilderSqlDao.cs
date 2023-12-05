using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Data.SqlClient;

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
            string sql = "SELECT record_id, discogs_id, country, notes, released, title, url, discogs_date_changed, is_active " +
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
                throw new DaoException("Sql exception occured", ex);
            }
            return output;
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
                    cmd.Parameters.AddWithValue("@notes", input.Notes);
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
                throw new DaoException("Sql exception occured", ex);
            }
            return output;
        }
         

        private RecordTableData MapRowToRecordTableData(SqlDataReader reader)
        {
            RecordTableData output = new RecordTableData();
            output.Record_Id = Convert.ToInt32(reader["record_id"]);
            output.Discogs_Id = Convert.ToInt32(reader["discogs_id"]);
            output.Country = Convert.ToString(reader["country"]);
            output.Notes = Convert.ToString(reader["notes"]);
            output.Released = Convert.ToString(reader["released"]);
            output.Title = Convert.ToString(reader["title"]);
            output.URL = Convert.ToString(reader["url"]);
            output.Discogs_Date_Changed = Convert.ToDateTime(reader["discogs_date_changed"]);
            output.Is_Active = Convert.ToBoolean(reader["is_active"]);

            return output;
        }
    }
}
