using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Collection
    {
        public int Collection_Id { get; set; }
        public int Username_Id { get; set; }
        public int Discog_Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
