using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ISearchDao
    {
        public List<int> WildcardSearchDatabaseForRecords(SearchRequest requestObject);
    }
}
