using System.ComponentModel.DataAnnotations.Schema;

namespace WebshopTemplate.Models
{
    public class Order
    {
        public int Id { get; set; } // Primary key for the order - ID of the order in the database

        // Foreign key for User. This is the ID of the Customer who placed the order
        public string UserId { get; set; } = null!;

        // Navigation property for User
        [ForeignKey("UserId")]
        public virtual Customer Customer { get; set; } = null!; // The customer who placed the order. Is virtual to allow for lazy loading. Lazy loading is the concept of delaying the loading of related data until you specifically request for it. This can be useful when you have a large amount of data and you don't want to load it all at once. It can also be useful when you want to avoid circular references. Lazy loading is enabled by default in Entity Framework Core.

        public OrderStatus Status { get; set; } = new OrderStatus(); // Status of the order (Submitted, Pending, Received, Confirmed, Processing, Ready, Shipped, Delivered, Cancelled)
        public string CommentFromUser { get; set; } = string.Empty;
        public string CommentFromShop { get; set; } = string.Empty;
        public List<OrderDetail>? OrderDetails { get; set; } = []; // List of order details - Contains the products and quantities of the order - List initialized to avoid null reference exceptions, but can be empty. Could also be initialized in the constructor
        public decimal Total => OrderDetails?.Sum(od => od.Price * od.Quantity) ?? 0; // Total price of the order, calculated from the order details in order to avoid inconsistencies
        public int StatusCode // Property to store the status of the order as an integer in the database
        {
            get => (int)Status;
            set => Status = (OrderStatus)value;
        }
    }
    public enum OrderStatus // Enum for the status of the order
    {
        Submitted = 1,
        Pending = 2,
        Received = 3,
        Confirmed = 4,
        Processing = 5,
        Ready = 6,
        Shipped = 7,
        Delivered = 8,
        Cancelled = 9
    }
}
