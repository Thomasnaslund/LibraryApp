using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Consid.Helper;
using Consid.Services;
using Consid.ViewModels;
using System.Text;
using System.Collections.Generic;

namespace Consid
{
    public class LibraryItemController : Controller
    {

        private readonly ILibraryItemService _liService;

        public LibraryItemController(ILibraryItemService LIService)
        {
            _liService = LIService;
        }
        
        // GET: LibraryItem
        public IActionResult Index(string sortOrder)
        {

            _liService.SetSortOrder(sortOrder);
            var libraryItemVm = _liService.GetLibraryItems();
            return View(libraryItemVm);
            
        }

        
        // GET: LibraryItem/Create
        [NoDirectAccess]
        public IActionResult Create()
        {
            var libraryItem = new LibraryItemViewModel();
                libraryItem.Categories = _liService.GetCategories();
                libraryItem.Types = _liService.GetTypes();

            return View(libraryItem);
        }

        // POST: LibraryItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CategoryId,IsCheckedOut,Title,Author,Pages,RuntimeMinutes,IsBorrowable,Borrower,BorrowDate,Type")] LibraryItemViewModel libraryItem)
        {
            if (ModelState.IsValid)
            {
                  _liService.Add(libraryItem);

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllItems",  _liService.GetLibraryItems()) });
            }
            return Json(new { isValid = false,  errormsg="Invalid input!" });
        }

        [NoDirectAccess]
        public async Task<IActionResult> CheckOut(int id)
        {

              var libraryItem = await _liService.GetLibraryItemAsync(id);

              return View(libraryItem);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckInOut(int id, string borrower)
        {
            var libraryItem = _liService.GetLibraryItem(id);

            if (libraryItem.IsCheckedOut) 
            {
                libraryItem.IsCheckedOut = false;
            } else
            {
                libraryItem.Borrower = borrower;
                libraryItem.BorrowDate = DateTime.Now;
                libraryItem.IsCheckedOut = true;
            }

            if (_liService.TryUpdate(libraryItem))
            {
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllItems", _liService.GetLibraryItems()) });
            }   
                return Json(new { isValid = false, errormsg = "Failed to check in!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refresh([Bind("Id,FirstName,LastName,IsCEO,IsManager,Rank,ManagerId")] EmployeeViewModel employee)
        {

            //Make sure that employee is not both CEO and Manager

            return Json(new { html = Helper.RenderRazorViewToString(this, "Create", employee) });
        }

        [HttpPost]
        public FileResult ExportCSV()
        {
            var builder = new StringBuilder();

            var lbItems = (List<LibraryItemViewModel>)_liService.GetLibraryItems();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lbItems.Count; i++)
            {               
                sb.Append(lbItems[i].Id + ';');
                sb.Append(lbItems[i].Title + ';');
                sb.Append(lbItems[i].CategoryName + ';');
                sb.Append(lbItems[i].Author + ';');
                sb.Append(lbItems[i].Borrower + ';');
                sb.AppendLine();
                //sb.Append("\r\n");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Grid.csv");
        }

        // GET: LibraryItem/Edit/5
        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {

            var libraryItem = await _liService.GetLibraryItemAsync(id);
            libraryItem.Categories = _liService.GetCategories();
            if (libraryItem == null)
            {
                return Json(new { isValid = true, errormsg = "Could not find item" });
            }

            return View(libraryItem);

        }

        // POST: LibraryItem/Edit/5
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,CategoryId,Title,Author,Pages,RuntimeMinutes,IsBorrowable,Type,Borrower,BorrowDate,IsCheckedOut")] LibraryItemViewModel libraryItem)
        {

            if (ModelState.IsValid)
            {
                if (_liService.TryUpdate(libraryItem))
                {
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllItems", _liService.GetLibraryItems()) });
                } else
                {
                    return Json(new { isValid = true, errormsg = "Could not update item" });
                }

            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", libraryItem) });
        }

        // GET: LibraryItem/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var libraryItem = await _liService.GetLibraryItemAsync(id);

            if (libraryItem == null)
            {
                return NotFound();
            }

            return View(libraryItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

            if (_liService.TryDelete(id))
            {
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllItems", _liService.GetLibraryItems()) });
            } else
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "_ViewAllItems", _liService.GetLibraryItems()), errormsg = "Error: item does not exist in db" });
            }

        }
    }
}
