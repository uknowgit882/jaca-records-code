using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ILibrariesDao
    {
        public List<Library> GetLibrary(string username);
        public bool AddRecord(int discogId, string username, string notes);
        public bool RemoveRecord(string username);
        public bool ChangeQuantity(string username);
        public bool ChangeNote(string username);
        public bool DeactivateLibrary(string username);
        public bool ReactivateLibrary(string username);
    }
}
