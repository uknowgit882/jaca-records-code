using Capstone.Exceptions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class SearchDao: ISearchDao
    {
        private readonly string connectionString;
        public SearchDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<int> WildcardSearchDatabaseForRecords(SearchRequest requestObject)
        {
            List<int> recordIDs = new List<int>();

            string sql = "SELECT * " +
                "FROM records " +
                "JOIN records_artists ON records.discogs_id = records_artists.discogs_id " +
                "JOIN artists ON records_artists.artist_id = artists.artist_id " +
                "JOIN records_extra_artists ON records.discogs_id = records_extra_artists.discogs_id " +
                "JOIN artists AS extra_artists ON records_extra_artists.extra_artist_id = extra_artists.artist_id" +
                "JOIN images ON records.discogs_id = images.discogs_id " +
                "JOIN records_labels ON records.discogs_id = records_labels.discogs_id " +
                "JOIN labels ON records_labels.label_id = labels.label_id " +
                "JOIN tracks ON records.discogs_id = tracks.discogs_id " +
                "JOIN barcodes ON records.discogs_id = barcodes.discogs_id " +
                "JOIN records_formats ON records.discogs_id = records_formats.discogs_id " +
                "JOIN formats ON records_formats.format_id = formats.format_id " +
                "JOIN records_genres ON records.discogs_id = records_genres.discogs_id " +
                "JOIN genres ON records_genres.genre_id = genres.genre_id " +
                "WHERE record.title LIKE @recordTitle AND artist.name LIKE @artistName " +
                "AND genre.name LIKE @genreName AND records.released LIKE @recordsReleased " +
                "AND record.country LIKE @recordCountry AND labels.name LIKE @labelsName " +
                "AND barcode.value LIKE @barcodeValue";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recordTitle", SearchStringWildcardAdder(requestObject.Title));
                    cmd.Parameters.AddWithValue("@artistName", SearchStringWildcardAdder(requestObject.Artist));
                    cmd.Parameters.AddWithValue("@genreName", SearchStringWildcardAdder(requestObject.Genre));
                    cmd.Parameters.AddWithValue("@recordsReleased", SearchStringWildcardAdder(requestObject.Year));
                    cmd.Parameters.AddWithValue("@recordCountry", SearchStringWildcardAdder(requestObject.Country));
                    cmd.Parameters.AddWithValue("@labelsName", SearchStringWildcardAdder(requestObject.Label));
                    cmd.Parameters.AddWithValue("@barcodeValue", SearchStringWildcardAdder(requestObject.Barcode));
                    SqlDataReader reader = cmd.ExecuteReader();

                    //if (reader.Read())
                    //{
                    //    recordIDs;
                    //}
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occured", ex);
            }
            

            return recordIDs;
        }

        protected string SearchStringWildcardAdder(string query)
        {
            return string.IsNullOrEmpty(query) ? "" : "%" + query + "%";
        }
        //        SELECT*
        //FROM records
        //JOIN records_artists ON records.discogs_id = records_artists.discogs_id
        //        JOIN artists ON records_artists.artist_id = artists.artist_id
        //JOIN records_extra_artists ON records.discogs_id = records_extra_artists.discogs_id
        //JOIN artists AS extra_artists ON records_extra_artists.extra_artist_id = extra_artists.artist_id
        //JOIN images ON records.discogs_id = images.discogs_id
        //JOIN records_labels ON records.discogs_id = records_labels.discogs_id
        //JOIN labels ON records_labels.label_id = labels.label_id
        //JOIN tracks ON records.discogs_id = tracks.discogs_id
        //JOIN barcodes ON records.discogs_id = barcodes.discogs_id
        //JOIN records_formats ON records.discogs_id = records_formats.discogs_id
        //JOIN formats ON records_formats.format_id = formats.format_id
        //JOIN records_genres ON records.discogs_id = records_genres.discogs_id
        //JOIN genres ON records_genres.genre_id = genres.genre_id















    }
}
