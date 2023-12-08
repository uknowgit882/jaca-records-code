using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface ISearchDao
    {
        public List<int> WildcardAdvancedSearchDatabaseForRecords(SearchRequest requestObject, string username);
        public List<int> WildcardSearchDatabaseForRecords(string requestObject, string username);
        public List<int> WildcardSearchCollectionsForRecords(string requestObject, string username);
        public List<int> WildcardAdvancedSearchCollectionsForRecords(SearchRequest requestObject, string username);
    }
}
