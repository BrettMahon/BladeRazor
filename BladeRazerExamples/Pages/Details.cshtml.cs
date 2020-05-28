using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BladeRazorExamples.Data;
using BladeRazorExamples.Models;

namespace BladeRazorExamples.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly BladeRazorExamples.Data.ApplicationDbContext _context;

        public DetailsModel(BladeRazorExamples.Data.ApplicationDbContext context)
        {
            _context = context;
        }

       
        public dynamic Anything { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Anything = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
             
            if (Anything == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
