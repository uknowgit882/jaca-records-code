namespace Capstone.DAO
{
    public class GenreSqlDao : IGenresDao
    {
        private readonly string connectionString;

        public GenreSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public bool GetGenre(string genre)
        {
            
            return true;
        }
        public bool AddGenre(string genre)
        {
            return true;
        }

    }
}
