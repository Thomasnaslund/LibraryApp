using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Consid.Services;
using Consid.ViewModels;
using static Consid.Helper;
using Microsoft.AspNetCore.Html;

namespace Consid
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _cService;

        public CategoryController(ICategoryService cService)
        {
            _cService = cService;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _cService.GetCategoriesAsync());
        }

        // GET: Category/Create
        [NoDirectAccess]
        public IActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName")] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                if (await _cService.TryAddAsync(category))
                {
                     
                    return Json(new { isValid = true, html = RenderRazorViewToString(this, "_ViewAllCategories", await _cService.GetCategoriesAsync()) });
                }
                return Json(new { isValid = false, errormsg = "Name already exists" });
            }
            return Json(new { isValid = false, errormsg = "Invalid input" });
        }

        // GET: Category/Edit/5
        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _cService.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,CategoryName")] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                if (await _cService.TryUpdateAsync(category))
                {
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCategories", await _cService.GetCategoriesAsync()) });
                }
                else
                {
                    return Json(new { isValid = false, errormsg = "Name already exists!" }); ;
                }

            }
            return Json(new { isValid = false, errormsg = "Incorrect input!" });
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (await _cService.TryDeleteAsync(id))
            {
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllCategories", await _cService.GetCategoriesAsync()) });
            }
            else
            {
                return Json(new { isValid = false, errormsg = "Category is referenced by one or more library items!" });
            }
        }


    }
}
