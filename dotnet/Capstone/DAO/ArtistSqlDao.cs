using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class ArtistSqlDao : IArtistsDao
    {
        private readonly string connectionString;
        public ArtistSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Artist GetArtist(Artist artist)
        {
            Artist output = null;
            string sql = "SELECT artist_id, name " +
                "FROM artists " +
                "WHERE name = @name";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", artist.Name);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToArtist(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }
        public bool AddArtist(Artist artist)
        {
            Artist checkedArtist = GetArtist(artist);

            if (checkedArtist != null)
            {
                return false;
            }
            string sql = "INSERT INTO artists (name) " +
                "OUTPUT INSERTED.artist_id " +
                "VALUES (@name);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", artist.Name);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new DaoException("exception occurred", e);
            }
        }

        private Artist MapRowToArtist(SqlDataReader reader)
        {
            Artist output = new Artist();
            output.Artist_Id = Convert.ToInt32(reader["artist_id"]);
            output.Name = Convert.ToString(reader["name"]);
            return output;
        }
    }
}
