using Capstone.Exceptions;
using Capstone.Models;
using System;
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
            string sql = "SELECT label_id, name, url, is_active, created_date, updated_date, FROM labels " +
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
