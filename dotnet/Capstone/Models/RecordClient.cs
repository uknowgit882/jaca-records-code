using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class RecordClient
    {
        [JsonIgnore]
        public int Record_Id { get; set; }
        // this is the discogs_id
        public int Id { get; set; }
        public string URI { get; set; }
        public List<Artist> Artists { get; set; } = new List<Artist>();
        public List<Label> Labels { get; set; } = new List<Label>();
        public List<Format> Formats { get; set; } = new List<Format>();
        public string Title { get; set; }
        public string Country { get; set; }
        [JsonIgnore]
        public DateTime Date_Changed { get; set; }
        public string Released { get; set; }
        public List<Identifier> Identifiers { get; set; } = new List<Identifier>();
        public List<string> Genres { get; set; } = new List<string>();
        public List<string> Styles { get; set; } = new List<string>();
        public List<Track> Tracklist { get; set; } = new List<Track>();
        public List<Artist> ExtraArtists { get; set; } = new List<Artist>();
        public List<Image> Images { get; set; } = new List<Image>();
        public string Notes { get; set; }
        public override string ToString()
        {
            return Title + " | " + Tracklist.Count + " Tracks";
        }
    }
}
