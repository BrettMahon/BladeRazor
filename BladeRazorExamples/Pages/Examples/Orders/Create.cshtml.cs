using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BladeRazorExamples.Data;
using BladeRazorExamples.Models;

namespace BladeRazerExamples.Pages.FormEdit.Orders
{
    public class CreateModel : PageModel
    {
        private readonly BladeRazorExamples.Data.ApplicationDbContext _context;
        private readonly IHtmlHelper _htmlHelper;


        public CreateModel(BladeRazorExamples.Data.ApplicationDbContext context, IHtmlHelper htmlHelper)
        {
            _context = context;
            _htmlHelper = htmlHelper;
        }

        public IActionResult OnGet()
        {

            var customers = new SelectList(_context.Customers, "Id", "FullName");
            ViewData["Customers"] = customers;
            var status = _htmlHelper.GetEnumSelectList<OrderStatus>();
            ViewData["OrderStatus"] = status;

            // add the select lists to a dictionary for the FormEditTagHelper 
            // these are then looked up using the property attributes on the Order class
            // this is only because we have two select lists. 
            // if we have only one you simply use asp-items as we do in the customer example
            ViewData["SelectDictionary"] = new Dictionary<string, IEnumerable<SelectListItem>>()
                { { "Customers", customers }, { "OrderStatus", status } };

            return Page();
        }

        [BindProperty]
        public Order Order { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
