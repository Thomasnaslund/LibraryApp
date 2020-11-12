using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consid.Models;
using Consid.ViewModels;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consid.Services
{
    public interface ILibraryItemService
    {
        /// <summary>
        /// Asynchronous, only use if HttpContext is not used. 
        /// </summary>
        /// <param name="id"id of library item</param>
        /// <returns>Returns Dto with information of library items</returns>
        public Task<LibraryItemViewModel> GetLibraryItemAsync(int id);

        /// <summary>
        /// </summary>
        /// <param name="id">id of library item</param>
        /// <returns>Returns Dto with information of library items</returns>
        public LibraryItemViewModel GetLibraryItem(int id);

        /// <summary>
        /// Deletes library item if it exist.
        /// </summary>
        /// <param name="id">id of library item</param>
        /// <returns>A Boolean depending on if library item exists in db</returns>
        public Task<bool> TryDeleteAsync(int id);

        /// <summary>
        /// Asynchronous, only use if HttpContext is not used. 
        /// </summary>
        /// <param name="id">id of library item</param>
        /// <returns></returns>
        public bool TryDelete(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IEnumerable Dto with all library items </returns>
        public IEnumerable<LibraryItemViewModel> GetLibraryItems();

        /// <summary>
        /// Sets sorting value in httpcontext to eighter type, or manager.
        /// </summary>
        /// <param name="sortOrder">Sort order (currently only "type" or "manager" possible)</param>
        public void SetSortOrder(string sortOrder);

        /// <summary>
        /// Asynchronous, adds library item to db, only use if HttpContext is not used. 
        /// </summary>
        /// <param name="dto">Dto of library item</param>
        /// <returns></returns>
        public Task AddAsync(LibraryItemViewModel dto);

        /// <summary>
        /// Adds library item to db, no checks atm
        /// </summary>
        /// <param name="dto">Dto of library item</param>
        /// <returns></returns>
        public void Add(LibraryItemViewModel dto);

        /// <summary>
        /// Asynchronous, deletes library item if not null, only use if HttpContext is not used. 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<bool> TryUpdateAsync(LibraryItemViewModel dto);

        /// <summary>
        /// Deletes library item if it exists in db
        /// </summary>
        /// <param name="dto">Dto of library item</param>
        /// <returns>Boolean depending on if library item exists</returns>
        public bool TryUpdate(LibraryItemViewModel dto);

        /// <summary>
        /// </summary>
        /// <returns>SelectList with available categories</returns>
        public SelectList GetCategories();

        /// <summary>
        /// </summary>
        /// <returns>SelectList with available types</returns>
        public SelectList GetTypes();
    }
}

