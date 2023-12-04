using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class RecordClient
    {
        public int Id { get; set; }
        public string URI { get; set; }
        public List<Artist> Artists { get; set; } = new List<Artist>();
        public List<Label> Labels { get; set; } = new List<Label>();
        public List<Format> Formats { get; set; } = new List<Format>();
        public string Title { get; set; }
        public string Country { get; set; }
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

    public class Artist
    {
        public int Artist_Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Label
    {
        public int Label_Id { get; set; }
        public string Name { get; set; }
        public string Resource_Url { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Format
    {
        public int Format_Id { get; set; }
        public string Name { get; set; }
        public string Qty { get; set; }
        public List<string> Descriptions { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Identifier
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
    public class Track
    {
        public int Track_Id { get; set; }
        public int Record_Id { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Duration { get; set; }
        public override string ToString()
        {
            return Position + " | " + Title ;
        }
    }

    public class Image
    {
        public string Uri { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }

    

}
