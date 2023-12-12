using System.Collections.Generic;

namespace Capstone.Models
{
    public class StatisticsAggregate : Statistics
    {
        public int TotalUsers { get; set; }
        
    }

    public class Statistics
    {
        public int TotalArtists { get; set; }
        public int TotalCollections { get; set; }
        public int TotalFormats { get; set; }
        public int TotalGenres { get; set; }
        public int TotalImages { get; set; }
        public int TotalLabels { get; set; }
        public int TotalRecords { get; set; }//
        public int TotalRecordsInCollections { get; set; }
        public int TotalTracks { get; set; }
        public Dictionary<string, int> NumRecordsByArtist { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> NumRecordsByFormat { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> NumRecordsByGenre { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> NumRecordsByLabel { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> NumRecordsByCountry { get; set; } = new Dictionary<string, int>();//
        public Dictionary<string, int> NumRecordsByYear { get; set; } = new Dictionary<string, int>();//
    }
}
