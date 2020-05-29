using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BladeRazorMvcExamples.Data;
using BladeRazorMvcExamples.Models;
using BladeRazorMvcExamples.ViewModels;

namespace BladeRazorMvcExamples.Controllers
{
   
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHtmlHelper _htmlHelper;

        public CustomersController(ApplicationDbContext context, IHtmlHelper htmlHelper)
        {
            _context = context;
            _htmlHelper = htmlHelper;
        }
        
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.ToArrayAsync();                            
            return View("BladeIndex", new BladeViewModel(new Customer()) { DynamicList = customers });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View("BladeDetails", new BladeViewModel(customer));
        }

        public IActionResult Create()
        {
            var vm = new BladeViewModel(new Customer());
            // set the items for the select list in the view model
            vm.SelectItems = _htmlHelper.GetEnumSelectList<CustomerType>();
            return View("BladeCreate", vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            var vm = new BladeViewModel(new Customer());
            // set the items for the select list in the view model
            vm.SelectItems = _htmlHelper.GetEnumSelectList<CustomerType>();
            return View("BladeEdit", vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View("BladeDelete", new BladeViewModel(customer));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, FirstName,LastName,Telephone,Email,Address,CustomerType,CustomerNumber,Active,Registered")] Customer dynamicModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dynamicModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return View("BladeCreate", new BladeViewModel(dynamicModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Telephone,Email,Address,CustomerType,CustomerNumber,Active,Registered")] Customer dynamicModel)
        {
            if (id != dynamicModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dynamicModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(dynamicModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View("BladeEdit", new BladeViewModel(dynamicModel));
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
