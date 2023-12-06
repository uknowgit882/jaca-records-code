using Capstone.Models;

namespace Capstone.DAO.Interfaces
{
    public interface IFormatsDao
    {
        public Format GetFormat(string description);
        public bool AddFormat(string description);
    }
}
