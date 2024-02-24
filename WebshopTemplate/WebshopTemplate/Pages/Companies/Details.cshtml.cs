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

public class DetailsModel : PageModel
{
    private readonly WebshopTemplate.Data.ApplicationDbContext _context;

    public DetailsModel(WebshopTemplate.Data.ApplicationDbContext context)
    {
        _context = context;
    }

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
}
