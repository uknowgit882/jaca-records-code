using Capstone.Exceptions;
using Capstone.Models;
using System.Data.SqlClient;
using System;
using Capstone.DAO.Interfaces;

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

        public Image GetImageInfo(Image image)
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
                throw new DaoException("Sql exception occured", ex);
            }

            return output;
        }
        public bool AddImage(Image image)
        {
            Image checkedImage = GetImageInfo(image);

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
            catch (Exception e)
            {
                throw new DaoException("exception occurred", e);
            }
        }

        private Image MapRowToImage(SqlDataReader reader)
        {
            Image output = new Image();
            output.Image_Id = Convert.ToInt32(reader["image_id"]);
            output.Discogs_Id = Convert.ToInt32(reader["discogs_id"]);
            if (reader["uri"] is DBNull)
            {
                output.Uri = "";
            }
            else
            {
                output.Uri = Convert.ToString(reader["uri"]);
            }
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
