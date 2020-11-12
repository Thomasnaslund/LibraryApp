using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consid.Models;
using Consid.ViewModels;

namespace Consid.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// retrieves all categories
        /// </summary>
        /// <param name="id"> Takes in id of entity</param>
        /// <returns> A category ViewModel</returns>
        public Task<CategoryViewModel> GetCategoryAsync(int id);

        /// <summary>
        /// retrieves all categories
        /// </summary>
        /// <returns> A IEnumerabl containing all categories</returns>
        public Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();

        /// <summary>
        /// Deletes category if category name is not referenced.
        /// </summary>
        /// <param name="id"> Takes in id entity</param>
        /// <returns> A Boolean depending on if category is referenced or not</returns>
        public Task<bool> TryDeleteAsync(int id);

        /// <summary>
        /// Adds category to database if category name is unique and no ConcurrencyExeption
        /// </summary>
        /// <param name="dto"> Takes in ViewModel</param>
        /// <returns> A Boolean depending on if category already exists or not</returns>
        public Task<bool> TryAddAsync(CategoryViewModel dto);

        /// <summary>
        /// Updates the category if category name is unique and no ConcurrencyExeption
        /// </summary>
        /// <param name="dto"> Takes in ViewModel</param>
        /// <returns> A Boolean depending on if category already exists or not</returns>
        public Task<bool> TryUpdateAsync(CategoryViewModel dto);

        /// <summary>
        /// retrieves all categories
        /// </summary>
        /// <param name="id"> Takes in id entity</param>
        /// <returns> A Boolean depending on if entity is referenced or not</returns>
        public Task<bool> IsReferencedAsync(int id);
    }
}
