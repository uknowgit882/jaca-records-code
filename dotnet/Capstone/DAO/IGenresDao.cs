using System.Collections.Generic;
using Capstone.Models;


namespace Capstone.DAO
{
    public interface IGenresDao
    {
        public Genre GetGenre(Genre genre);
        public bool AddGenre(Genre genre);
    }
}
