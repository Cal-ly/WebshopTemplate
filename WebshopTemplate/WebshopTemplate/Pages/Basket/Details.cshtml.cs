namespace WebshopTemplate.Pages.Basket
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.Basket Basket { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets.FirstOrDefaultAsync(m => m.Id == id);
            if (basket == null)
            {
                return NotFound();
            }
            else
            {
                Basket = basket;
            }
            return Page();
        }
    }
}
