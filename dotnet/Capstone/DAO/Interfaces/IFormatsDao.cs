using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IFormatsDao
    {
        public Format GetFormat(string description);
        public List<Format> GetFormatsByDiscogsId(int discogId);
        public int GetFormatCountByUsername(string username, bool isPremium);
        public int GetFormatCount();
        public Dictionary<string, int> GetFormatAndRecordCountByUsername(string username, bool isPremium);
        public Dictionary<string, int> GetFormatAndRecordCount();
        public bool AddFormat(string description);
    }
}
