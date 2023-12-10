using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IRecordsCollectionsDao
    {
        public List<RecordCollection> GetAllRecordsInCollectionByCollectionId(int collectionId);
        public List<int> GetAllCollectionsForThisDiscogsIdByUsername(int discogsId, string username);
        public RecordCollection GetRecordCollectionByDiscogsIdAndCollectionId(int discogsId, int collectionId);
        public bool AddRecordCollections(int discogsId, int collectionId);
        public bool DeleteRecordCollectionByDiscogsIdAndCollectionId(int discogsID, int collectionId);
        public bool DeleteAllRecordsInCollectionByCollectionId(int collectionId);
        public bool ChangeSingleRecordCollectionIsPremium(int discogsId, string username, bool isPremium);
        public bool ChangeAllRecordCollectionIsPremium(string username, bool isPremium);
        public bool DeReactivateRecordsInCollection(string username, bool isActive);
    }
}
