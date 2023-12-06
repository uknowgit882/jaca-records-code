using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Capstone.Models
{
    public class Library
    {
        public int Library_Id { get; set; }
        public int Username { get; set; }
        public int Discog_Id { get; set; }
        public string Notes { get; set; }
        public int Quantity { get; set; } = 1;
        public bool IsActive { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
