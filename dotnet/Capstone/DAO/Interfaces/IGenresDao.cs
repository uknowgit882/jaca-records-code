using System.Collections.Generic;
using Capstone.Models;


namespace Capstone.DAO
{
    public interface IGenresDao
    {
        public Genre GetGenre(string genre);
        public List<string> GetGenresByDiscogsId(int discogId);
        public int GetGenreCountByUsername(string username);
        public int GetGenreCount();
        //public List<string> GetGenresByDiscogsIdAndUsername(int discogId, string username); // might not need this
        public bool AddGenre(string genre);
    }
}
