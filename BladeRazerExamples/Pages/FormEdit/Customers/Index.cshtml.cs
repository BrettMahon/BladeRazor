using BladeRazorExamples.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BladeRazorExamples.Pages.FormEdit.Customers
{
    public class IndexModel : PageModel
    {
        private readonly BladeRazorExamples.Data.ApplicationDbContext _context;

        public IndexModel(BladeRazorExamples.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get; set; }

        public async Task OnGetAsync()
        {
            Customer = await _context.Customers.ToListAsync();
        }
    }
}
