﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebshopTemplate.Data;
using WebshopTemplate.Models;

namespace WebshopTemplate.Pages.Products;

public class IndexModel : PageModel
{
    private readonly WebshopTemplate.Data.ApplicationDbContext _context;

    public IndexModel(WebshopTemplate.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Product> Product { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Product = await _context.Products
            .Include(p => p.Category).ToListAsync();
    }
}
