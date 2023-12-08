using System.Text.Json.Serialization;

namespace Capstone.Models
{
    public class RecordCollection
    {
        [JsonIgnore]
        public int Collection_Id { get; set; }
        [JsonIgnore]
        public int Discogs_Id { get; set; }
        [JsonIgnore]
        public bool Is_Premium { get; set; }

    }
}
