using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IArtistsDao
    {
        public Artist GetArtist(Artist artist);
        public List<Artist> GetArtistsByDiscogsId(int discogId);
        public List<Artist> GetExtraArtistsByDiscogsId(int discogId);
        public int GetArtistCountByUsername(string username, bool isPremium);
        public int GetExtraArtistCountByUsername(string username, bool isPremium);
        public Dictionary<string, int> GetArtistAndRecordCountByUsername(string username, bool isPremium);
        public Dictionary<string, int> GetArtistAndRecordCount();
        public int GetArtistCount();
        public bool AddArtist(Artist artist);
    }
}
