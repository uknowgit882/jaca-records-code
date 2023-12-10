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
            string sql = "SELECT track_id, discogs_id, title, position, duration FROM tracks " +
                "WHERE title = @title AND discogs_id = @discogsId;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@title", track.Title);
                    cmd.Parameters.AddWithValue("@discogsId", track.Discogs_Id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapToTrack(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get track", $"For {track.Discogs_Id}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Gets the track info associated for this record. Includes title, position, duration
        /// </summary>
        /// <param name="discogId"></param>
        /// <returns>List of tracks. Only the title, position, duration should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Track> GetTracksByDiscogsId(int discogId)
        {
            List<Track> output = new List<Track>();
            string sql = "SELECT tracks.title, position, duration " +
                "FROM tracks " +
                "JOIN records ON tracks.discogs_id = records.discogs_id " +
                "WHERE records.discogs_id = @discogId";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogId", discogId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Track row = new Track();
                        row.Title = Convert.ToString(reader["title"]);
                        row.Position = Convert.ToString(reader["position"]);
                        row.Duration = Convert.ToString(reader["duration"]);
                        output.Add(row);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get track by discogsId", $"For {discogId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
            return output;
        }

        /// <summary>
        /// Returns how many tracks are in the entire database.
        /// </summary>
        /// <returns>Int number of tracks</returns>
        /// <exception cref="DaoException"></exception>
        public int GetTrackCount()
        {
            int output = 0;

            string sql = "SELECT count(track_id) AS count " +
                "FROM tracks ";
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
                ErrorLog.WriteLog("Trying to get track count", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many tracks are associated with this user. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns>Int number of tracks</returns>
        /// <exception cref="DaoException"></exception>
        public int GetTrackCountByUsername(string username, bool isPremium)
        {
            int output = 0;

            string sql = "SELECT count(tracks.track_id) AS count " +
                "FROM tracks " +
                "JOIN records ON tracks.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND is_premium = @isPremium AND tracks.is_active = 1 ";
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
                ErrorLog.WriteLog("Trying to get track count for user", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        public bool AddTrack(Track track)
        {
            Track checkedTrack = GetTrack(track);

            if (checkedTrack != null)
            {
                return false;
            }
            string sql = "INSERT INTO tracks (discogs_id, title, position, duration) " +
                "OUTPUT INSERTED.track_id " +
                "VALUES (@discogs_id, @title, @position, @duration);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("discogs_id", track.Discogs_Id);
                    cmd.Parameters.AddWithValue("title", track.Title);
                    cmd.Parameters.AddWithValue("position", track.Position);
                    cmd.Parameters.AddWithValue("duration", track.Duration);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add track", $"For {track.Discogs_Id}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Exception occurred", ex);
            }
        }
        public Track UpdateTrack(Track updatedTrack)
        {
            string sql = "UPDATE tracks " +
                "SET position = @position, duration = @duration, updated_date = @updatedDate " +
                "WHERE title = @title AND discogs_id = @discogsId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@position", updatedTrack.Position);
                    cmd.Parameters.AddWithValue("@duration", updatedTrack.Duration);
                    cmd.Parameters.AddWithValue("@updatedDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@title", updatedTrack.Title);
                    cmd.Parameters.AddWithValue("@discogsId", updatedTrack.Discogs_Id);
                    int numberOfRowsAffected = cmd.ExecuteNonQuery();
                }
                return GetTrack(updatedTrack);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to update track", $"For {updatedTrack.Discogs_Id}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }

        private Track MapToTrack(SqlDataReader reader)
        {
            Track track = new Track();
            track.Discogs_Id = Convert.ToInt32(reader["discogs_id"]);
            track.Title = Convert.ToString(reader["title"]);
            track.Position = Convert.ToString(reader["position"]);
            track.Duration = Convert.ToString(reader["duration"]);
            return track;
        }

    }
}
