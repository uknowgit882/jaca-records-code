using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Capstone.Models
{
    public class SearchResult
    {
        public Pagination Pagination { get; set; }
        public List<Result> Results { get; set; } = new List<Result>();
    }

    public class Pagination
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int PerPage { get; set; }
        public int Items { get; set; }
        public URLs Urls { get; set; }
    }

    public class Result
    {
        public string Country { get; set; }
        public string Year { get; set; }
        [JsonIgnore]
        public List<string> Format { get; set; } = new List<string>();
        public List<string> Label { get; set; } = new List<string>();
        [JsonIgnore]
        public string Type { get; set; }
        public List<string> Genre { get; set; } = new List<string>();
        public List<string> Style { get; set; } = new List<string>();
        public int Id { get; set; }
        public List<string> Barcode { get; set; } = new List<string>();
        [JsonIgnore]
        public UserData UserData { get; set; }
        [JsonIgnore]
        public int MasterId { get; set; }
        [JsonIgnore]
        public string MasterUrl { get; set; }
        [JsonIgnore]
        public string Uri { get; set; }
        [JsonIgnore]
        public string Catno { get; set; } // Category Number
        public string Title { get; set; }
        public string Thumb { get; set; }
        [JsonIgnore]
        public string CoverImage { get; set; }
        [JsonIgnore]
        public string ResourceUrl { get; set; }
        [JsonIgnore]
        public Community Community { get; set; }
    }

    public class UserData
    {
        public bool InWantlist { get; set; }
        public bool InCollection { get; set; }
    }

    public class Community
    {
        public int Want { get; set; }
        public int Have { get; set; }
    }

    public class URLs
    {
        public string Last { get; set; }
        public string Next { get; set; }
    }
}
