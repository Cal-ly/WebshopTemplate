namespace WebshopTemplate.Models
{
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        // The order that the order detail is part of also Foreign key for Order
        public string? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        // The product that was ordered also Foreign key for Product
        public string? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? ProductInOrder { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}