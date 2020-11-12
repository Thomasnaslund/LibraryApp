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
using Consid.Controllers;

namespace Consid
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _eService;

        public EmployeeController(IEmployeeService cService)
        {
            _eService = cService;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _eService.GetAllEmployeesAsync());
        }

        // GET: Employee/Create
        [NoDirectAccess]
        public async Task<IActionResult> Create(string select)
        {

            var employee = new EmployeeViewModel();
            employee.Managers = await _eService.GetAvailableManagersAsync(employee);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refresh([Bind("Id,FirstName,LastName,IsCEO,IsManager,Rank,ManagerId")] EmployeeViewModel employee)
        {

            //Make sure that employee is not both CEO and Manager
            if (employee.IsCEO)
            {
                employee.IsManager = false;
            }
            if (employee.IsManager)
            {
                employee.IsCEO = false;
            }

            employee.Managers = await _eService.GetAvailableManagersAsync(employee);
            return Json(new { html = Helper.RenderRazorViewToString(this, "Create", employee) });
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,IsCEO,IsManager,Rank,ManagerId")] EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                string errormessage = await _eService.TryAddAsync(employee);
                if (errormessage == null)
                {
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllEmployees", await _eService.GetAllEmployeesAsync()) });
                }
                else
                {
                    
                    return Json(new { isValid = false, errormsg = errormessage });
                }
            }
            return Json(new { isValid = false, errormsg = "Fill all the fields!" });

        }

        // GET: Employee/Edit/5
        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _eService.GetEmployeeAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,FirstName,LastName,IsCEO,IsManager,Rank,ManagerId")] EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                string errormessage = await _eService.TryUpdateAsync(employee);
                if (errormessage == null)
                {
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllEmployees", await _eService.GetAllEmployeesAsync()) });
                }
                else
                {
                    return Json(new { isValid = false, errormsg = errormessage });
                }

            }
            return Json(new { isValid = false, errormsg = "Error: This user manages one or more employees" });
        }


        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _eService.GetEmployeeAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (await _eService.TryDeleteAsync(id))
            {
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllEmployees", await _eService.GetAllEmployeesAsync()) });
            }
            else
            {
                return Json(new { isValid = false, errormsg = "Error: This user manages one or more employees" });
            }

        }


    }
}
