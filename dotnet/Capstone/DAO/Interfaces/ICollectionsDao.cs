using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface ICollectionsDao
    {
        public List<Collection> GetAllCollections(string username, bool isPremium);
        public List<Collection> GetPubOrPrivAllCollections(string username, bool isPremium, bool isPrivate = false);
        public Collection GetNamedCollection(string username, string name, bool isPremium);
        public Collection GetPubOrPrivCollection(string username, string name, bool isPremium, bool isPrivate = false);
        public List<int> GetAllRecordsInCollectionByUsernameAndName(string username, string name, bool isPremium, bool isPrivate = false);
        public int GetCollectionCountByUsername(string username);
        public int GetCollectionCount();
        public int CountOfRecordsInSpecificCollectionByUsername(string username, string name);
        public int CountOfRecordsInAllCollectionsByUsername(string username);
        public int CountOfRecordsInAllCollections();
        public int AddCollection(string username, string name);
        public bool UpdateCollectionTitle(string name, string username, string newName);
        public bool DeleteCollection(string name, string username);
        public bool ChangeCollectionPrivacy(string name, string username, bool isPrivate);
        public bool ChangeCollectionIsPremium(string name, string username, bool isPremium);
        public bool DeReactivateCollection(string name, string username, bool isActive);
    }
}
