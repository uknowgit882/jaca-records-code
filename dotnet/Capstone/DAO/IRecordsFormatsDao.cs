using Capstone.Models;

namespace Capstone.DAO
{
    public interface IRecordsFormatsDao
    {
        public bool AddRecordFormat(int discogsId, int formatId);
    }
}
