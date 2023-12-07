using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IBarcodesDao
    {
        public Identifier GetIdentifier(Identifier identifier);
        public bool AddIdentifier(Identifier identifier);
        public Identifier UpdateIdentifier(Identifier updatedIdentifier);
        public List<Identifier> GetIdentifiersByDiscogsIdAndUsername(int discogId, string username);
    }
}
