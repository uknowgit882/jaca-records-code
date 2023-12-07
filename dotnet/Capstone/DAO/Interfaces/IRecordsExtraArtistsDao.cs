using Capstone.Models;

namespace Capstone.DAO
{
    public interface IRecordsExtraArtistsDao
    {
        public bool GetRecordExtraArtistByRecordIdAndExtraArtistId(int discogsId, int extraArtistId);
        public bool AddRecordExtraArtist(int discogsId, int extraArtistId);
    }
}
