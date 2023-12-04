using Capstone.Exceptions;
using Capstone.Models;
using System.Data.SqlClient;
using System;

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
            string sql = "SELECT barcode_id, record_id, type, value, description " +
                "FROM barcodes " +
                "WHERE record_id = @recordId AND type = @type AND value = @value AND description = @description";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recordId", identifier.Record_Id);
                    cmd.Parameters.AddWithValue("@type", identifier.Type);
                    cmd.Parameters.AddWithValue("@value", identifier.Value);
                    cmd.Parameters.AddWithValue("@description", identifier.Description);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToIdentifier(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }
        public bool AddIdentifier(Identifier identifier)
        {
            Identifier checkedIdentifier = GetIdentifier(identifier);

            if (checkedIdentifier != null)
            {
                return false;
            }
            string sql = "INSERT INTO barcodes (record_id, type, value, description) " +
                "OUTPUT INSERTED.barcode_id " +
                "VALUES (@recordId, @type, @value, @description);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recordId", identifier.Record_Id);
                    cmd.Parameters.AddWithValue("@type", identifier.Type);
                    cmd.Parameters.AddWithValue("@value", identifier.Value);
                    cmd.Parameters.AddWithValue("@description", identifier.Description);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new DaoException("exception occurred", e);
            }
        }

        private Identifier MapRowToIdentifier(SqlDataReader reader)
        {
            Identifier output = new Identifier();
            output.Barcode_Id = Convert.ToInt32(reader["barcode_id"]);
            output.Record_Id = Convert.ToInt32(reader["record_id"]);
            output.Type = Convert.ToString(reader["type"]);
            output.Value = Convert.ToString(reader["value"]);
            output.Description = Convert.ToString(reader["description"]);
            return output;
        }
    }
}
