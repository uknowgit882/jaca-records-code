using Capstone.Models;
namespace Capstone.DAO.Interfaces
{
    public interface ISearchDao
    {
        public List<int> WildcardSearchDatabaseForRecords(SearchRequest requestObject);
    }
}
