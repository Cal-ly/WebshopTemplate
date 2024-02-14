namespace WebshopTemplate.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty; // Optional
        public ICollection<Product> Products { get; set; } = new List<Product>();
        // Consider adding a ParentCategoryId for hierarchical categories
    }
}
