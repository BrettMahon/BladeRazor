// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHtmlHelper _htmlHelper;

        public OrdersController(ApplicationDbContext context, IHtmlHelper htmlHelper)
        {
            _context = context;
            _htmlHelper = htmlHelper;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.Customer).ToArrayAsync();
            return View("BladeIndex", new BladeViewModel(new Order()) { DynamicList = orders });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View("BladeDetails", new BladeViewModel(order));
        }

        public IActionResult Create()
        {
            var vm = new BladeViewModel(new Order());
            // setup the items dictionary for the select lists
            // we use the dictionary now because we have two select lists - otherwiwe use SelectItems instead            
            var customers = new SelectList(_context.Customers, "Id", "FullName");
            var orderStatus = _htmlHelper.GetEnumSelectList<OrderStatus>();
            // the keys for the dictionary are specified via the data annotations of the class
            vm.SelectItemsDictionary = new Dictionary<string, IEnumerable<SelectListItem>>()
                { {"Customers", customers }, {"OrderStatus", orderStatus} };
            // return the view
            return View("BladeCreate", vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var vm = new BladeViewModel(order);
            // setup the items dictionary for the select lists
            // we use the dictionary now because we have two select lists - otherwiwe use SelectItems instead            
            var customers = new SelectList(_context.Customers, "Id", "FullName");
            var orderStatus = _htmlHelper.GetEnumSelectList<OrderStatus>();
            // the keys for the dictionary are specified via the data annotations of the class
            vm.SelectItemsDictionary = new Dictionary<string, IEnumerable<SelectListItem>>()
                { {"Customers", customers }, {"OrderStatus", orderStatus} };
            // return the view
            return View("BladeEdit", vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View("BladeDelete", new BladeViewModel(order));
        }


        /// <summary>
        /// We need to name the parameter as it is in the view model - dynamicModel
        /// </summary>      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,ProductCode,Date,Notes,OrderStatus,CustomerId")] Order dynamicModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dynamicModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create));
        }

        /// <summary>
        /// We need to name the parameter as it is in the view model - dynamicModel
        /// </summary>       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,ProductCode,Date,Notes,OrderStatus,CustomerId")] Order dynamicModel)
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
                    if (!OrderExists(dynamicModel.Id))
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
