using Capstone.DAO.Interfaces;
using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class CollectionsSqlDao : ICollectionsDao
    {
        private readonly string connectionString;

        public CollectionsSqlDao(string dbconnectionString)
        {
            connectionString = dbconnectionString;
        }

        public Collection GetCollection(string username, string name)
        {
            Collection output = null;

            string sql = "SELECT collection_id, library_id, discogs_id, name, is_private, is_active, created_date, updated_date FROM collections " +
                "WHERE name = @name AND username = @username AND collections.is_active = 1;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
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
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }
        public List<Collection> GetAllCollection(string username)
        {
            List<Collection> output = new List<Collection>();

            string sql = "SELECT collection_id, library_id, discogs_id, name, is_private, is_active, created_date, updated_date FROM collections " +
                "WHERE name = @name AND username = @username AND collections.is_active = 1;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output.Add(MapRowToCollection(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }
        public bool AddCollection(string username, string name, int? discogsId = null)
        {
            int collectionId = 0;

            Collection output = new Collection();

            string sql = "INSERT INTO collections (name) " +
                "OUTPUT INSERTED.collection_id " +
                "VALUES (@name);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogs_id", discogsId.Value);
                    cmd.Parameters.AddWithValue("@name", name);

                    collectionId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                throw new DaoException("Exception occurred", e);
            }
            return true;
        }

        public bool UpdateCollectionTitle(string name, string username)
        {
            int numberOfRows = 0;

            Collection collection = GetCollection(username, name);

            if (collection == null)
            {
                return false;
            }
            string sql = "UPDATE collections " +
                "SET name = @name, updated_date = @updated_date " +
                "WHERE name = @name AND username = @username AND collections.is_active = 1;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
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

        public bool AddRecordToCollection(string name, int discogId, string username)
        {
            int collectionId = 0;

            Collection output = new Collection();

            if (output == null)
            {
                return false;
            }
            else if (output.IsActive == false)
            {
                return false;
            }

            string sql = "INSERT INTO collections (library_id, discogs_id, name) " +
                "OUTPUT INSERTED.collection_id " +
                "VALUES (@library_id, @discogs_id, @name)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@discogs_id", discogId);
                    cmd.Parameters.AddWithValue("@name", name);

                    collectionId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                throw new DaoException("Exception occurred", e);
            }
            return true;
        }

        public bool RemoveSongFromCollection(string name, int discogsID, string username)
        {
            int NumberOfRows = 0;

            string sql = "DELETE FROM collections " +
                "WHERE name = @name AND discogs_id = @discogs_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("discogs_id", discogsID);
                    NumberOfRows = cmd.ExecuteNonQuery();
                    if (NumberOfRows != 1)
                    {
                        throw new DaoException("The wrong number of rows is impacted");
                    }
                }
            }
            catch (SqlException e)
            {
                throw new DaoException("RemoveRecord() not implemented", e);
            }
            return true;
        }

        public bool RemoveCollection(string name, string username)
        {
            int numberOfRows = 0;

            string sql = "DELETE FROM collections " +
                "WHERE name = @name AND updated_date = @updated_date;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
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
                throw new DaoException("RemoveCollection() not implemented", ex);
            }
            return true;
        }

        public bool PrivatizeCollection(string name, string username)
        {
            int numberOfRows = 0;

            string sql = "UPDATE collections " +
                "SET is_private = 1, updated_date = @updated_date " +
                "WHERE name = @name;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
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

        public bool PublicizeCollection(string name, string username)
        {
            int numberOfRows = 0;

            string sql = "UPDATE collections " +
                "WHERE is_private = 0, updated_date = @updated_date " +
                "WHERE name = @name;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
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
        public bool DeactivateCollection(string name, string username)
        {
            int numberOfRows = 0;

            string sql = "UPDATE collections " +
                "SET is_active = 0, updated_date = @updated_date " +
                "WHERE name = @name;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
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

        public bool ReactivateCollection(string name, string username)
        {
            int numberOfRows = 0;

            string sql = "UPDATE collections " +
                "SET is_active = 1, updated_date = @updated_date " +
                "WHERE name = @name;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@updated_date", DateTime.UtcNow);
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

        private Collection MapRowToCollection(SqlDataReader reader)
        {
            Collection collection = new Collection();
            collection.Collection_Id = Convert.ToInt32(reader["collection_id"]);
            collection.Library_Id = Convert.ToInt32(reader["library_id"]);
            collection.Discog_Id = Convert.ToInt32(reader["discogs_id"]);
            collection.Name = Convert.ToString(reader["name"]);
            collection.IsPrivate = Convert.ToBoolean(reader["is_private"]);
            collection.IsActive = Convert.ToBoolean(reader["is_active"]);
            collection.Created_Date = Convert.ToDateTime(reader["created_date"]);
            collection.Updated_Date = Convert.ToDateTime(reader["updated_date"]);
            return collection;
        }
    }
}
