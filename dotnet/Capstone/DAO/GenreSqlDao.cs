using Capstone.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace Capstone.DAO
{
    public class GenreSqlDao : IGenresDao
    {
        private readonly string connectionString;

        public GenreSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public string GetGenre(string genre)
        {
            string output = null;
            string sql = "SELECT genre_id, name, is_active, created_date, updated_date FROM genres " +
                "WHERE name = @name;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", genre);

                    //SqlDataAdapter reader = cmd.ExecuteReader();

                    //if (reader.Read())
                    //{
                    //    //output = MapToRowGenre(reader);
                    //}
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            return genre;
        }
        public bool AddGenre(string genre)
        {
            return true;
        }

        //private string MapToRowGenre(SqlDataReader reader)
        //{
        //    string genre = null;
            
        //}
    }
}
