using System;

namespace Capstone.Models
{
    public class Genre
    {
        public int Genre_Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }

    public class RecordsGenres
    {
        public int Records_Genres_Id { get; set; }
    }
}
