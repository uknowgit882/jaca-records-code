using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IRecordBuilderDao
    {
        public RecordTableData GetRecordByDiscogsId(int discogsId);
        public RecordTableData GetRecordByRecordId(int recordId);
        //public RecordTableData GetRecordByDiscogsIdAndUsername(int discogsId, string username); // might not need this
        public int GetRecordCount();
        public Dictionary<string, int> GetYearAndRecordCount();
        public Dictionary<string, int> GetCountryAndRecordCount();
        public RecordTableData AddRecord(RecordClient input);
        public RecordTableData ActivateRecord(int discogsId);
        public RecordTableData UpdateRecord(RecordClient input);
        public RecordTableData UpdateRecordDiscogsDateChanged(int discogsId, DateTime discogsDateChanged);
    }
}
