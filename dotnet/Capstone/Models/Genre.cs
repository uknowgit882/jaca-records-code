using System;
using System.Text.Json.Serialization;

namespace Capstone.Models
{
    public class Genre
    {
        [JsonIgnore]
        public int Genre_Id { get; set; }
        public string Name { get; set; }
    }

}
