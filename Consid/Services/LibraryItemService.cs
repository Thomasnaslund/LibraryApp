using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Consid.Models;
using Consid.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Consid.Services
{
    public class LibraryItemService : ILibraryItemService

    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISession _session;


        public LibraryItemService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public async Task AddAsync(LibraryItemViewModel dto)
        {

            var libraryItem = _mapper.Map<LibraryItem>(dto);
            await _context.AddAsync(libraryItem);
            await _context.SaveChangesAsync();

        }


        public async Task<bool> TryDeleteAsync(int id)
        {

            var libraryItem = await _context.LibraryItems.FindAsync(id);
            if (libraryItem != null)
            {
                _context.LibraryItems.Remove(libraryItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }



        public async Task<bool> TryUpdateAsync(LibraryItemViewModel dto)
        {
            var libraryItem = _mapper.Map<LibraryItem>(dto);
            try
            {
                _context.Update(libraryItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!LibraryItemExists(libraryItem.Id))
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



        public IEnumerable<LibraryItemViewModel> GetLibraryItems()
        {
            var query = from l in _context.LibraryItems.Include(l => l.Category)
                                    select l;

            if (_session.GetString("sortOrder") == "type")
            {
                query = query.OrderByDescending(l => l.Type);
            }
            else
            {
                query = query.OrderByDescending(l => l.CategoryId);
            }

            List<LibraryItem> libraryItems = query.AsNoTracking().ToList();
            List<LibraryItemViewModel> libraryItemsDto = _mapper.Map<List<LibraryItem>, List<LibraryItemViewModel>>(libraryItems);
            AddAcronyms(libraryItemsDto);

            return libraryItemsDto;
        }

        public async Task<LibraryItemViewModel> GetLibraryItemAsync(int id)
        {
            var libraryItem = await _context.LibraryItems.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            var libraryItemDto = _mapper.Map<LibraryItemViewModel>(libraryItem);

            return libraryItemDto;
        }

        public void SetSortOrder(string sortOrder)
        {
            if (String.IsNullOrEmpty(sortOrder))
            {
                if (_session.GetString("sortOrder") == null)
                {
                    _session.SetString("sortOrder", "category"); 
                }
                
            }
            else
            {
                _session.SetString("sortOrder", sortOrder);
            }
        }



        public SelectList GetCategories()
        {

            List<SelectListItem> categories = _context.Categories.AsNoTracking()
                    .OrderBy(n => n.CategoryName)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.Id.ToString(),
                            Text = n.CategoryName
                        }).ToList();

            return new SelectList(categories, "Value", "Text", 1);

        }



        public SelectList GetTypes()
        {

            var types = new SelectList(new List<SelectListItem>
            {
                new SelectListItem() { Text = "Book", Value = "Book" },
                new SelectListItem() { Text = "Audio book", Value = "Audio book" },
                new SelectListItem() { Text = "DVD", Value = "DVD" },
                new SelectListItem() { Text = "Reference book", Value = "Reference book" },
            }, "Value", "Text", 1);

            return types;
        }



        public void Add(LibraryItemViewModel dto)
        {
            var libraryItem = _mapper.Map<LibraryItem>(dto);
            _context.Add(libraryItem);
            _context.SaveChanges();
        }



        public bool TryDelete(int id)
        {
            var libraryItem = _context.LibraryItems.AsNoTracking().FirstOrDefault(l => l.Id == id);
            if (libraryItem != null)
            {
                _context.LibraryItems.Remove(libraryItem);
                _context.SaveChanges();
                return true;
            }
            return false;
        }



        public bool TryUpdate(LibraryItemViewModel dto)
        {
            var libraryItem = _mapper.Map<LibraryItem>(dto);
            try
            {
                _context.Update(libraryItem);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!LibraryItemExists(libraryItem.Id))
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

        private bool LibraryItemExists(int id)
        {
            return _context.LibraryItems.Any(e => e.Id == id);
        }

        private void AddAcronyms(List<LibraryItemViewModel> libraryItems)
        {
            StringBuilder sb = new StringBuilder();
            char[] lista = { ' ', '.' };
            string[] newString;

            foreach (LibraryItemViewModel libraryitem in libraryItems)
            {
                if (libraryitem.Title != null)
                {
                    newString = libraryitem.Title.Split(lista);
                    sb.Append("(");
                    for (int i = 0; i < newString.Length; i++)
                    {
                        sb.Append(newString[i][0]);
                    }
                    sb.Append(")");
                    libraryitem.Acronym = sb.ToString();
                    sb.Clear();
                }
            }            
        }

        public LibraryItemViewModel GetLibraryItem(int id)
        {
            var libraryItem = _context.LibraryItems.AsNoTracking().FirstOrDefault(l => l.Id == id);
            var libraryItemDto = _mapper.Map<LibraryItemViewModel>(libraryItem);

            return libraryItemDto;
        }
    }
}
