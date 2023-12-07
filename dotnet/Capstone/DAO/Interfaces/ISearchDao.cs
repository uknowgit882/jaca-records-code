using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface ISearchDao
    {
        public List<int> WildcardAdvancedSearchDatabaseForRecords(SearchRequest requestObject);
        public List<int> WildcardSearchDatabaseForRecords(string requestObject);

    }
}
