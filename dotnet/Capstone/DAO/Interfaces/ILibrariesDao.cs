using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface ILibrariesDao
    {
        public List<Library> GetPremiumUsersLibrary(string username);
        public List<Library> GetFreeUsersLibrary(string username);
        public List<Library> Get25MostRecentDiscogsIdsInLibrary(string username);
        public string GetNote(string username, int discogsId);
        public int GetQuantity(string username, int discogId);
        public int GetRecordCountByUsername(string username);
        public int GetRecordCountForAllUsers();
        public bool AddRecord(int discogsId, string username, string notes);
        public bool ChangeRecordIsPremium(int discogsId, string username, bool toggleIsPremium);
        public string ChangeNote(string username, int discogsId, string notes);
        public int ChangeQuantity(string username, int discogsId, int quantity);
        public bool DeleteRecord(int discogsId, string username);
        public bool DeReactivateLibrary(string username, bool isActive);
    }
}
