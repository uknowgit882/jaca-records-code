using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface ILibrariesDao
    {
        public List<Library> GetLibrary(string username);
        public bool AddRecord(int discogsId, string username, string notes);
        public bool RemoveRecord(int discogsId, string username);
        public int GetQuantity(string username, int discogId);
        public int ChangeQuantity(string username, int discogsId, int quantity);
        public string GetNote(string username, int discogsId);
        public string ChangeNote(string username, int discogsId, string notes);
        public bool DeactivateLibrary(string username);
        public bool ReactivateLibrary(string username);
    }
}
