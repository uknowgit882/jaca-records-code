using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Genre
    {
        [JsonIgnore]
        public int Genre_Id { get; set; }
        public string Name { get; set; }
    }

}
