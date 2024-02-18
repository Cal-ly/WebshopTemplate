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
    public class DetailsModel : PageModel
    {
        private readonly WebshopTemplate.Data.ApplicationDbContext _context;

        public DetailsModel(WebshopTemplate.Data.ApplicationDbContext context)
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
    }
}
