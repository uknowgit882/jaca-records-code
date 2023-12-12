using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IRecordBuilderDao
    {
        public RecordTableData GetRecordByDiscogsId(int discogsId);
        public RecordTableData GetRecordByRecordId(int recordId);
        public int GetRecordCount();
        public int GetRecordCountByUsername(string username, bool isPremium);
        public Dictionary<string, int> GetYearAndRecordCount();
        public Dictionary<string, int> GetYearAndRecordCountByUsername(string username, bool isPremium);
        public Dictionary<string, int> GetCountryAndRecordCount();
        public Dictionary<string, int> GetCountryAndRecordCountByUsername(string username, bool isPremium);
        public RecordTableData AddRecord(RecordClient input);
        public RecordTableData ActivateRecord(int discogsId);
        public RecordTableData UpdateRecord(RecordClient input);
        public RecordTableData UpdateRecordDiscogsDateChanged(int discogsId, DateTime discogsDateChanged);
    }
}
