using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BladeRazorExamples.Data;
using BladeRazorExamples.Models;

using BladeRazor.TagHelpers;
using BladeRazor.Attributes;


namespace BladeRazorExamples.Pages.FormEdit.Customers
{
    public class IndexModel : PageModel
    {
        private readonly BladeRazorExamples.Data.ApplicationDbContext _context;

        public IndexModel(BladeRazorExamples.Data.ApplicationDbContext context)
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
