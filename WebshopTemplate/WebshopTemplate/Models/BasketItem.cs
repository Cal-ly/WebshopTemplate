namespace WebshopTemplate.Models
{
    public class BasketItem
    {
        public int Id { get; set; } // Unique identifier for the basket item
        public string BasketId { get; set; } = null!; // Unique identifier for the basket for EF relationship
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!; // Navigation property for the product
    }
}
