using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BladeRazer.Data;
using BladeRazer.Models;

namespace BladeRazer.Pages.FormEdit.Customers
{
    public class IndexModel : PageModel
    {
        private readonly BladeRazer.Data.ApplicationDbContext _context;

        public IndexModel(BladeRazer.Data.ApplicationDbContext context)
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
