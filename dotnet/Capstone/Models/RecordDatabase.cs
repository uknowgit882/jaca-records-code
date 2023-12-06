using System;

namespace Capstone.Models
{
    public class RecordTableData
    {
        public int Record_Id { get; set; }
        public int Discogs_Id { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
        public string Released { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public DateTime Discogs_Date_Changed { get; set; }
        public bool Is_Active { get; set; }

    }
}
