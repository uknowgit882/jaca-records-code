using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IArtistsDao
    {
        public Artist GetArtist(Artist artist);
        public bool AddArtist(Artist artist);
        public List<Artist> GetArtistsByDiscogsIdAndUsername(int discogId, string username);
        public List<Artist> GetExtraArtistsByDiscogsIdAndUsername(int discogId, string username);
    }
}
