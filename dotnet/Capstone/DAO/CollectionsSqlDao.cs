using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Capstone.DAO
{
    public class CollectionsSqlDao : ICollectionsDao
    {
        private readonly string connectionString;

        public CollectionsSqlDao(string dbconnectionString)
        {
            connectionString = dbconnectionString;
        }


        /// <summary>
        /// Gets all user collections, regardless of privacy status or role, or active status
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public List<Collection> GetAllCollections(string username)
        {
            List<Collection> output = new List<Collection>();

            string sql = "SELECT collection_id, username, name, is_private, is_premium " +
                "FROM collections " +
                "WHERE username = @username ";

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
                        output.Add(MapRowToCollection(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get all collections", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets all user collections, regardless of privacy status.
        /// For all users. Pass in the current user's role in the toggle. 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public List<Collection> GetAllCollectionsByRole(string username, bool isPremium)
        {
            List<Collection> output = new List<Collection>();

            string sql = "SELECT collection_id, username, name, is_private, is_premium " +
                "FROM collections " +
                "WHERE username = @username " +
                "AND is_active = 1 AND is_premium = @isPremium;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output.Add(MapRowToCollection(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get all collections", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets all user collections, you can choose public/private.
        /// For all users. 
        /// </summary>
        /// <param name="isPrivate">Default is false (public collection)</param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public List<Collection> GetPubOrPrivAllCollections(bool isPrivate = false)
        {
            List<Collection> output = new List<Collection>();

            string sql = "SELECT collection_id, username, name, is_private, is_premium " +
                "FROM collections " +
                "WHERE is_private = @isPrivate " +
                "AND is_active = 1;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@isPrivate", isPrivate);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output.Add(MapRowToCollection(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get all pub/priv collections", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }



        /// <summary>
        /// Gets the specific user's named collection, regardless of privacy status.
        /// For all users. Pass in the current user's role in the toggle. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <returns>User's singular collection, regardless of privacy status</returns>
        /// <exception cref="DaoException"></exception>
        public Collection GetNamedCollection(string username, string name, bool isPremium)
        {
            Collection output = null;

            string sql = "SELECT collection_id, username, name, is_private, is_premium " +
                "FROM collections " +
                "WHERE username = @username AND name = @name " +
                "AND is_active = 1 AND is_premium = @isPremium;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToCollection(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get named collection", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the specific user's named collection, regardless of privacy or premium status.
        /// For all users. Overloaded method, no need for user role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <returns>User's singular collection, regardless of privacy status</returns>
        /// <exception cref="DaoException"></exception>
        public Collection GetNamedCollection(string username, string name)
        {
            Collection output = null;

            string sql = "SELECT collection_id, username, name, is_private, is_premium " +
                "FROM collections " +
                "WHERE username = @username AND name = @name " +
                "AND is_active = 1 ;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@name", name);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToCollection(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get named collection", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the specific user's named collection, you can choose public/private.
        /// For all users. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <param name="isPrivate">Default is false (public collection)</param>
        /// <returns>Singular named collection</returns>
        /// <exception cref="DaoException"></exception>
        public Collection GetPubOrPrivCollection(string username, string name, bool isPrivate = false)
        {
            Collection output = null;

            string sql = "SELECT collection_id, username, name, is_private, is_premium " +
                "FROM collections " +
                "WHERE username = @username AND name = @name AND is_private = @isPrivate " +
                "AND is_active = 1 ;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@isPrivate", isPrivate);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToCollection(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get named pub/priv collection", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets all records in a collection, based on the username/name. Can toggle based on isPremium, or isPrivate.
        /// Then use "BuildFullRecord" in the common controller to build out the full record to display.
        /// Assumes you will not need to get a singular record/collection combination individually.
        /// Free users should only see the isPremium false records in their free collection
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <param name="isPremium"></param>
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public List<int> GetAllRecordsInCollectionByUsernameAndName(string username, string name, bool isPremium, bool isPrivate = false)
        {
            List<int> output = new List<int>();

            string sql = "SELECT discogs_id " +
                "FROM collections " +
                "JOIN records_collections ON collections.collection_id = records_collections.collections_id " +
                "WHERE username = @username AND name = @name, AND collections.is_active = 1 AND is_private = @isPrivate AND records_collections.is_premium = @isPremium;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@isPrivate", isPrivate);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int row = Convert.ToInt32(reader["discogs_id"]);
                        output.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to get all records in named collection", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns how many collections are in the active user's library.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns>Int number of collections</returns>
        /// <exception cref="DaoException"></exception>
        public int GetCollectionCountByUsername(string username, bool isPremium)
        {
            int output = 0;

            string sql = "SELECT username, count(username) AS count " +
                "FROM collections " +
                "WHERE username = @username AND is_premium = @isPremium AND is_active = 1 " +
                "GROUP BY username;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
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
                ErrorLog.WriteLog("Trying to get collection count", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many collections marked is_premium false are in the free user's collection set. Active users only. Should not exceed 1
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Int number of collections</returns>
        /// <exception cref="DaoException"></exception>
        public int GetFreeUserCollectionCountByUsername(string username)
        {
            int output = 0;

            string sql = "SELECT username, count(username) AS count " +
                "FROM collections " +
                "WHERE username = @username AND is_active = 1 AND is_premium = 0 " +
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
                ErrorLog.WriteLog("Trying to get collection count", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many collections are in the entire database for all active users.
        /// </summary>
        /// <returns>Int number of collections</returns>
        /// <exception cref="DaoException"></exception>
        public int GetCollectionCount()
        {
            int output = 0;

            string sql = "SELECT count(username) AS count " +
                "FROM collections " +
                "WHERE is_active = 1 ";
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
                ErrorLog.WriteLog("Trying to get total collection count", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }


        /// <summary>
        /// Returns how many records are in the user's specific collection.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <returns>Int number of records</returns>
        /// <exception cref="DaoException"></exception>
        public int CountOfRecordsInSpecificCollectionByUsername(string username, string name)
        {
            int output = 0;

            string sql = "SELECT username, name, count(records_collections.discogs_id) AS count " +
                "FROM collections " +
                "JOIN records_collections ON collections.collection_id = records_collections.collection_id " +
                "WHERE username = @username AND name = @name AND collections.is_active = 1 " +
                "GROUP BY username, name;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@name", name);
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
                ErrorLog.WriteLog("Trying to get records in named collection count", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many records are in all collections for the user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns>Int number of records</returns>
        /// <exception cref="DaoException"></exception>
        public int CountOfRecordsInAllCollectionsByUsername(string username, bool isPremium)
        {
            int output = 0;

            string sql = "SELECT username, count(records_collections.discogs_id) AS count " +
                "FROM collections " +
                "JOIN records_collections ON collections.collection_id = records_collections.collection_id " +
                "WHERE username = @username AND collections.is_premium = @isPremium AND collections.is_active = 1 " +
                "GROUP BY username;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
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
                ErrorLog.WriteLog("Trying to get records in collection count", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many records are in all collections for all active users, in aggregate.
        /// </summary>
        /// <returns>Int number of records</returns>
        /// <exception cref="DaoException"></exception>
        public int CountOfRecordsInAllCollections()
        {
            int output = 0;

            string sql = "SELECT count(records_collections.discogs_id) AS count " +
                "FROM collections " +
                "JOIN records_collections ON collections.collection_id = records_collections.collection_id " +
                "WHERE collections.is_active = 1 ";
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
                ErrorLog.WriteLog("Trying to get records in all collections count", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }


        /// <summary>
        /// Adds a collection for the user. Records are added separately in their own RecordsColelctionsSqlDao.
        /// Methods calling this should first call GetCollectionCountByUsername. Free users should not be able to have >1 collection.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name">The collection's name</param>
        /// <param name="role">The collection's name</param>
        /// <returns>Returns newly created collection's id</returns>
        /// <exception cref="DaoException"></exception>
        public int AddCollection(string username, string name, bool isPremium)
        {
            int collectionId = 0;

            Collection output = new Collection();

            string sql = "INSERT INTO collections (username, name, is_premium) " +
                "OUTPUT INSERTED.collection_id " +
                "VALUES (@username, @name, @isPremium);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);

                    collectionId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add collection", $"For {username}, adding {name}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Exception occurred", ex);
            }
            return collectionId;
        }

        /// <summary>
        /// Update's the user's collection's name (title). Doesn't need a toggle for privacy or premium.
        /// (Free user's shouldn't be able to see their hidden premium collections).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <param name="newName"></param>
        /// <returns>True if actioned</returns>
        /// <exception cref="DaoException"></exception>
        public bool UpdateCollectionTitle(string name, string username, string newName)
        {
            int numberOfRows = 0;

            string sql = "UPDATE collections " +
                "SET name = @new_name, updated_date = @updated_date " +
                "WHERE name = @name AND username = @username AND is_active = 1;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@new_name", newName);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();

                    if (numberOfRows == 0)
                    {
                        return false;
                    }
                    else if (numberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows is impacted");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to rename collection", $"For {username}, {name}, to {newName}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
        }

        /// <summary>
        /// Deletes a collection. Should not be called before DeleteRecord in RecordCollectionSqlDao or FK_ error will occur.
        /// Removes collection permanently
        /// </summary>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool DeleteCollection(string name, string username)
        {
            int numberOfRows = 0;

            string sql = "DELETE FROM collections " +
                "WHERE name = @name AND username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@username", username);

                    numberOfRows = cmd.ExecuteNonQuery();
                    if (numberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows is impacted");
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to delete collection", $"For {username}, {name}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("RemoveCollection() not implemented", ex);
            }
            return true;
        }

        /// <summary>
        /// Changes the private/public status of a collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool ChangeCollectionPrivacy(string name, string username, bool isPrivate)
        {
            int numberOfRows = 0;

            string sql = "UPDATE collections " +
                "SET is_private = @isPrivate, updated_date = @updated_date " +
                "WHERE name = @name AND username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@isPrivate", isPrivate);
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
                ErrorLog.WriteLog("Trying to update collection", $"For {username}, {name}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;

        }

        /// <summary>
        /// Toggles a collection from isPremium true to false and vice versa. Ideally called in a for/foreach loop. Used when down/upgrading an account.
        /// All existing collections should be left isPremium. A new collection should be created (empty), called "free", and should be marked isPremium False.
        /// The get collections will prevent free users seeing isPremium true.
        /// When upgrading, the free collection should then be marked isPremiumTrue.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool ChangeCollectionIsPremium(string name, string username, bool isPremium)
        {
            int numberOfRows = 0;

            string sql = "UPDATE collections " +
                "SET is_premium = @isPremium, updated_date = @updated_date " +
                "WHERE name = @name AND username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
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
                ErrorLog.WriteLog("Trying to change collection premium/free status", $"For {username}, {name}, direction of up/downgrade {isPremium}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;

        }

        /// <summary>
        /// Called when a user's account is de/reactivated. Does not un/delete the collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="username"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool DeReactivateCollection(string username, bool isActive)
        {
            int numberOfRows = 0;

            string sql = "UPDATE collections " +
                "SET is_active = @isActive, updated_date = @updated_date " +
                "WHERE username = @username;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@isActive", isActive);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();
                   
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to de/reactivate collection", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;
        }


        private Collection MapRowToCollection(SqlDataReader reader)
        {
            Collection collection = new Collection();
            collection.Collection_Id = Convert.ToInt32(reader["collection_id"]);
            collection.Username = Convert.ToString(reader["username"]);
            collection.Name = Convert.ToString(reader["name"]);
            collection.IsPrivate = Convert.ToBoolean(reader["is_private"]);
            collection.IsPremium = Convert.ToBoolean(reader["is_premium"]);
            return collection;
        }
    }
}
