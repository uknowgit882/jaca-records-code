using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.Security.Models;
using Capstone.Utils;

namespace Capstone.DAO
{
    public class UserSqlDao : IUserDao
    {
        private readonly string connectionString;

        public UserSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<User> GetUsers()
        {
            IList<User> users = new List<User>();

            string sql = "SELECT user_id, username, first_name, last_name, email_address, password_hash, salt, user_role, is_active, created_date, updated_date, last_login " +
                "FROM users";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = MapRowToUser(reader);
                        users.Add(user);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get all users", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("SQL exception occurred", ex);
            }

            return users;
        }

        public User GetUserById(int userId)
        {
            User user = null;

            string sql = "SELECT user_id, username, first_name, last_name, email_address, password_hash, salt, user_role, is_active, created_date, updated_date, last_login " +
                "FROM users WHERE user_id = @user_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = MapRowToUser(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get user by id", $"For {userId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("SQL exception occurred", ex);
            }

            return user;
        }

        public User GetUserByUsername(string username)
        {
            User user = null;

            string sql = "SELECT user_id, username, first_name, last_name, email_address, password_hash, salt, user_role, is_active, created_date, updated_date, last_login " +
                "FROM users WHERE username = @username";

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
                        user = MapRowToUser(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get user by username", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("SQL exception occurred", ex);
            }

            return user;
        }

        public string GetUserRole(string username)
        {
            string output = "";

            string sql = "SELECT user_role " +
                "FROM users " +
                "WHERE username = @username";

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
                        output = Convert.ToString(reader["user_role"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get user role by username", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("SQL exception occurred", ex);
            }

            return output;
        }

        /// <summary>
        /// Returns how many users are in the entire database.
        /// </summary>
        /// <returns>Int number of users</returns>
        /// <exception cref="DaoException"></exception>
        public int GetUserCount()
        {
            int output = 0;

            string sql = "SELECT count(user_id) AS count " +
                "FROM users ";
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
                ErrorLog.WriteLog("Trying to get user count", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        public User CreateUser(RegisterUser userParam)
        {
            User newUser = null;

            IPasswordHasher passwordHasher = new PasswordHasher();
            PasswordHash hash = passwordHasher.ComputeHash(userParam.Password);

            string sql = "INSERT INTO users (username, first_name, last_name, email_address, password_hash, salt, user_role) " +
                         "OUTPUT INSERTED.user_id " +
                         "VALUES (@username, @first_name, @last_name, @email_address, @password_hash, @salt, @user_role)";

            int newUserId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", userParam.Username);
                    cmd.Parameters.AddWithValue("@first_name", userParam.First_Name);
                    cmd.Parameters.AddWithValue("@last_name", userParam.Last_Name);
                    cmd.Parameters.AddWithValue("@email_address", userParam.Email_Address);
                    cmd.Parameters.AddWithValue("@password_hash", hash.Password);
                    cmd.Parameters.AddWithValue("@salt", hash.Salt);
                    cmd.Parameters.AddWithValue("@user_role", userParam.Role);

                    newUserId = Convert.ToInt32(cmd.ExecuteScalar());

                }
                newUser = GetUserById(newUserId);
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get add user to db, with pw hash", $"For {userParam.Username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("SQL exception occurred", ex);
            }

            return newUser;
        }

        public bool DeactivateUser(string username)
        {
            int numberOfRows = 0;

            User userNotActive = GetUserByUsername(username);

            if (userNotActive == null)
            {
                return false;
            }
            else if (userNotActive.IsActive == false)
            {
                return false;
            }
            string sql = "UPDATE users " +
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
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to deactivate username", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;
        }

        public bool ReactivateUser(string username)
        {
            int numberOfRows = 0;

            User userReactive = GetUserByUsername(username);

            if (userReactive == null)
            {
                return false;
            }
            else if (userReactive.IsActive == true)
            {
                return false;
            }
            string sql = "UPDATE users " +
                "SET is_active = 1, updated_date = @updated_date " +
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
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to reactivate by username", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;
        }
        public bool UpgradeUser(string username)
        {
            int numberOfRows = 0;

            User upgUser = GetUserByUsername(username);

            if (upgUser == null)
            {
                return false;
            }
            else if (upgUser.Role == "premium")
            {
                return false;
            }
            string sql = "UPDATE users " +
                "SET user_role = 'premium', updated_date = @updated_date " +
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
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
            }
            catch (DaoException ex)
            {
                ErrorLog.WriteLog("Trying to upgrade user", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;
        }

        public bool DowngradeUser(string username)
        {
            int numberOfRows = 0;

            User downUser = GetUserByUsername(username);

            if (downUser == null)
            {
                return false;
            }
            else if (downUser.Role == "free")
            {
                return false;
            }
            string sql = "UPDATE users " +
                "SET user_role = 'free', updated_date = @updated_date " +
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
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
            }
            catch (DaoException ex)
            {
                ErrorLog.WriteLog("Trying to downgrade user", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;
        }

        public bool UpgradeAdmin(string username)
        {
            int numberOfRows = 0;

            User upgAdmin = GetUserByUsername(username);

            if (upgAdmin == null)
            {
                return false;
            }
            else if (upgAdmin.Role == "jacapreme")
            {
                return false;
            }
            string sql = "UPDATE users " +
                "SET user_role = 'jacapreme', updated_date = @updated_date " +
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
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
            }
            catch (DaoException ex)
            {
                ErrorLog.WriteLog("Trying to upgrade admin", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;
        }

        public bool DowngradeAdmin(string username)
        {
            int numberOfRows = 0;

            User downUser = GetUserByUsername(username);

            if (downUser == null)
            {
                return false;
            }
            else if (downUser.Role == "free" || downUser.Role == "premium")
            {
                return false;
            }
            string sql = "UPDATE users " +
                "SET user_role = 'premium', updated_date = @updated_date " +
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
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
            }
            catch (DaoException ex)
            {
                ErrorLog.WriteLog("Trying to downgrade admin", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;
        }

        public bool UpdateLastLogin(string username)
        {
            int numberOfRows = 0;

            User updLastLogin = GetUserByUsername(username);

            if (updLastLogin == null)
            {
                return false;
            }
            string sql = "UPDATE users " +
                "SET last_login = @time " +
                "WHERE username = @username;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@time", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();

                    if (numberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
            }
            catch (DaoException ex)
            {
                ErrorLog.WriteLog("Trying to update last login", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;
        }

        private User MapRowToUser(SqlDataReader reader)
        {
            User user = new User();
            user.UserId = Convert.ToInt32(reader["user_id"]);
            user.Username = Convert.ToString(reader["username"]);
            user.PasswordHash = Convert.ToString(reader["password_hash"]);
            user.Salt = Convert.ToString(reader["salt"]);
            user.Role = Convert.ToString(reader["user_role"]);
            user.IsActive = Convert.ToBoolean(reader["is_active"]);
            user.Created_Date = Convert.ToDateTime(reader["created_date"]);
            user.Updated_Date = Convert.ToDateTime(reader["updated_date"]);
            user.Last_Login = Convert.ToDateTime(reader["last_login"]);
            return user;
        }

    }
}
