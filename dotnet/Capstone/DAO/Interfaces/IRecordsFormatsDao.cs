using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IRecordsFormatsDao
    {
        public bool AddRecordFormat(int discogsId, int formatId);
        public bool GetRecordFormatByRecordIdAndFormatId(int discogsId, int formatId);
    }
}
