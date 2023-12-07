using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface ICollectionsDao
    {
        public Collection GetCollection(string username, string name);
        public List<Collection> GetAllCollection(string username);
        public bool AddCollection(string username, string name, int? discogsId = null);
        public bool AddRecordToCollection(string name, int discogId, string username);
        public bool UpdateCollectionTitle(string name, string username);
        public bool RemoveSongFromCollection(string name, int discogsID, string username);
        public bool RemoveCollection(string name, string username);
        public bool PrivatizeCollection(string name, string username);
        public bool PublicizeCollection(string name, string username);
        public bool DeactivateCollection(string name, string username);
        public bool ReactivateCollection(string name, string username);
    }
}
