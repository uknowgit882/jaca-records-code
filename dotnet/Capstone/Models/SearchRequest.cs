namespace Capstone.Models
{
    public class SearchRequest
    {
        public string Query { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }
        public string Country { get; set; }
        public string Label { get; set; }
        public string Barcode { get; set; }
        public string TypeOfSearch { get; set; }
    }
}
