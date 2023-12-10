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
    public class RecordCollectionSqlDao : IRecordsCollectionsDao
    {
        private readonly string connectionString;
        public RecordCollectionSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Gets all records in a collection. Use the get methods in collections to obtain the record by username/name.
        /// </summary>
        /// <param name="discogsId"></param>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public List<RecordCollection> GetAllRecordsInCollectionByCollectionId(int collectionId)
        {
            List<RecordCollection> output = new List<RecordCollection>();

            string sql = "SELECT records_collections_id, is_premium " +
                "FROM records_collections " +
                "WHERE collection_id = @collectionId AND is_active = 1 ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        RecordCollection row = MapRowToRecordCollection(reader);
                        output.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to get all records in collection", $"For {collectionId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
            return output;
        }

        public List<int> GetAllCollectionsForThisDiscogsIdByUsername(int discogsId, string username)
        {
            List<int> output = new List<int>();

            string sql = "SELECT records_collections.collection_id " +
                "FROM records_collections " +
                "JOIN collections ON records_collections.collection_id = collections.collection_id " +
                "WHERE username = @username AND discogs_id = @discogsId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int row = Convert.ToInt32(reader["collection_id"]);
                        output.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to get all collections that this record appears in for this user", $"For {username}, {discogsId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets singular record in a collection. Use the get methods in collections to obtain the record by username/name.
        /// </summary>
        /// <param name="discogsId"></param>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public int GetRecordCollectionByDiscogsIdAndCollectionId(int discogsId, int collectionId)
        {
            int output = 0; 

            string sql = "SELECT records_collections_id " +
                "FROM records_collections " +
                "WHERE discogs_id = @discogsId AND collection_id = @collectionId AND is_active = 1 ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = Convert.ToInt32(reader["records_collections_id"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to get record in collection", $"For {discogsId}, {collectionId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Supply the recordId. Then find the collection ID and supply that here too.
        /// Either it adds it and is successful or returns false/errors out
        /// </summary>
        /// <param name="discogsId">From the record</param>
        /// <param name="collectionId">From the collections table - use GetCollections</param>
        /// <param name="libraryId"></param>
        /// <param name="isPremium"></param>
        /// <returns>True if successful, false if already exists</returns>
        /// <exception cref="DaoException"></exception>
        public bool AddRecordCollections(int discogsId, int collectionId, int libraryId, bool isPremium)
        {
            if (GetRecordCollectionByDiscogsIdAndCollectionId(discogsId, collectionId) != 0)
            {
                return false;
            }

            string sql = "INSERT INTO records_collections (library_id, collection_id, discogs_id, is_premium) " +
                "OUTPUT INSERTED.records_collections_id " +
                "VALUES (@libraryId, @collectionId, @discogsId, @isPremium);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@libraryId", libraryId);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add record to collection", $"For {discogsId}, {collectionId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }

        /// <summary>
        /// Removes an association between a collection and a record. Should be called before a colleciton is deleted.
        /// </summary>
        /// <param name="discogsID"></param>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool DeleteRecordCollectionByDiscogsIdAndCollectionId(int discogsId, int collectionId)
        {
            int NumberOfRows = 0;

            string sql = "DELETE FROM records_collections " +
                "WHERE discogs_id = @discogsId AND collection_id = @collectionId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);
                    NumberOfRows = cmd.ExecuteNonQuery();
                    if (NumberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to delete record from collection", $"For {discogsId}, {collectionId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
            return true;
        }

        /// <summary>
        /// Deletes all records in a collection. Should be called before a colleciton is deleted.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool DeleteAllRecordsInCollectionByCollectionId(int collectionId)
        {
            int NumberOfRows = 0;

            string sql = "DELETE FROM records_collections " +
                "WHERE collection_id = @collectionId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);
                    NumberOfRows = cmd.ExecuteNonQuery();
                    if (NumberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows were impacted");
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to delete all records in a collection", $"For {collectionId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
            return true;
        }

        /// <summary>
        /// Toggles specific record in record_collections for a user from isPremium true to false and vice versa. Ideally called in a for/foreach loop. Used when down/upgrading an account.
        /// </summary>
        /// <param name="discogsId"></param>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool ChangeSingleRecordCollectionIsPremium(int discogsId, string username, bool isPremium)
        {
            int numberOfRows = 0;

            string sql = "UPDATE rc " +
                "SET rc.is_premium = @isPremium, rc.updated_date = @updated_date " +
                "FROM collections AS c " +
                "JOIN records_collections AS rc ON c.collection_id = rc.collection_id " +
                "WHERE username = @username AND discogs_id = @discogsId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogsId", discogsId);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();
                    

                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to change collection free/premium status for all records", $"For {username}, action: {isPremium}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;

        }

        /// <summary>
        /// Toggles all records in record_collections for a user from isPremium true to false and vice versa. Ideally called in a for/foreach loop. Used when down/upgrading an account.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool ChangeAllRecordCollectionIsPremium(string username, bool isPremium)
        {
            int numberOfRows = 0;

            string sql = "UPDATE rc " +
                "SET rc.is_premium = @isPremium, rc.updated_date = @updated_date " +
                "FROM collections AS c " +
                "JOIN records_collections AS rc ON c.collection_id = rc.collection_id " +
                "WHERE username = @username";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@isPremium", isPremium);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
                    numberOfRows = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to change collection free/premium status for all records", $"For {username}, action: {isPremium}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
            return true;

        }

        /// <summary>
        /// De/Reactivates (but not un/deletes) a user's records in the collection when they de/reactivate their profile.
        /// Separate from when they down/upgrade
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        /// <exception cref="DaoException"></exception>
        public bool DeReactivateRecordsInCollection(string username, bool isActive)
        {
            int numberOfRows = 0;

            string sql = "UPDATE rc " +
                "SET rc.is_active = @isActive, rc.updated_date = @updated_date " +
                "FROM collections AS c " +
                "JOIN records_collections AS rc ON c.collection_id = rc.collection_id " +
                "WHERE username = @username";

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
                ErrorLog.WriteLog("Trying to de/reactivate records in the user's collection", $"For {username}, action: {isActive}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Something went wrong", ex);
            }
        }

        private RecordCollection MapRowToRecordCollection(SqlDataReader reader)
        {
            RecordCollection output = new RecordCollection();
            output.Collection_Id = Convert.ToInt32(reader["collection_id"]);
            output.Discogs_Id = Convert.ToInt32(reader["discogs_id"]);
            output.Is_Premium = Convert.ToBoolean(reader["is_premium"]);

            return output;
        }
    }
}
