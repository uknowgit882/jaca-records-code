using Capstone.Models;

namespace Capstone.DAO
{
    public interface IRecordsGenresDao
    {
        public bool AddRecordGenre(int discogsId, int genreId);
    }
}
