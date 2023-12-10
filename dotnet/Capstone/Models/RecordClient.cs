using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    // this is the outbound object from the database that goes to the front end
    public class RecordClient
    {
        // this is the discogs_id
        public int Id { get; set; }
        public string Title { get; set; }
        public string Released { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
        public string URI { get; set; }
        public List<Artist> Artists { get; set; } = new List<Artist>();
        public List<Artist> ExtraArtists { get; set; } = new List<Artist>();
        public List<Format> Formats { get; set; } = new List<Format>();
        public List<string> Genres { get; set; } = new List<string>();
        public List<Identifier> Identifiers { get; set; } = new List<Identifier>();
        public List<Image> Images { get; set; } = new List<Image>();
        public List<Label> Labels { get; set; } = new List<Label>();
        public List<Track> Tracklist { get; set; } = new List<Track>();
        

        [JsonIgnore]
        public int Record_Id { get; set; }
        [JsonIgnore]
        public DateTime Date_Changed { get; set; }
        [JsonIgnore]
        public List<string> Styles { get; set; } = new List<string>();
        
        
        public override string ToString()
        {
            return Title + " | " + Tracklist.Count + " Tracks";
        }
    }

    public class IncomingRecord
    {
        public int Discogs_Id { get; set; }
    }
}
