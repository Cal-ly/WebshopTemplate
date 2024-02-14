namespace WebshopTemplate.Models
{
    public class Company
    {
        public Company()
        {
            Representatives = new List<Customer>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public List<Customer> Representatives { get; set; }

        // Calculated properties
        public string FullCompanyAddress => $"{Address}, {PostalCode} {City}, {Country}";
        public string FullCompanyContact => $"{Email}, {Phone}, {Website}";
        public string AllRepresentatives => string.Join(", ", Representatives.Select(r => r.FullName));
    }
}
