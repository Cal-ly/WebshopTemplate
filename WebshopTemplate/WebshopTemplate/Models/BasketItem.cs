namespace WebshopTemplate.Models
{
    public class BasketItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!; // Unique identifier for the basket item

        public string BasketId { get; set; } = null!; // Unique identifier for the basket for EF relationship
        [ForeignKey("BasketId")]
        public Basket Basket { get; set; } = null!; // Navigation property for the basket

        public int Quantity { get; set; }
        public string ProductId { get; set; } = null!;
        public Product Product { get; set; } = null!; // Navigation property for the product
    }
}
