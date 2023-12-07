using Capstone.Models;

namespace Capstone.Service
{
    public interface IRecordService
    {
        public RecordClient GetRecord(int release_id);
        public SearchResult SearchForRecordsDiscogs(SearchRequest searchObject, int pageNumber);
        public SearchRequest GenerateRequestObject(string q, string artist, string title, string genre, string year, string country, string label, string barcode);

    }
}
