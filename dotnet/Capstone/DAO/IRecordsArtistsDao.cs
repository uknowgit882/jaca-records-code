using Capstone.Models;

namespace Capstone.DAO
{
    public interface IRecordsArtistsDao
    {
        public bool AddRecordArtist(int recordId, int artistId);
    }
}
