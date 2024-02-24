namespace WebshopTemplate.Models;

public class Category
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty; // Optional
    public ICollection<Product> Products { get; set; } = new List<Product>();
    // Consider adding a ParentCategoryId for hierarchical categories
}
