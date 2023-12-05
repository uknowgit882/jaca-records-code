﻿using System.Collections.Generic;

namespace Capstone.Models
{
    public class SearchResult
    {
        public List<Result> Results { get; set; } = new List<Result>();
    }

    public class Result
    {
        public string Country { get; set; }
        public string Year { get; set; }
        public List<string> Format { get; set; } = new List<string>();
        public List<string> Label { get; set; } = new List<string>();
        public string Type { get; set; }
        public List<string> Genre { get; set; } = new List<string>();
        public List<string> Style { get; set; } = new List<string>();
        public int Id { get; set; }
        public List<string> Barcode { get; set; } = new List<string>();
        // public UserData UserData { get; set; }
        public int MasterId { get; set; }
        public string MasterUrl { get; set; }
        public string Uri { get; set; }
        public string Catno { get; set; } // Category Number
        public string Title { get; set; }
        public string Thumb { get; set; }
        public string CoverImage { get; set; }
        public string ResourceUrl { get; set; }
        // public Community Community

    }
}
