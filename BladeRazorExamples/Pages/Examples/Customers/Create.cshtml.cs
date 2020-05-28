using BladeRazorExamples.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BladeRazorExamples.Pages.FormEdit.Customers
{
    public class CreateModel : PageModel
    {
        private readonly BladeRazorExamples.Data.ApplicationDbContext _context;

        public CreateModel(BladeRazorExamples.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer MyCustomer { get; set; }
        [BindProperty]
        public Customer Customer2 { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Customers.Add(MyCustomer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
