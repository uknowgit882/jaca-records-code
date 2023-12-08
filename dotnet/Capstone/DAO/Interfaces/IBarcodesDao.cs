using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IBarcodesDao
    {
        public Identifier GetIdentifier(Identifier identifier);
        public List<Identifier> GetIdentifiersByDiscogsId(int discogId);
        public int GetBarcodesCount();
        //public List<Identifier> GetIdentifiersByDiscogsIdAndUsername(int discogId, string username); // might not need
        public bool AddIdentifier(Identifier identifier);
        public Identifier UpdateIdentifier(Identifier updatedIdentifier);
    }
}
