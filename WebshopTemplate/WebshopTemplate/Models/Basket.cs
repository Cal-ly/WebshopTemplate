namespace WebshopTemplate.Models
{
    public class Basket
    {
        public string BasketId { get; set; } = null!; // Unique identifier for the basket
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
