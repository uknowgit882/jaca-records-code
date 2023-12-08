using Capstone.Models;
using System.Data.SqlClient;
using System;
using Capstone.Exceptions;
using System.Collections.Generic;
using Capstone.DAO.Interfaces;
using Capstone.Utils;
using System.Reflection.Emit;
using System.Reflection;

namespace Capstone.DAO
{
    public class LibrariesSqlDao : ILibrariesDao
    {
        private readonly string connectionString;

        public LibrariesSqlDao(string dbconnectionString)
        {
            connectionString = dbconnectionString;
        }

        /// <summary>
        /// Gets the user's entire library and all discogsIds within. Active libraries only.
        /// For premium users.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>List of all library rows. Will return greater than 25 (if library has that many) - should only be used for premium users. Active libraries only.</returns>
        /// <exception cref="DaoException"></exception>
        public List<Library> GetPremiumUsersLibrary(string username)
        {
            List<Library> output = new List<Library>();

            string sql = "SELECT library_id, username, discogs_id, notes, quantity, is_premium, is_active, created_date, updated_date " +
                "FROM libraries " +
                "WHERE username = @username AND is_active = 1;";
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
                ErrorLog.WriteLog("Trying to get library", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets a free user's is_premium = false library and discogsIds. Active libraries only.
        /// For free users.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>List of max 25 library rows. Should not return greater than 25 rows (if library has that many) - should only be used for free users. Active libraries only.</returns>
        /// <exception cref="DaoException"></exception>
        public List<Library> GetFreeUsersLibrary(string username)
        {
            List<Library> output = new List<Library>();

            string sql = "SELECT library_id, username, discogs_id, notes, quantity, is_premium, is_active, created_date, updated_date " +
                "FROM libraries " +
                "WHERE username = @username AND is_active = 1 AND is_premium = 0;";
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
                    if (output.Count > 25)
                    {
                        throw new UserInfoRetrievalErrorException("The wrong number of rows were retrieved - a free user should not have greater than 25 records in their library");
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get library", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the user's 25 most recently created discogsIds in their library. Active libraries only.
        /// Pulls whether the user is/was premium - all records returned, even if they currently won't be able to see them.
        /// Should only be used when downgraded a user from premium
        /// </summary>
        /// <param name="username"></param>
        /// <returns>List of user's 25 most recently created library rows. Active libraries only.</returns>
        /// <exception cref="DaoException"></exception>
        public List<Library> Get25MostRecentDiscogsIdsInLibrary(string username)
        {
            List<Library> output = new List<Library>();

            string sql = "SELECT TOP 25 library_id, username, discogs_id, notes, quantity, is_premium, is_active, created_date, updated_date " +
                "FROM libraries " +
                "WHERE username = @username AND is_active = 1" +
                "ORDER BY created_date DESC;";
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
                ErrorLog.WriteLog("Trying to get library top 25", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns the note (only) for the record specified in this user's library
        /// </summary>
        /// <param name="username"></param>
        /// <param name="discogsId"></param>
        /// <returns>String note</returns>
        /// <exception cref="DaoException"></exception>
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
                ErrorLog.WriteLog("Trying to get note", $"For {username}, record: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns how many of this particular record (i.e. duplicate copies) are in the user's library
        /// </summary>
        /// <param name="username"></param>
        /// <param name="discogsId"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
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
                ErrorLog.WriteLog("Trying to get quantity of records in library", $"For {username}, record: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns how many records are in the user's library. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Int number of records</returns>
        /// <exception cref="DaoException"></exception>
        public int GetRecordCountByUsername(string username)
        {
            int output = 0;

            string sql = "SELECT username, count(username) AS count " +
                "FROM libraries " +
                "WHERE username = @username AND is_active = 1 " +
                "GROUP BY username;";
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
                ErrorLog.WriteLog("Trying to get library record count", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many records are in the entire database for all active users.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Int number of records</returns>
        /// <exception cref="DaoException"></exception>
        public int GetRecordCountForAllUsers()
        {
            int output = 0;

            string sql = "SELECT count(username) AS count " +
                "FROM libraries " +
                "WHERE is_active = 1;";

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
                ErrorLog.WriteLog("Trying to get library count", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Before calling this method, you need to call the GetRecordCountByUsername method. Then you need to get the user's role. 
        /// If the user's role is free, you should NOT use this method if their record count is greater than 25.
        /// Should also call the "ChangeRecordIsPremium" after this method is called, too - set the status to match the role.
        /// </summary>
        /// <param name="discogsId"></param>
        /// <param name="username"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool AddRecord(int discogsId, string username, string notes)
        {
            int libraryId = 0;

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
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add to library", $"For {username}, record: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Exception occurred", ex);
            }
        }

        /// <summary>
        /// Toggles a record from isPremium true to false and vice versa. Ideally called in a for/foreach loop. Used when down/upgrading an account.
        /// </summary>
        /// <param name="discogsId"></param>
        /// <param name="username"></param>
        /// <param name="toggleIsPremium"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool ChangeRecordIsPremium(int discogsId, string username, bool toggleIsPremium)
        {
            int recordsAffected = 0;

            string sql = "UPDATE libraries " +
                "SET is_premium = @toggleIsPremium, updated_date = @updated_date " +
                "WHERE username = @username AND discogs_id = @discogsId;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@toggleIsPremium", toggleIsPremium);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogs_id", discogsId);

                    recordsAffected = cmd.ExecuteNonQuery();

                    if (recordsAffected != 0)
                    {
                        throw new DaoException("The wrong number of rows were affected");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to change record in library to free/premium", $"For {username}, record: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Exception occurred", ex);
            }
        }

        /// <summary>
        /// Changes a specific record's note in this user's library.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="discogsId"></param>
        /// <param name="notes"></param>
        /// <returns>Returns the changed note</returns>
        /// <exception cref="DaoException"></exception>
        public string ChangeNote(string username, int discogsId, string newNotes)
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
                    cmd.Parameters.AddWithValue("@notes", newNotes);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();

                    if (numberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
                return GetNote(username, discogsId);
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to change note in library", $"For {username}, record: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to change note in library", $"For {username}, record: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Exception occurred", ex);
            }
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
                ErrorLog.WriteLog("Trying to change record quantity library", $"For {username}, record: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to change record quantity library", $"For {username}, record: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Exception occurred", ex);
            }
            return chngQuantity;
        }

        /// <summary>
        /// Removes a record from the user's library permanently. Does not remove it from the records table.
        /// Should only be used after the user's record has been removd from records collections
        /// </summary>
        /// <param name="discogsId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool DeleteRecord(int discogsId, string username)
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
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                    return true;
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to delete record from library", $"For {username}, record: {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong deleting a record", ex);
            }
        }

        /// <summary>
        /// De/Reactivates (but not un/deletes) a user's library when they de/reactivate their profile.
        /// Separate from when they down/upgrade
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool DeReactivateLibrary(string username, bool isActive)
        {
            int numberOfRows = 0;

            string sql = "UPDATE libraries " +
                "SET is_active = @isActive, updated_date = @updated_date " +
                "WHERE username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@isActive", isActive);
                    numberOfRows = cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to de/reactivate library", $"For {username}, action: {isActive}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
        }


        private Library MapToLibrary(SqlDataReader reader)
        {
            Library library = new Library();
            library.Library_Id = Convert.ToInt32(reader["library_id"]);
            library.Username = Convert.ToInt32(reader["username"]);
            library.Discog_Id = Convert.ToInt32(reader["discogs_id"]);
            library.Notes = Convert.ToString(reader["notes"]);
            library.Quantity = Convert.ToInt32(reader["quantity"]);
            library.Is_Premium = Convert.ToBoolean(reader["is_premium"]);
            library.IsActive = Convert.ToBoolean(reader["is_active"]);
            library.Created_Date = Convert.ToDateTime(reader["created_date"]);
            library.Updated_Date = Convert.ToDateTime(reader["updated_date"]);
            return library;
        }

    }
}
