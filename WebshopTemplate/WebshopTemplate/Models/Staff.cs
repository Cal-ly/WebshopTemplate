﻿namespace WebshopTemplate.Models
{
    public class Staff
    {
        // The Id property is the primary key
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; } = null!;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? PostalCode { get; set; } = string.Empty;
        public string? Country { get; set; } = "Denmark";
        public string? Phone { get; set; } = string.Empty;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Work-specific details
        public DateTime? EmploymentDate { get; set; } = DateTime.Now;
        public DateTime? TerminationDate { get; set; } = null;
        public decimal BasePay { get; set; } = 0;
        public virtual string? Notes { get; set; } = string.Empty;
        public virtual string? ImageUrl { get; set; } = string.Empty;


        // Calculated properties
        public string? FullName => $"{FirstName} {LastName}";
        public string? FullAddress => $"{Address}, {PostalCode} {City}, {Country}";
    }
}
