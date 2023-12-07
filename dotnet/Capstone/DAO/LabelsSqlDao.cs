using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
                throw new DaoException("Sql exception occured", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the labels associated for this record for this specific user
        /// </summary>
        /// <param name="discogId"></param>
        /// <param name="username"></param>
        /// <returns>List of labels for this specific user. Only the name and uri should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Label> GetLabelsByDiscogsIdAndUsername(int discogId, string username)
        {
            List<Label> output = new List<Label>();
            string sql = "SELECT name, labels.url " +
                "FROM labels " +
                "JOIN records_labels ON labels.label_id = records_labels.label_id " +
                "JOIN records ON records_labels.discogs_id = records.discogs_id " +
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
                        Label row = new Label();
                        row.Name = Convert.ToString(reader["name"]);
                        row.Resource_Url = Convert.ToString(reader["url"]);
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
            catch (Exception e)
            {
                throw new DaoException("exception occurred", e);
            }
        }

        private Label MapRowToLabel(SqlDataReader reader)
        {
            Label label = new Label();
            label.Label_Id = Convert.ToInt32(reader["label_id"]);
            label.Name = Convert.ToString(reader["name"]);
            return label;
        }
    }
}
