using System;
using System.ComponentModel;

namespace Consid.ViewModels
{
    public class CategoryViewModel
    {

        public int Id { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        public bool IsReferenced { get; set; }

    }
}
