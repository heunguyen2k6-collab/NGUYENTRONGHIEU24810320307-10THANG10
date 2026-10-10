namespace BAITAPSANG10THANG10
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }

        public Product() { }

        public Product(string productId, string productName, decimal unitPrice, int quantity, string category)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Category = category;
        }
    }
}