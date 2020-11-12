using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consid.Models;
using Consid.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consid.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// </summary>
        /// <returns>Dto with employee information</returns>
        public Task<EmployeeViewModel> GetEmployeeAsync(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>IEnumerablo Dto:s with all employees</returns>
        public Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync();

        /// <summary>
        /// Takes in employee dto and checks whichs managers are available to
        /// object depending on busiess logic
        /// </summary>
        /// <param name="dto">Dto of employee</param>
        /// <returns>A SelectList containing dto:s of available managers</returns>
        public Task<SelectList> GetAvailableManagersAsync(EmployeeViewModel dto);

        /// <summary>
        ///  
        /// </summary>
        /// <returns>EInumerable Dto containing all managers</returns>
        public Task<IEnumerable<EmployeeViewModel>> GetManagersAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>EInumerable Dto containing the CEOs</returns>
        public Task<IEnumerable<EmployeeViewModel>> GetCEOAsync();

        /// <summary>
        /// Deletes employee if not referenced as manager by other employees
        /// </summary>
        /// <param name="id">id of employee</param>
        /// <returns>A Boolean depening on if delete was sucessfull or not</returns>
        public Task<bool> TryDeleteAsync(int id);

        /// <summary>
        /// Adds employee to db depending on business rules
        /// </summary>
        /// <param name="dto">Dto of employee</param>
        /// <returns>null if successfull otherwise returns error message as string</returns>
        public Task<string> TryAddAsync(EmployeeViewModel dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto">Dto of employee</param>
        /// <returns>null if successfull otherwise returns error message as string</returns>
        public Task<string> TryUpdateAsync(EmployeeViewModel dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of employee</param>
        /// <returns>Return a Boolean depending on if employee is currently managing other employees</returns>
        public Task<bool> IsManagingAsync(int id);

    }
}
