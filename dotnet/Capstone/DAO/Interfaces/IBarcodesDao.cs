using Capstone.Models;

namespace Capstone.DAO.Interfaces
{
    public interface IBarcodesDao
    {
        public Identifier GetIdentifier(Identifier identifier);
        public bool AddIdentifier(Identifier identifier);
    }
}
