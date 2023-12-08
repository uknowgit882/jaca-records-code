using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IRecordsCollectionsDao
    {
        public RecordCollection GetRecordCollectionByDiscogsIdAndCollectionId(int discogsId, int collectionId);
        public bool AddRecordCollections(int discogsId, int collectionId);
        public bool DeleteRecordCollectionByDiscogsIdAndCollectionId(int discogsID, int collectionId);
        public bool ChangeCollectionIsPremium(int discogsID, int collectionId, bool isPremium);
    }
}
