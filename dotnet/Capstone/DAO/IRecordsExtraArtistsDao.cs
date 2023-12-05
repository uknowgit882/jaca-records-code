using Capstone.Models;

namespace Capstone.DAO
{
    public interface IRecordsExtraArtistsDao
    {
        public bool AddRecordExtraArtist(int discogsId, int extraArtistId);
    }
}
