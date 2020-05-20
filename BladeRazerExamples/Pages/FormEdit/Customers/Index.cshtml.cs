using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BladeRazerExamples.Data;
using BladeRazerExamples.Models;

using BladeRazer.TagHelpers;
using BladeRazer.Attributes;


namespace BladeRazerExamples.Pages.FormEdit.Customers
{
    public class IndexModel : PageModel
    {
        private readonly BladeRazerExamples.Data.ApplicationDbContext _context;

        public IndexModel(BladeRazerExamples.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; }

        public async Task OnGetAsync()
        {
            Customer = await _context.Customers.ToListAsync();
        }
    }
}
