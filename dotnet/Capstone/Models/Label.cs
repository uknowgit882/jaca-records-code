using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Label
    {
        [JsonIgnore]
        public int Label_Id { get; set; }
        public string Name { get; set; }
        public string Resource_Url { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
