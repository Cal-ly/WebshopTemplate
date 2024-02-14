namespace WebshopTemplate.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        // The order that the order detail is part of also Foreign key for Order
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;

        // The product that was ordered also Foreign key for Product
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
