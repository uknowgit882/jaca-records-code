using Capstone.Models;

namespace Capstone.DAO.Interfaces
{
    public interface IRecordsGenresDao
    {
        public bool AddRecordGenre(int discogsId, int genreId);
        public bool GetRecordGenreByRecordIdAndGenreId(int discogsId, int genreId);
    }
}
