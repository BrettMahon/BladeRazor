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
    public class DetailsModel : PageModel
    {
        private readonly BladeRazorExamples.Data.ApplicationDbContext _context;

        public DetailsModel(BladeRazorExamples.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.Customer).FirstOrDefaultAsync(m => m.Id == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
