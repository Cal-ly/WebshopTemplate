namespace WebshopTemplate.Pages.Basket
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets.FindAsync(id);
            if (basket != null)
            {
                Basket = basket;
                _context.Baskets.Remove(Basket);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
