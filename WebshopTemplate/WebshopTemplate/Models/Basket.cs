namespace WebshopTemplate.Models
{
    public class Basket
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!; // Unique identifier for the basket
        public string CustomerId { get; set; } = null!; // Unique identifier for the user
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } = null!; // Navigation property for the user
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
