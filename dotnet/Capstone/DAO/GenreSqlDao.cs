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
            
            return genre;
        }
        public bool AddGenre(string genre)
        {
            return true;
        }

    }
}
