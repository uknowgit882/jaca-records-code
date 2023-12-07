using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Identifier
    {
        [JsonIgnore]
        public int Barcode_Id { get; set; }
        [JsonIgnore]
        public int Discogs_Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        [JsonIgnore]
        public string Description { get; set; }
    }
}
