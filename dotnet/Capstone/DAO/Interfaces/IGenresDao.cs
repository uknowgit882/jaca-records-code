using System.Collections.Generic;
using Capstone.Models;


namespace Capstone.DAO
{
    public interface IGenresDao
    {
        public Genre GetGenre(string genre);
        public List<string> GetGenresByDiscogsId(int discogId);
        public int GetGenreCountByUsername(string username, bool isPremium);
        public int GetGenreCount();
        public Dictionary<string, int> GetGenreAndRecordCountByUsername(string username, bool isPremium);
        public Dictionary<string, int> GetGenreAndRecordCount();
        public bool AddGenre(string genre);
    }
}
