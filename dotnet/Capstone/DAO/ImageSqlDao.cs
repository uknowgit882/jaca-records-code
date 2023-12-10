using Capstone.Exceptions;
using Capstone.Models;
using System.Data.SqlClient;
using System;
using Capstone.DAO.Interfaces;
using System.Collections.Generic;
using Capstone.Utils;
using System.Reflection;

namespace Capstone.DAO
{
    public class ImageSqlDao : IImagesDao
    {
        // barcodes are "identifiers" in the discogs api
        private readonly string connectionString;
        public ImageSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Image GetImageInfoExact(Image image)
        {
            Image output = null;
            string sql = "SELECT image_id, discogs_id, uri, height, width " +
                "FROM images " +
                "WHERE discogs_id = @discogsId AND uri = @uri AND height = @height AND width = @width";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", image.Discogs_Id);
                    cmd.Parameters.AddWithValue("@uri", image.Uri);
                    cmd.Parameters.AddWithValue("@height", image.Height);
                    cmd.Parameters.AddWithValue("@width", image.Width);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        output = MapRowToImage(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get image based on exact match", $"For {image.Discogs_Id}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }

        /// <summary>
        /// Gets the image info associated for this record
        /// </summary>
        /// <param name="discogId"></param>
        /// <returns>List of images. Only the uri, height, width should be sent to the front end - use JSONIgnore on the other properties</returns>
        /// <exception cref="DaoException"></exception>
        public List<Image> GetAllImagesByDiscogsId(int discogId)
        {
            List<Image> output = new List<Image>();
            string sql = "SELECT image_id, discogs_id, uri, height, width " +
                "FROM images " +
                "WHERE discogs_id = @discogsId";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", discogId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output.Add(MapRowToImage(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to get image by discogId", $"For {discogId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }

        /// <summary>
        /// Returns how many images are in the entire database.
        /// </summary>
        /// <returns>Int number of images</returns>
        /// <exception cref="DaoException"></exception>
        public int GetImageCount()
        {
            int output = 0;

            string sql = "SELECT count(image_id) AS count " +
                "FROM images ";
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
                ErrorLog.WriteLog("Trying to get total image count", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        /// <summary>
        /// Returns how many images are associated with this user. Active users only.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isPremium"></param>
        /// <returns>Int number of images</returns>
        /// <exception cref="DaoException"></exception>
        public int GetImageCountByUsername(string username, bool isPremium)
        {
            int output = 0;

            string sql = "SELECT count(images.image_id) AS count " +
                "FROM images " +
                "JOIN records ON images.discogs_id = records.discogs_id " +
                "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
                "WHERE username = @username AND is_premium = @isPremium AND images.is_active = 1 ";
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
                ErrorLog.WriteLog("Trying to get image count for this user", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occurred", ex);
            }
        }

        // I don't think I need this...
        ///// <summary>
        ///// Gets the image info associated for this record for this specific user. Includes uri, height, width
        ///// </summary>
        ///// <param name="discogId"></param>
        ///// <param name="username"></param>
        ///// <returns>List of images for this specific user. Only the uri, height, width should be sent to the front end - use JSONIgnore on the other properties</returns>
        ///// <exception cref="DaoException"></exception>
        //public List<Image> GetImagesByDiscogsIdAndUsername(int discogId, string username)
        //{
        //    List<Image> output = new List<Image>();
        //    string sql = "SELECT uri, height, width " +
        //        "FROM images " +
        //        "JOIN records ON images.discogs_id = records.discogs_id " +
        //        "JOIN libraries ON records.discogs_id = libraries.discogs_id " +
        //        "WHERE records.discogs_id = @discogId AND username = @username";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@discogId", discogId);
        //            cmd.Parameters.AddWithValue("@username", username);
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                Image row = new Image();
        //                row.Uri = Convert.ToString(reader["uri"]);
        //                row.Height = Convert.ToInt32(reader["height"]);
        //                row.Width = Convert.ToInt32(reader["width"]);
        //                output.Add(row);
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("Sql exception occurred", ex);
        //    }
        //    return output;
        //}

        public bool AddImage(Image image)
        {
            Image checkedImage = GetImageInfoExact(image);

            if (checkedImage != null)
            {
                return false;
            }
            string sql = "INSERT INTO images (discogs_id, uri, height, width) " +
                "OUTPUT INSERTED.image_id " +
                "VALUES (@discogsId, @uri, @height, @width);";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@discogsId", image.Discogs_Id);
                    cmd.Parameters.AddWithValue("@uri", image.Uri);
                    cmd.Parameters.AddWithValue("@height", image.Height);
                    cmd.Parameters.AddWithValue("@width", image.Width);
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add image", $"For {image.Discogs_Id}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }
        public int DeleteImage(int imageId)
        {
            string sql = "DELETE images " +
                "WHERE image_id = @imageId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@imageId", imageId);

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to delete image", $"For {imageId}", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("exception occurred", ex);
            }
        }

        private Image MapRowToImage(SqlDataReader reader)
        {
            Image output = new Image();
            output.Image_Id = Convert.ToInt32(reader["image_id"]);
            output.Discogs_Id = Convert.ToInt32(reader["discogs_id"]);
            output.Uri = SqlUtil.NullableString(reader["uri"]);
            if (reader["height"] is DBNull)
            {
                output.Height = 0;
            }
            else
            {
                output.Height = Convert.ToInt32(reader["height"]);
            }
            if (reader["width"] is DBNull)
            {
                output.Width = 0;
            }
            else
            {
                output.Width = Convert.ToInt32(reader["width"]);
            }
            return output;
        }
        
    }
}
