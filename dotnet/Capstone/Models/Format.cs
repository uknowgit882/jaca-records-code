using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Format
    {
        [JsonIgnore]
        public int Format_Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string Qty { get; set; }
        [JsonIgnore]
        public List<string> Descriptions { get; set; }
        public override string ToString()
        {
            return Name;
        }

    }
}
