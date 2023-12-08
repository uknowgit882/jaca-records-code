using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IFormatsDao
    {
        public Format GetFormat(string description);
        public List<Format> GetFormatsByDiscogsId(int discogId);
        public int GetFormatCountByUsername(string username);
        public int GetFormatCount();
        //public List<Format> GetFormatsByDiscogsIdAndUsername(int discogId, string username); // might not need
        public bool AddFormat(string description);
    }
}
