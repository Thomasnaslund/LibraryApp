using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Consid.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consid.ViewModels
{
    public class LibraryItemViewModel
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public string Title { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        public string Acronym { get; set; }

        public string Author { get; set; }

        public int? Pages { get; set; }

        [DisplayName("Runtime (Min)")]
        public int? RuntimeMinutes { get; set; }

        [DisplayName("Is Borrowable?")]
        public bool IsBorrowable { get; set; }

        public bool IsCheckedOut { get; set; }

        public string Borrower { get; set; }

        [DisplayName("Borrowed at")]
        public DateTime? BorrowDate { get; set; }

        public string Type { get; set; }
    }
}
