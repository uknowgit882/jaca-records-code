using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface IArtistsDao
    {
        public Artist GetArtist(Artist artist);
        public bool AddArtist(Artist artist);
        public List<Artist> GetArtistsByDiscogsId(int discogId);
        public List<Artist> GetExtraArtistsByDiscogsId(int discogId);
        public int GetArtistCountByUsername(string username);
        public int GetExtraArtistCountByUsername(string username);
        public Dictionary<string, int> GetArtistAndRecordCountByUsername(string username);
        public Dictionary<string, int> GetArtistAndRecordCount();
        public int GetArtistCount();
        //public List<Artist> GetArtistsByDiscogsIdAndUsername(int discogId, string username); // might not need
        //public List<Artist> GetExtraArtistsByDiscogsIdAndUsername(int discogId, string username); // might not need
    }
}
