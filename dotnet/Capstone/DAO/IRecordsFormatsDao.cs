using Capstone.Models;

namespace Capstone.DAO
{
    public interface IRecordsFormatsDao
    {
        public bool AddRecordFormat(int recordId, int formatId);
    }
}
