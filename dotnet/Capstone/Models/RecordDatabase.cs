using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class RecordTableData
    {
        [JsonIgnore]
        public int Record_Id { get; set; }
        public int Discogs_Id { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
        public string Released { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        [JsonIgnore]
        public DateTime Discogs_Date_Changed { get; set; }
        [JsonIgnore]
        public bool Is_Active { get; set; }

    }
}
