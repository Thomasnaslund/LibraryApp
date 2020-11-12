using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consid.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<LibraryItem> LibraryItems { get; set; }
    }
}
