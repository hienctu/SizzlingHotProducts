namespace SizzlingHotProducts.Models
{
    public class Sale
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public List<SaleEntry> Entries { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }
}
