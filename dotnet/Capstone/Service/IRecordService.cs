using Capstone.Models;

namespace Capstone.Service
{
    public interface IRecordService
    {
       public Record GetRecord(int release_id);
    }
}
