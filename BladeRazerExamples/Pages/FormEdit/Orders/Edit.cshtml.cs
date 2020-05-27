using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BladeRazorExamples.Data;
using BladeRazorExamples.Models;

namespace BladeRazerExamples.Pages.FormEdit.Orders
{
    public class EditModel : PageModel
    {
        private readonly BladeRazorExamples.Data.ApplicationDbContext _context;
        private readonly IHtmlHelper _htmlHelper;

        public EditModel(BladeRazorExamples.Data.ApplicationDbContext context, IHtmlHelper htmlHelper)
        {
            _context = context;
            _htmlHelper = htmlHelper;
        }

        [BindProperty]
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

            var customers = new SelectList(_context.Customers, "Id", "FullName");
            var status = _htmlHelper.GetEnumSelectList<OrderStatus>();

            // add the select lists to a dictionary for the FormEditTagHelper 
            // these are then looked up using the property attributes on the Order class
            // this is only because we have two select lists. 
            // if we have only one you simply use asp-items as we do in the customer example
            var sd = new Dictionary<string, IEnumerable<SelectListItem>>()
            {
                { "Customers", customers },
                { "OrderStatus", status }
            };
            ViewData["SelectDictionary"] = sd;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
