using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Track
    {
        [JsonIgnore]
        public int Track_Id { get; set; }
        [JsonIgnore]
        public int Discogs_Id { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Duration { get; set; }
        public override string ToString()
        {
            return Position + " | " + Title;
        }
    }
}
