﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Collection
    {
        [JsonIgnore]
        public int Collection_Id { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
      
        public string Name { get; set; }
        [JsonIgnore]
        public bool IsPrivate { get; set; }
        [JsonIgnore]
        public bool IsPremium { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
        [JsonIgnore]
        public DateTime Created_Date { get; set; }
        [JsonIgnore]
        public DateTime Updated_Date { get; set; }
    }

    public class IncomingCollectionRequest
    {
        public string Name { get; set; } 
        public bool Is_Private { get; set; }
    }
    public class OutboundCollectionWithFullRecords
    {
        public string Name { get; set; }
        public bool Is_Private { get; set; }
        public List<RecordClient> Records { get; set; } = new List<RecordClient>();
    }
}
