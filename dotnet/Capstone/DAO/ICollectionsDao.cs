using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICollectionsDao
    {
        public Collection GetCollection(Collection collection);
        public List<Collection> GetAllCollection(List<Collection> collection);
        public bool AddCollection(Collection collection);
        public bool UpdateCollection(string username);
        public bool AddRecordToCollection(string username);
        public bool DeleteRecordFromCollection(string username);
        public bool PrivatizeCollection(string username);
        public bool PublicizeCollection(string username);
        public bool DeactivateCollection(string username);
        public bool RaeactivateCollection(string username);
    }
}
