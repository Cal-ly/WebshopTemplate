using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebshopTemplate.Data;
using WebshopTemplate.Models;

namespace WebshopTemplate.Pages.Products;

public class EditModel : PageModel
{
    private readonly WebshopTemplate.Data.ApplicationDbContext _context;

    public EditModel(WebshopTemplate.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        Product = product;
       ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(Product.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool ProductExists(string id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
