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
    public class DeleteModel : PageModel
    {
        private readonly BladeRazer.Data.ApplicationDbContext _context;

        public DeleteModel(BladeRazer.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer = await _context.Customers.FindAsync(id);

            if (Customer != null)
            {
                _context.Customers.Remove(Customer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
