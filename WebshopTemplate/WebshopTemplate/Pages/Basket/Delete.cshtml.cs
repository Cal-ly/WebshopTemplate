using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebshopTemplate.Data;
using WebshopTemplate.Models;

namespace WebshopTemplate.Pages.Basket
{
    public class DeleteModel : PageModel
    {
        private readonly WebshopTemplate.Data.ApplicationDbContext _context;

        public DeleteModel(WebshopTemplate.Data.ApplicationDbContext context)
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

            var basket = await _context.Basket.FirstOrDefaultAsync(m => m.Id == id);

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

            var basket = await _context.Basket.FindAsync(id);
            if (basket != null)
            {
                Basket = basket;
                _context.Basket.Remove(Basket);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
