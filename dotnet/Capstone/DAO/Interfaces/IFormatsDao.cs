using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IFormatsDao
    {
        public Format GetFormat(string description);
        public bool AddFormat(string description);
        public List<Format> GetFormatsByDiscogsIdAndUsername(int discogId, string username);
    }
}
