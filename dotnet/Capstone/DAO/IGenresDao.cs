using System.Collections.Generic;
using Capstone.Models;


namespace Capstone.DAO
{
    public interface IGenresDao
    {
        public Genre GetGenre(string genre);
        public bool AddGenre(string genre);
    }
}
