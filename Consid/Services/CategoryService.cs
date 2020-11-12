using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Consid.Models;
using Consid.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Consid.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> TryAddAsync(CategoryViewModel dto)
        {
            if (await _context.Categories.AnyAsync(c => c.CategoryName == dto.CategoryName))
            {
                return false;
            }
            var category = _mapper.Map<Category>(dto);
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TryDeleteAsync(int id)
        {
            if (await IsReferencedAsync(id))
            {
                return false;
            }

            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {

            var categoryQuery = from s in _context.Categories
                                select s;
            categoryQuery = categoryQuery.OrderByDescending(s => s.CategoryName);

            List<Category> categories = await categoryQuery.AsNoTracking().ToListAsync();
            List<CategoryViewModel> categoriesDto = _mapper.Map<List<Category>, List<CategoryViewModel>>(categories);

            return categoriesDto;

        }


        public async Task<CategoryViewModel> GetCategoryAsync(int id)
        {
            var categoryDto = _mapper.Map<CategoryViewModel>(await _context.Categories.FindAsync(id));
            categoryDto.IsReferenced = await IsReferencedAsync(id);
     
            return categoryDto;
        }

        public async Task<bool> IsReferencedAsync(int id)
        {
            return await _context.LibraryItems.AnyAsync(e => e.CategoryId == id);
        }

        public async Task<bool> TryUpdateAsync(CategoryViewModel dto)
        {
            if (await _context.Categories.AnyAsync(c => c.CategoryName == dto.CategoryName))
            {
                return false;
            }

            var category = _mapper.Map<Category>(dto);
  
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
                {
                    { return false; }
                }
                else
                {
                    { throw; }
                }
            }
            return true;
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}