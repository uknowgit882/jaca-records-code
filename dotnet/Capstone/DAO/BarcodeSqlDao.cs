using Capstone.Exceptions;
using Capstone.Models;
using System.Data.SqlClient;
using System;
using Capstone.DAO.Interfaces;
using System.Collections.Generic;
using Capstone.Utils;
using System.Reflection;

namespace Capstone.DAO
{
    public class BarcodeSqlDao : IBarcodesDao
    {
        // barcodes are "identifiers" in the discogs api
        private readonly string connectionString;
        public BarcodeSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Identifier GetIdentifier(Identifier identifier)
        {
            Identifier output = null;
            string sql = "SELECT barcode_id, discogs_id, type, value, description " +
                "FROM barcodes " +
                "WHERE discogs_id = @discogsId AND value = @value ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", identifier.Discogs_Id);
                    cmd.Parameters.AddWithValue("@value", identifier.Value);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToIdentifier(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get barcode", $"{identifier.Discogs_Id} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }

        /// <summary>
        /// Gets the barcodes and identifiers associated for this record. Identifiers can include barcodes and record etchings/codes
        /// </summary>
        /// <param name="discogId"></param>
        /// <returns>List of barcodes and identifiers. Only the type and value should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Identifier> GetIdentifiersByDiscogsId(int discogId)
        {
            List<Identifier> output = new List<Identifier>();
            string sql = "SELECT type, value " +
                "FROM barcodes " +
                "JOIN records ON barcodes.discogs_id = records.discogs_id " +
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
                        Identifier row = new Identifier();
                        row.Type = Convert.ToString(reader["type"]);
                        row.Value = Convert.ToString(reader["value"]);
                        output.Add(row);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get barcode", $"{discogId} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns how many barcodes are in the entire database.
        /// </summary>
        /// <returns>Int number of barcodes</returns>
        /// <exception cref="DaoException"></exception>
        public int GetBarcodesCount()
        {
            int output = 0;

            string sql = "SELECT count(barcode_id) AS count " +
                "FROM barcodes ";
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
                ErrorLog.WriteLog("Trying to get all barcode count", $"Failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        // I think I don't need this...
        ///// <summary>
        ///// Gets the barcodes and identifiers associated for this record for this specific user. Identifiers can include barcodes and record etchings/codes
        ///// </summary>
        ///// <param name="discogId"></param>
        ///// <param name="username"></param>
        ///// <returns>List of barcodes and identifiers for this specific user. Only the type and value should be sent to the front end - use JSONIgnore on the other properties</returns>
        ///// <exception cref="DaoException"></exception>
        //public List<Identifier> GetIdentifiersByDiscogsIdAndUsername(int discogId, string username)
        //{
        //    List<Identifier> output = new List<Identifier>();
        //    string sql = "SELECT type, value " +
        //        "FROM barcodes " +
        //        "JOIN records ON barcodes.discogs_id = records.discogs_id " +
        //        "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
        //        "WHERE records.discogs_id = @discogId AND username = @username";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@discogId", discogId);
        //            cmd.Parameters.AddWithValue("@username", username);
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                Identifier row = new Identifier();
        //                row.Type = Convert.ToString(reader["type"]);
        //                row.Value = Convert.ToString(reader["value"]);
        //                output.Add(row);
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("Sql exception occurred", ex);
        //    }
        //    return output;
        //}

        public bool AddIdentifier(Identifier identifier)
        {
            Identifier checkedIdentifier = GetIdentifier(identifier);

            if (checkedIdentifier != null)
            {
                return false;
            }
            string sql = "INSERT INTO barcodes (discogs_id, type, value, description) " +
                "OUTPUT INSERTED.barcode_id " +
                "VALUES (@discogsId, @type, @value, @description);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", identifier.Discogs_Id);
                    cmd.Parameters.AddWithValue("@type", identifier.Type);
                    cmd.Parameters.AddWithValue("@value", identifier.Value);
                    cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(identifier.Description) ? "" : identifier.Description);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add barcode", $"{identifier.Discogs_Id} add failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }

        public Identifier UpdateIdentifier(Identifier updatedIdentifier)
        {
            string sql = "UPDATE barcodes " +
                "SET type = @type, description = @description, updated_date = @updatedDate " +
                "WHERE value = @value AND discogs_id = @discogsId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@type", updatedIdentifier.Type);
                    cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(updatedIdentifier.Description) ? "" : updatedIdentifier.Description);
                    cmd.Parameters.AddWithValue("@updatedDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@value", updatedIdentifier.Value);
                    cmd.Parameters.AddWithValue("@discogsId", updatedIdentifier.Discogs_Id);
                    int numberOfRowsAffected = cmd.ExecuteNonQuery();
                }
                return GetIdentifier(updatedIdentifier);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to update barcode", $"{updatedIdentifier.Discogs_Id} update failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }

        private Identifier MapRowToIdentifier(SqlDataReader reader)
        {
            Identifier output = new Identifier();
            output.Barcode_Id = Convert.ToInt32(reader["barcode_id"]);
            output.Discogs_Id = Convert.ToInt32(reader["discogs_id"]);
            output.Type = Convert.ToString(reader["type"]);
            output.Value = Convert.ToString(reader["value"]);
            output.Description = Convert.ToString(reader["description"]);
            return output;
        }
    }
}
