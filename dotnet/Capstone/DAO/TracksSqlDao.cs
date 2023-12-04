using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class TracksSqlDao : ITracksDao
    {
        private readonly string connectionString;

        public TracksSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Track GetTrack(Track track)
        {
            Track output = null;
            string sql = "SELECT track_id, record_id, title, position, duration, is_active, created_date, updated_date FROM tracks " +
                "WHERE title = @title;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@title", track.Title);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapToTrack(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        public bool AddTrack(Track track)
        {
            Track checkedTrack = GetTrack(track);

            if (checkedTrack != null)
            {
                return false;
            }
            string sql = "INSERT INTO tracks (record_id, title, position, duration) " +
                "OUTPUT INSERTED.track_id " +
                "VALUES (@record_id, @title, @position, @duration);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("record_id", track.Record_Id);
                    cmd.Parameters.AddWithValue("title", track.Title);
                    cmd.Parameters.AddWithValue("position", track.Position);
                    cmd.Parameters.AddWithValue("duration", track.Duration);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new DaoException("Exception occurred", e);
            }
        }

        private Track MapToTrack(SqlDataReader reader)
        {
            Track track = new Track();
            track.Record_Id = Convert.ToInt32(reader["record_id"]);
            track.Title = Convert.ToString(reader["title"]);
            track.Position = Convert.ToString(reader["position"]);
            track.Duration = Convert.ToString(reader["duration"]);
            return track;
        }

    }
}
