using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BladeRazorExamples.Data;
using BladeRazorExamples.Models;

namespace BladeRazerExamples.Pages.FormEdit.Orders
{
    public class IndexModel : PageModel
    {
        private readonly BladeRazorExamples.Data.ApplicationDbContext _context;

        public IndexModel(BladeRazorExamples.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            Order = await _context.Orders
                .Include(o => o.Customer).ToListAsync();
        }
    }
}
