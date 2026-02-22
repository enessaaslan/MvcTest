namespace MvcApplication.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string ProductDescription { get; set; } = string.Empty;
        public int PhoneId { get; set; }
        public PhoneCategory? PhoneCategory { get; set; }
    }
}
