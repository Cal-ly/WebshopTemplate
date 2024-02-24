namespace WebshopTemplate.Models;

public class Staff
{
    // The Id property is the primary key
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string? UserId { get; set; }
    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; } = string.Empty;
    [Display(Name = "Last Name")]
    public string? LastName { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; } = string.Empty;
    public string? Country { get; set; } = "Denmark";
    public string? Phone { get; set; } = string.Empty;
    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();

    // Work-specific details
    [DataType(DataType.Date),Display(Name = "Employment Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? EmploymentDate { get; set; }
    //[DataType(DataType.Date), Display(Name = "Termination Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //public DateTime? TerminationDate { get; set; }
    public decimal? BasePay { get; set; }
    public virtual string? Notes { get; set; } = string.Empty;
    public virtual string? ImageUrl { get; set; } = string.Empty;

    // Calculated properties
    public string? FullName => $"{FirstName} {LastName}";
    public string? FullAddress => $"{Address}, {PostalCode} {City}, {Country}";
}
