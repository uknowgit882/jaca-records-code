using Capstone.Models;

namespace Capstone.Service
{
    public interface ISearchService
    {
        public SearchResult SearchForRecord(SearchRequest searchObject);

    }
}
