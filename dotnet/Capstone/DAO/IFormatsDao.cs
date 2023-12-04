using Capstone.Models;

namespace Capstone.DAO
{
    public interface IFormatsDao
    {
        public string GetFormat(string description);
        public bool AddFormat(string description);
    }
}
