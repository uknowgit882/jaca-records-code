using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Library
    {
        [JsonIgnore]
        public int Library_Id { get; set; }
        public int Username { get; set; }
        [JsonIgnore]
        public int Discog_Id { get; set; }
        public string Notes { get; set; }
        public int Quantity { get; set; } = 1;
        [JsonIgnore]
        public bool Is_Premium { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
        [JsonIgnore]
        public DateTime Created_Date { get; set; }
        [JsonIgnore]
        public DateTime Updated_Date { get; set; }
    }
    public class IncomingLibrary
    {
        public int DiscogsId { get; set; }
        public string Notes { get; set; } = "";
        public int Quantity { get; set; } = 1;
    }
}
