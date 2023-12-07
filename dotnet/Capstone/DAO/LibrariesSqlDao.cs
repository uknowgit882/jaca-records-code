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
                "WHERE username = @username AND libraries.is_active = 1;";
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
        public bool AddRecord(int discogsId, string username, string notes)
        {
            int libraryId = 0;

            Library output = new Library();

            if (output == null)
            {
                return false;
            }
            else if (output.IsActive == false)
            {
                return false;
            }

            string sql = "INSERT INTO libraries (username, discogs_id, notes, quantity) " +
                "OUTPUT INSERTED.library_id " +
                "VALUES (@username, @discogs_id, @notes, @quantity);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogs_id", discogsId);
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
        public bool RemoveRecord(int discogsId, string username)
        {
            int numberOfRows = 0;

            string userSql = "DELETE FROM library " +
                "WHERE username = @username AND discogs_id = @discogs_id";
                    
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(userSql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogs_id", discogsId);
                    numberOfRows = cmd.ExecuteNonQuery();
                    if (numberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows is impacted");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("RemoveRecord() not implemented", ex);
            }
            return true;
        }

        public string GetNote(string username, int discogsId)
        {
            string output = null;

            string sql = "SELECT notes FROM libraries " +
                "WHERE username = @username AND discogs_id = @discogs_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogs_id", discogsId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = Convert.ToString(reader["notes"]);
                    }

                }
            }
            catch (DaoException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        public string ChangeNote(string username, int discogsId, string notes)
        {
            int numberOfRows = 0;

            string chngNote = null;

            string sql = "UPDATE libraries " +
                "SET notes = @notes, updated_date = @updated_date " +
                "WHERE username = @username AND discogs_id = @discogs_id;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogs_id", discogsId);
                    cmd.Parameters.AddWithValue("@notes", notes);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();

                    if (numberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows is impacted");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            catch (Exception e)
            {
                throw new DaoException("Exception occurred", e);
            }
            return chngNote;
        }

        public int GetQuantity(string username, int discogsId)
        {
            int output = 0;

            string sql = "SELECT quantity FROM libraries " +
                "WHERE username = @username AND discogs_id = @discogs_id;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogs_id", discogsId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = Convert.ToInt32(reader["quantity"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }
        public int ChangeQuantity(string username, int discogsId, int quantity)
        {
            int numberOfRows = 0;

            int chngQuantity = 0;

            int currentQuantity = GetQuantity(username, discogsId);

            if (currentQuantity + quantity < 0)
            {
                throw new DaoException("You can't subtract more than you have");
            }

            string sql = "UPDATE libraries " +
                "SET quantity = @quantity, updated_date = @updated_date " +
                "WHERE username = @username AND discogs_id = @discogs_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogs_id", discogsId);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();

                    if (numberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows is impacted");
                    }

                }
                chngQuantity = GetQuantity(username, discogsId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("Something went wrong", ex);
            }
            catch (Exception e)
            {
                throw new DaoException("Exception occurred", e);
            }
            return chngQuantity;
        }

        public bool DeactivateLibrary(string username)
        {
            int numberOfRows = 0;

            List<Library> libraryNotActive = GetLibrary(username);

            string sql = "UPDATE libraries " +
                "SET is_active = 0, updated_date = @updated_date " +
                "WHERE username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();
                    if (numberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows is impacted");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DaoException("Something went wrong", ex);
            }
            return true;
        }


        public bool ReactivateLibrary(string username)
        {
            int numberOfRows = 0;

            string sql = "UPDATE libraries " +
                "SET is_active = 1, updated_date = @updated_date " +
                "WHERE username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("updated_date", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();
                    if (numberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows is impacted");
                    }
                }
            }
            catch (Exception)
            {
                throw new DaoException("Something went wrong");
            }
            return true;

        }

        private Library MapToLibrary(SqlDataReader reader)
        {
            Library library = new Library();
            library.Library_Id = Convert.ToInt32(reader["library_id"]);
            library.Username = Convert.ToInt32(reader["username"]);
            library.Discog_Id = Convert.ToInt32(reader["discogs_id"]);
            library.Notes = Convert.ToString(reader["notes"]);
            library.Quantity = Convert.ToInt32(reader["quantity"]);
            library.IsActive = Convert.ToBoolean(reader["is_active"]);
            library.Created_Date = Convert.ToDateTime(reader["created_date"]);
            library.Updated_Date = Convert.ToDateTime(reader["updated_date"]);
            return library;
        }

    }
}
