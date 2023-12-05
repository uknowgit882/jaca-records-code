using Capstone.Models;

namespace Capstone.Service
{
    public interface IRecordService
    {
        public RecordClient GetRecord(int release_id);
        public SearchResult SearchForRecordsDiscogs(SearchRequest searchObject);
    }
}
