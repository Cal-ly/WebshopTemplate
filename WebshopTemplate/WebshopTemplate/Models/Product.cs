namespace WebshopTemplate.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public decimal PriceTotal { get; set; } = 0;
        public int Quantity { get; set; } = 0;

        public string CategoryId { get; set; } = null!;
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;

        // Collection property for bidirectional relationship
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
