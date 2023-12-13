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
    public class SearchSqlDao : ISearchDao
    {
        private readonly string connectionString;
        public SearchSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<int> WildcardAdvancedSearchDatabaseForRecords(SearchRequest requestObject, string username)
        {
            List<int> recordIDs = new List<int>();
            if (!string.IsNullOrEmpty(requestObject.Query))
            {
                List<int> recordIDsFromQuerySearch = WildcardSearchDatabaseForRecords(requestObject.Query, username);
                for (int i = 0; i < recordIDsFromQuerySearch.Count; i++)
                {
                    recordIDs.Add(recordIDsFromQuerySearch[i]);
                }
            }

            string sql = "SELECT records.discogs_id " +
           "FROM records " +
           "LEFT JOIN records_artists ON records.discogs_id = records_artists.discogs_id " +
           "LEFT JOIN artists ON records_artists.artist_id = artists.artist_id " +
           "LEFT JOIN records_labels ON records.discogs_id = records_labels.discogs_id " +
           "LEFT JOIN labels ON records_labels.label_id = labels.label_id " +
           "LEFT JOIN barcodes ON records.discogs_id = barcodes.discogs_id " +
           "LEFT JOIN records_genres ON records.discogs_id = records_genres.discogs_id " +
           "LEFT JOIN genres ON records_genres.genre_id = genres.genre_id " +
           "JOIN libraries ON libraries.discogs_id = records.discogs_id " +
           "JOIN users on libraries.username = users.username " +
           "WHERE users.username = @username " +
           "AND (records.title LIKE @recordsTitle OR @recordsTitle = '') " +
           "AND (artists.name LIKE @artistsName OR @artistsName = '') " +
           "AND (genres.name LIKE @genresName OR @genresName = '') " +
           "AND (records.released LIKE @recordsReleased OR @recordsReleased = '') " +
           "AND (records.country LIKE @recordsCountry OR @recordsCountry = '') " +
           "AND (labels.name LIKE @labelsName OR @labelsName = '') " +
           "AND (barcodes.value LIKE @barcodesValue OR @barcodesValue = '')" +
           "GROUP BY records.discogs_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@recordsTitle", SearchStringWildcardAdder(requestObject.Title));
                    cmd.Parameters.AddWithValue("@artistsName", SearchStringWildcardAdder(requestObject.Artist));
                    cmd.Parameters.AddWithValue("@genresName", SearchStringWildcardAdder(requestObject.Genre));
                    cmd.Parameters.AddWithValue("@recordsReleased", SearchStringWildcardAdder(requestObject.Year));
                    cmd.Parameters.AddWithValue("@recordsCountry", SearchStringWildcardAdder(requestObject.Country));
                    cmd.Parameters.AddWithValue("@labelsName", SearchStringWildcardAdder(requestObject.Label));
                    cmd.Parameters.AddWithValue("@barcodesValue", SearchStringWildcardAdder(requestObject.Barcode));
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int idToAdd = Convert.ToInt32(reader["discogs_id"]);
                        if (!recordIDs.Contains(idToAdd))
                        {
                            recordIDs.Add(idToAdd);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to perform advanced wildcard search on database", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }


            return recordIDs;
        }

        public List<int> WildcardSearchDatabaseForRecords(string requestObject, string username)
        {
            List<int> recordIDs = new List<int>();

            List<string> requestObjectWords = searchWords(requestObject);

            string sql = null;
            string singleParamQuery = null;
            for (int i = 0; i < requestObjectWords.Count; i++)
            {
                singleParamQuery = $"@querySearch{i}";
                sql = "SELECT records.discogs_id " +
                        "FROM records " +
                        "JOIN records_artists ON records.discogs_id = records_artists.discogs_id " +
                        "JOIN artists ON records_artists.artist_id = artists.artist_id " +
                        "JOIN records_labels ON records.discogs_id = records_labels.discogs_id " +
                        "JOIN labels ON records_labels.label_id = labels.label_id " +
                        "JOIN barcodes ON records.discogs_id = barcodes.discogs_id " +
                        "JOIN records_genres ON records.discogs_id = records_genres.discogs_id " +
                        "JOIN genres ON records_genres.genre_id = genres.genre_id " +
                        "JOIN libraries ON libraries.discogs_id = records.discogs_id " +
                        "JOIN users on libraries.username = users.username " +
                        "WHERE users.username = @username " +
                        $"AND records.title LIKE @querySearch{i} " +
                        $"OR artists.name LIKE @querySearch{i} " +
                        $"OR genres.name LIKE @querySearch{i} " +
                        $"OR records.released LIKE @querySearch{i} " +
                        $"OR records.country LIKE @querySearch{i} " +
                        $"OR labels.name LIKE @querySearch{i} " +
                        $"OR barcodes.value LIKE @querySearch{i}";
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue($"@querySearch{i}", SearchStringWildcardAdder(requestObjectWords[i]));
                        cmd.Parameters.AddWithValue("@username", username);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int idToAdd = Convert.ToInt32(reader["discogs_id"]);
                            if (!recordIDs.Contains(idToAdd))
                            {
                                recordIDs.Add(idToAdd);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteLog("Trying to perform wildcard search on database", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw;
                }
            }

            return recordIDs;
        }

        public List<int> WildcardSearchCollectionsForRecords(string requestObject, string username)
        {
            List<int> recordIDs = new List<int>();

            List<string> requestObjectWords = searchWords(requestObject);

            string sql = null;
            string singleParamQuery = null;
            for (int i = 0; i < requestObjectWords.Count; i++)
            {
                singleParamQuery = $"@querySearch{i}";
                sql = "SELECT records.discogs_id " +
                        "FROM records " +
                        "JOIN records_artists ON records.discogs_id = records_artists.discogs_id " +
                        "JOIN artists ON records_artists.artist_id = artists.artist_id " +
                        "JOIN records_labels ON records.discogs_id = records_labels.discogs_id " +
                        "JOIN labels ON records_labels.label_id = labels.label_id " +
                        "JOIN barcodes ON records.discogs_id = barcodes.discogs_id " +
                        "JOIN records_genres ON records.discogs_id = records_genres.discogs_id " +
                        "JOIN genres ON records_genres.genre_id = genres.genre_id " +
                        "JOIN records_collections on records.discogs_id = records_collections.discogs_id " +
                        "JOIN collections on records_collections.collection_id = collections.collection_id " +
                        "WHERE collections.username = @username " +
                        $"AND records.title LIKE @querySearch{i} " +
                        $"OR artists.name LIKE @querySearch{i} " +
                        $"OR genres.name LIKE @querySearch{i} " +
                        $"OR records.released LIKE @querySearch{i} " +
                        $"OR records.country LIKE @querySearch{i} " +
                        $"OR labels.name LIKE @querySearch{i} " +
                        $"OR barcodes.value LIKE @querySearch{i}";
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue($"@querySearch{i}", SearchStringWildcardAdder(requestObjectWords[i]));
                        cmd.Parameters.AddWithValue("@username", username);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int idToAdd = Convert.ToInt32(reader["discogs_id"]);
                            if (!recordIDs.Contains(idToAdd))
                            {
                                recordIDs.Add(idToAdd);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteLog("Trying to perform wildcard search on database", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                    throw;
                }
            }
            return recordIDs;
        }

        public List<int> WildcardAdvancedSearchCollectionsForRecords(SearchRequest requestObject, string username)
        {
            List<int> recordIDs = new List<int>();
            if (!string.IsNullOrEmpty(requestObject.Query))
            {
                List<int> recordIDsFromQuerySearch = WildcardSearchDatabaseForRecords(requestObject.Query, username);
                for (int i = 0; i < recordIDsFromQuerySearch.Count; i++)
                {
                    recordIDs.Add(recordIDsFromQuerySearch[i]);
                }
            }
            string sql = "SELECT records.discogs_id " +
                            "FROM records " +
                            "LEFT JOIN records_artists ON records.discogs_id = records_artists.discogs_id " +
                            "LEFT JOIN artists ON records_artists.artist_id = artists.artist_id " +
                            "LEFT JOIN records_labels ON records.discogs_id = records_labels.discogs_id " +
                            "LEFT JOIN labels ON records_labels.label_id = labels.label_id " +
                            "LEFT JOIN barcodes ON records.discogs_id = barcodes.discogs_id " +
                            "LEFT JOIN records_genres ON records.discogs_id = records_genres.discogs_id " +
                            "LEFT JOIN genres ON records_genres.genre_id = genres.genre_id " +
                            "JOIN records_collections on records.discogs_id = records_collections.discogs_id " +
                            "JOIN collections on records_collections.collection_id = collections.collection_id " +
                            "WHERE collections.username = @username " +
                            "AND (records.title LIKE @recordsTitle OR @recordsTitle = '') " +
                            "AND (artists.name LIKE @artistsName OR @artistsName = '') " +
                            "AND (genres.name LIKE @genresName OR @genresName = '') " +
                            "AND (records.released LIKE @recordsReleased OR @recordsReleased = '') " +
                            "AND (records.country LIKE @recordsCountry OR @recordsCountry = '') " +
                            "AND (labels.name LIKE @labelsName OR @labelsName = '') " +
                            "AND (barcodes.value LIKE @barcodesValue OR @barcodesValue = '')" +
                            "GROUP BY records.discogs_id";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@recordsTitle", SearchStringWildcardAdder(requestObject.Title));
                    cmd.Parameters.AddWithValue("@artistsName", SearchStringWildcardAdder(requestObject.Artist));
                    cmd.Parameters.AddWithValue("@genresName", SearchStringWildcardAdder(requestObject.Genre));
                    cmd.Parameters.AddWithValue("@recordsReleased", SearchStringWildcardAdder(requestObject.Year));
                    cmd.Parameters.AddWithValue("@recordsCountry", SearchStringWildcardAdder(requestObject.Country));
                    cmd.Parameters.AddWithValue("@labelsName", SearchStringWildcardAdder(requestObject.Label));
                    cmd.Parameters.AddWithValue("@barcodesValue", SearchStringWildcardAdder(requestObject.Barcode));
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int idToAdd = Convert.ToInt32(reader["discogs_id"]);
                        if (!recordIDs.Contains(idToAdd))
                        {
                            recordIDs.Add(idToAdd);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorLog.WriteLog("Trying to perform advanced wildcard search on database", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw new DaoException("Sql exception occured", ex);
            }

            return recordIDs;
        }

        protected string SearchStringWildcardAdder(string query)
        {
            return string.IsNullOrEmpty(query) ? "" : "%" + query + "%";
        }

        protected List<string> searchWords(string searchString)
        {
            List<string> searchStringWords = new List<string>();
            if (searchString != null)
            {
                string[] searchStringSplit = searchString.Split(' ');
                foreach (string word in searchStringSplit)
                {
                    if (word != "the" && word != "a" && word != "an")
                    {
                        searchStringWords.Add(word);
                    }
                }
            }
            return searchStringWords;
        }
    }
}
