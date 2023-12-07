using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Image
    {
        [JsonIgnore]
        public int Image_Id { get; set; }
        [JsonIgnore]
        public int Discogs_Id { get; set; }
        public string Uri { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
