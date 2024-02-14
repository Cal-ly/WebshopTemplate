using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebshopTemplate.Models
{
    public class Customer : IdentityUser
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? PostalCode { get; set; } = string.Empty;
        public string? Country { get; set; } = "Denmark";
        public string? Phone { get; set; } = string.Empty;

        /// <summary>
        /// List of orders placed by the customer.
        /// The list is initialized to avoid null reference exceptions, but can be empty.
        /// It could also be initialized in the constructor.
        /// The reason for using ICollection instead of List is that ICollection is more abstract and allows for more flexibility.
        /// This is useful when you want to change the implementation of the collection in the future.
        /// And it's also useful when you want to avoid circular references, which is why it is also has the <see langword="virtual"/>.
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Foreign key for Company
        public int? CompanyId { get; set; }

        // Navigation property for Company
        [ForeignKey("CompanyId")]
        public virtual Company? RepresentingCompany { get; set; }

        public string? Shopnotes { get; set; } = string.Empty;

        // Calculated properties
        public string? FullName => $"{FirstName} {LastName}";
        public string? FullAddress => $"{Address}, {PostalCode} {City}, {Country}";
    }
}
