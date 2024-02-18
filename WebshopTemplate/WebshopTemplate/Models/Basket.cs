namespace WebshopTemplate.Models
{
    public class Basket
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!; // Unique identifier for the basket
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
