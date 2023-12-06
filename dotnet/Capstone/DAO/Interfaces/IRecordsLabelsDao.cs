using Capstone.Models;

namespace Capstone.DAO
{
    public interface IRecordsLabelsDao
    {
        public bool AddRecordLabel(int discogsId, int labelId);
        public bool GetRecordLabelByLabelIdAndGenreId(int discogsId, int labelId);
    }
}
