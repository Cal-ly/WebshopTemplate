namespace WebshopTemplate.Models
{
    public class Staff : IdentityUser
    {
        // Id and Email is already part of IdentityUser
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? PostalCode { get; set; } = string.Empty;
        public string? Country { get; set; } = "Denmark";
        public string? Phone { get; set; } = string.Empty;

        // Work-specific details
        public DateTime? EmploymentDate { get; set; } = DateTime.Now;
        public DateTime? TerminationDate { get; set; } = null;
        public decimal BasePay { get; set; } = 0;
        public string? Notes { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;

        // Relation to Orders they've worked on. It can be empty, but not null.
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Calculated properties
        public string? FullName => $"{FirstName} {LastName}";
        public string? FullAddress => $"{Address}, {PostalCode} {City}, {Country}";
    }
}
