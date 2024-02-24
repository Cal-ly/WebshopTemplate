using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebshopTemplate.Data;
using WebshopTemplate.Models;

namespace WebshopTemplate.Pages.Companies;

public class DeleteModel : PageModel
{
    private readonly WebshopTemplate.Data.ApplicationDbContext _context;

    public DeleteModel(WebshopTemplate.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Company Company { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = await _context.Companies.FirstOrDefaultAsync(m => m.Id == id);

        if (company == null)
        {
            return NotFound();
        }
        else
        {
            Company = company;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = await _context.Companies.FindAsync(id);
        if (company != null)
        {
            Company = company;
            _context.Companies.Remove(Company);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
