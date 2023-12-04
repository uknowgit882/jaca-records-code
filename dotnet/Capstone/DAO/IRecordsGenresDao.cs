using Capstone.Models;

namespace Capstone.DAO
{
    public interface IRecordsLabelsDao
    {
        public bool AddRecordLabel(int recordId, int labelId);
    }
}
