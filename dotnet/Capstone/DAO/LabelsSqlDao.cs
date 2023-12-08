using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace Capstone.DAO
{
    public class LabelsSqlDao : ILabelsDao
    {
        private readonly string connectionString;
        public LabelsSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Label GetLabel(Label label)
        {
            Label output = null;
            string sql = "SELECT label_id, name, url, is_active, created_date, updated_date FROM labels " +
                "WHERE name = @name";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", label.Name);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToLabel(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get label", $"For {label.Name}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the labels associated for this record
        /// </summary>
        /// <param name="discogId"></param>
        /// <returns>List of labels. Only the name and uri should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Label> GetLabelsByDiscogsId(int discogId)
        {
            List<Label> output = new List<Label>();
            string sql = "SELECT name, labels.url " +
                "FROM labels " +
                "JOIN records_labels ON labels.label_id = records_labels.label_id " +
                "JOIN records ON records_labels.discogs_id = records.discogs_id " +
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
                        Label row = new Label();
                        row.Name = Convert.ToString(reader["name"]);
                        row.Resource_Url = Convert.ToString(reader["url"]);
                        output.Add(row);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get label by id", $"For {discogId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns how many labels are in the entire database.
        /// </summary>
        /// <returns>Int number of labels</returns>
        /// <exception cref="DaoException"></exception>
        public int GetLabelCount()
        {
            int output = 0;

            string sql = "SELECT count(label_id) AS count " +
                "FROM labels ";
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
                ErrorLog.WriteLog("Trying to get label count", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many labels are associated with this user. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Int number of labels</returns>
        /// <exception cref="DaoException"></exception>
        public int GetLabelCountByUsername(string username)
        {
            int output = 0;

            string sql = "SELECT count(labels.label_id) AS count " +
                "FROM labels " +
                "JOIN records_labels ON labels.label_id = records_labels.label_id " +
                "JOIN records ON records_labels.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND is_active = 1 ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
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
                ErrorLog.WriteLog("Trying to get label count for user", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns the record count by label in this user's library. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Dictionary of key, label name, value, count of records</returns>
        /// <exception cref="DaoException"></exception>
        public Dictionary<string, int> GetLabelAndRecordCountByUsername(string username)
        {
            Dictionary<string, int> output = new Dictionary<string, int>();

            string sql = "SELECT label.name, count(records.discogs_id) AS record_count " +
                "FROM labels " +
                "JOIN records_labels ON labels.label_id = records_labels.label_id " +
                "JOIN records ON records_labels.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND records.is_active = 1 " +
                "GROUP BY label.name";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output[Convert.ToString(reader["name"])] = Convert.ToInt32(reader["record_count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get record count by label for user", $"{username} get failed", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns the record count by genres in the entire database. Active users only.
        /// </summary>
        /// <returns>Dictionary of key, genre name, value, count of records</returns>
        /// <exception cref="DaoException"></exception>
        public Dictionary<string, int> GetGenreAndRecordCount()
        {
            Dictionary<string, int> output = new Dictionary<string, int>();

            string sql = "SELECT label.name, count(records.discogs_id) AS record_count " +
                "FROM labels " +
                "JOIN records_labels ON labels.label_id = records_labels.label_id " +
                "JOIN records ON records_labels.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE records.is_active = 1 " +
                "GROUP BY label.name"; 

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output[Convert.ToString(reader["name"])] = Convert.ToInt32(reader["record_count"]);
                    }
                    return output;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get record count by label for whole database", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        // don't think I need this...
        ///// <summary>
        ///// Gets the labels associated for this record for this specific user
        ///// </summary>
        ///// <param name="discogId"></param>
        ///// <param name="username"></param>
        ///// <returns>List of labels for this specific user. Only the name and uri should be sent to the front end - use JSONIgnore on the other properties</returns>
        ///// <exception cref="DaoException"></exception>
        //public List<Label> GetLabelsByDiscogsIdAndUsername(int discogId, string username)
        //{
        //    List<Label> output = new List<Label>();
        //    string sql = "SELECT name, labels.url " +
        //        "FROM labels " +
        //        "JOIN records_labels ON labels.label_id = records_labels.label_id " +
        //        "JOIN records ON records_labels.discogs_id = records.discogs_id " +
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
        //                Label row = new Label();
        //                row.Name = Convert.ToString(reader["name"]);
        //                row.Resource_Url = Convert.ToString(reader["url"]);
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

        public bool AddLabel(Label label)
        {
            Label checkedLabel = GetLabel(label);

            if (checkedLabel != null)
            {
                return false;
            }
            string sql = "INSERT INTO labels (name, url) " +
                "OUTPUT INSERTED.label_id " +
                "VALUES (@name, @url);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", label.Name);
                    cmd.Parameters.AddWithValue("@url", label.Resource_Url);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add label", $"For {label.Name}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }

        public Label UpdateLabel(Label updatedLabel)
        {
            Label output = null;

            string sql = "UPDATE labels " +
                "SET url = @url, updated_date = @updatedDate " +
                "WHERE name = @name";


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@url", updatedLabel.Resource_Url);
                    cmd.Parameters.AddWithValue("@updatedDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@name", updatedLabel.Name);
                    
                    int numberOfRowsAffected = cmd.ExecuteNonQuery();

                    if(numberOfRowsAffected != 1)
                    {
                        throw new DaoException("The wrong number of rows were affected");
                    }
                }
                return GetLabel(updatedLabel);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to update label", $"For {updatedLabel.Name}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }

        private Label MapRowToLabel(SqlDataReader reader)
        {
            Label label = new Label();
            label.Label_Id = Convert.ToInt32(reader["label_id"]);
            label.Name = Convert.ToString(reader["name"]);
            label.Resource_Url = Convert.ToString(reader["url"]);
            return label;
        }
    }
}
