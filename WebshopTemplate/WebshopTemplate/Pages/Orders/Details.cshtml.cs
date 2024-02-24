namespace WebshopTemplate.Pages.Orders;

public class DetailsModel : PageModel
{
    private readonly WebshopTemplate.Data.ApplicationDbContext _context;

    public DetailsModel(WebshopTemplate.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Order Order { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
        if (order == null)
        {
            return NotFound();
        }
        else
        {
            Order = order;
        }
        return Page();
    }
}
