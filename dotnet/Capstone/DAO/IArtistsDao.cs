using Capstone.Models;

namespace Capstone.DAO
{
    public interface IArtistsDao
    {
        public Artist GetArtist(Artist artist);
        public bool AddArtist(Artist artist);
    }
}
