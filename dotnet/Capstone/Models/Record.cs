namespace Capstone.Models
{
    public class Record
    {
        public Artist Artists { get; set; }
        public string Title { get; set; }
        public string[] Genres { get; set; }
        public Identifier Identifiers { get; set; }
        public Label[] Labels { get; set; }
        public Image[] Images { get; set; }
        public string Country { get; set; }
        public Format[] Formats { get; set; }
        public string Released { get; set; }
        public Artist[] ExtraArtists { get; set; }
        public string URI { get; set; }
        public string Notes { get; set; }

    }

    public class Identifier
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class Artist
    {
        public string Name { get; set; }
    }

    public class Label
    {
        public string Name { get; set; }
        public string ResourceUrl { get; set; }
    }

    public class Image
    {
        public string Uri { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class Format
    {
        public string Name { get; set; }
        public string Qty { get; set; }
        public string[] Descriptions { get; set; }
    }

}
