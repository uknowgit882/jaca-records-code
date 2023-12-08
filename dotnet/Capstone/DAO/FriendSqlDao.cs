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
    public class FriendSqlDao : IFriendsDao
    {
        private readonly string connectionString;
        public FriendSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all friends for the user, by the user's id
        /// </summary>
        /// <param name="id">The user's id</param>
        /// <returns>The user's username, first/last name, email address, and all friends,
        /// with the same information and whether the friend is active</returns>
        /// <exception cref="DaoException"></exception>
        public UserAndFriends GetUsersFriendsById(int id)
        {
            UserAndFriends output = null;
            // pull the user (username, first/last name, email)
            // pull their friends (username, first/last name, email) and whether they are still active or not
            string sql = "SELECT users.user_id, users.username AS users_username, users.first_name AS users_first_name, users.last_name AS users_last_name, users.email_address AS users_email_address, " +
                "friend.user_id AS friends_id, friend.username AS friends_username, friend.first_name AS friends_first_name, friend.last_name AS friends_last_name, friend.email_address AS friends_email_address, friend.is_active AS friend_is_active " +
                "FROM friends " +
                "JOIN users ON friends.user_id = users.user_id " +
                "JOIN users AS friend ON friends.friends_user_id = friend.user_id " +
                "WHERE users.user_id = @id " +
                "ORDER BY friend.user_id ASC";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // this reader will have the user, and all their friends, so multiple rows
                    // only need the user once

                    while (reader.Read())
                    {
                        // first time, do the full row
                        if(output == null)
                        {
                            output = MapRowToUser(reader);
                            output.Friends.Add(MapRowToFriend(reader));
                        }
                        else
                        {
                            // otherwise just add the friend portion of the row to the output
                            output.Friends.Add(MapRowToFriend(reader));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get user's friends", $"For user id {id}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }

        /// <summary>
        /// Returns all friends for the user, by the user's username
        /// </summary>
        /// <param name="id">The user's username</param>
        /// <returns>The user's username, first/last name, email address, and all friends,
        /// with the same information and whether the friend is active</returns>
        /// <exception cref="DaoException"></exception>
        public UserAndFriends GetUsersFriendsByUsername(string username)
        {
            // basically the same method as above
            UserAndFriends output = null;
            string sql = "SELECT users.user_id, users.username AS users_username, users.first_name AS users_first_name, users.last_name AS users_last_name, users.email_address AS users_email_address, " +
                "friend.user_id AS friends_id, friend.username AS friends_username, friend.first_name AS friends_first_name, friend.last_name AS friends_last_name, friend.email_address AS friends_email_address, friend.is_active AS friend_is_active " +
                "FROM friends " +
                "JOIN users ON friends.user_id = users.user_id " +
                "JOIN users AS friend ON friends.friends_user_id = friend.user_id " +
                "WHERE users.username = @username " +
                "ORDER BY friend.user_id ASC";
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
                        if (output == null)
                        {
                            output = MapRowToUser(reader);
                            output.Friends.Add(MapRowToFriend(reader));
                        }
                        else
                        {
                            output.Friends.Add(MapRowToFriend(reader));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get user's friends", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }

        /// <summary>
        /// Adds a friend to this user. Requires user's id and friend's id
        /// </summary>
        /// <param name="id">The user's id</param>
        /// <returns>The user's username, first/last name, email address, and all current friends (including addition),
        /// with the same information and whether the friend is active</returns>
        /// <exception cref="DaoException"></exception>
        public UserAndFriends AddFriend(int usersId, int friendsId)
        {
            UserAndFriends output = null;

            int friendId = 0;

            string sql = "INSERT INTO friends (user_id, friends_user_id) " +
                "OUTPUT INSERTED.friend_id " +
                "VALUES (@usersId, @friendsId);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@usersId", usersId);
                    cmd.Parameters.AddWithValue("@friendsId", friendsId);
                    
                    friendId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                output = GetUsersFriendsById(usersId);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add user's friend", $"For user: {usersId}, friend: {friendsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Removes a friend from this user. Requires user's id and friend's id
        /// </summary>
        /// <param name="id">The user's id</param>
        /// <returns>The user's username, first/last name, email address, and all current friends (excluding removed),
        /// with the same information and whether the friend is active</returns>
        /// <exception cref="DaoException"></exception>
        public UserAndFriends DropFriend(int usersId, int friendsId)
        {
            UserAndFriends output = null;

            int numberOfRowsAffected = 0;

            string sql = "DELETE FROM friends " +
                "WHERE user_id = @usersId AND friends_user_id = @friendsId";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@usersId", usersId);
                    cmd.Parameters.AddWithValue("@friendsId", friendsId);

                    numberOfRowsAffected = cmd.ExecuteNonQuery();
                    if(numberOfRowsAffected != 1)
                    {
                        throw new DaoException("The wrong number of rows was affected");
                    }
                }
                output = GetUsersFriendsById(usersId);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to drop user's friend", $"For user: {usersId}, friend: {friendsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
            return output;
        }

        private UserAndFriends MapRowToUser(SqlDataReader reader)
        {
            UserAndFriends output = new UserAndFriends();
            output.User_Id = Convert.ToInt32(reader["user_id"]);
            output.Username = Convert.ToString(reader["users_username"]);
            output.First_Name = Convert.ToString(reader["users_first_name"]);
            output.Last_Name = Convert.ToString(reader["users_last_name"]);
            output.Email_Address = Convert.ToString(reader["users_email_address"]);
            return output;
        }
        private Friend MapRowToFriend(SqlDataReader reader)
        {
            Friend output = new Friend();
            output.Friends_Id = Convert.ToInt32(reader["friends_id"]);
            output.Friends_Username = Convert.ToString(reader["friends_username"]);
            output.Friends_First_Name = Convert.ToString(reader["friends_first_name"]);
            output.Friends_Last_Name = Convert.ToString(reader["friends_last_name"]);
            output.Friends_Email_Address = Convert.ToString(reader["friends_email_address"]);
            output.Friend_Is_Active = Convert.ToBoolean(reader["friend_is_active"]);
            return output;
        }
    }
}
