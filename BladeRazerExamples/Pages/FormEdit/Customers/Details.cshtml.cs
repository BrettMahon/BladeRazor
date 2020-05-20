using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BladeRazerExamples.Data;
using BladeRazerExamples.Models;

namespace BladeRazerExamples.Pages.FormEdit.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly BladeRazerExamples.Data.ApplicationDbContext _context;

        public DetailsModel(BladeRazerExamples.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);

            if (Customer == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
