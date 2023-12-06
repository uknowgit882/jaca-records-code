using Capstone.Models;

namespace Capstone.DAO.Interfaces
{
    public interface IRecordsFormatsDao
    {
        public bool AddRecordFormat(int discogsId, int formatId);
        public bool GetRecordFormatByRecordIdAndFormatId(int discogsId, int formatId);
    }
}
