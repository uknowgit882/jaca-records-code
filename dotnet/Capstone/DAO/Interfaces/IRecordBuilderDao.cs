using Capstone.Models;
using System;

namespace Capstone.DAO
{
    public interface IRecordBuilderDao
    {
        public RecordTableData GetRecordByDiscogsId(int discogsId);
        public RecordTableData GetRecordByRecordId(int recordId);
        public RecordTableData GetRecordByDiscogsIdAndUsername(int discogsId, string username);
        public RecordTableData AddRecord(RecordClient input);
        public RecordTableData ActivateRecord(int discogsId);
        public RecordTableData UpdateRecord(RecordClient input);
        public RecordTableData UpdateRecordDiscogsDateChanged(int discogsId, DateTime discogsDateChanged);
    }
}
