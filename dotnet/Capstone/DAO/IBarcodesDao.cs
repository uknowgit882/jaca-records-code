using Capstone.Models;

namespace Capstone.DAO
{
    public interface IBarcodesDao
    {
        public Identifier GetIdentifier(Identifier identifier);
        public bool AddIdentifier(Identifier identifier);
    }
}
