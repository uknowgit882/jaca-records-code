using Capstone.Models;
using System.Data.SqlClient;
using System;
using Capstone.Exceptions;
using System.Collections.Generic;
using Capstone.DAO.Interfaces;

namespace Capstone.DAO
{
    public class LibrariesSqlDao : ILibrariesDao
    {
        private readonly string connectionString;

        public LibrariesSqlDao(string dbconnectionString)
        {
            connectionString = dbconnectionString;
        }
        public List<Library> GetLibrary(string username)
        {
            List<Library> output = new List<Library>();

            string sql = "SELECT library_id, username, discogs_id, notes, quantity, is_active, created_date, updated_date From libraries " +
                "WHERE username = @username;";
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
                        output.Add(MapToLibrary(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }
        public bool AddRecord(int discogId, string username, string notes)
        {
            int libraryId = 0;

            string sql = "INSERT INTO libraries (username, discog_id, notes, quantity) " +
                "OUTPUTED INSERTED.library_id " +
                "VALUES (@username, @discog_id, @notes, @quantity);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discog_id", discogId);
                    cmd.Parameters.AddWithValue("@notes", notes);
                    cmd.Parameters.AddWithValue("@quantity", 1);

                    libraryId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                throw new DaoException("Exception occurred", e);
            }
            return true;
        }
        public bool RemoveRecord(string library)
        {
            throw new System.NotImplementedException();
        }

        public bool ChangeNote(string library)
        {
            throw new System.NotImplementedException();
        }

        public bool ChangeQuantity(string library)
        {
            throw new System.NotImplementedException();
        }

        public bool DeactivateLibrary(string library)
        {
            throw new System.NotImplementedException();
        }


        public bool ReactivateLibrary(string library)
        {
            throw new System.NotImplementedException();
        }

        private Library MapToLibrary(SqlDataReader reader)
        {
            Library library = new Library();
            library.Library_Id = Convert.ToInt32(reader["library_id"]);
            library.Username = Convert.ToInt32(reader["user_id"]);
            library.Discog_Id = Convert.ToInt32(reader["record_id"]);
            library.Notes = Convert.ToString(reader["notes"]);
            library.Quantity = Convert.ToInt32(reader["quantity"]);
            library.IsActive = Convert.ToBoolean(reader["is_active"]);
            library.Created_Date = Convert.ToDateTime(reader["created_date"]);
            library.Updated_Date = Convert.ToDateTime(reader["updated_date"]);
            return library;
        }

    }
}
