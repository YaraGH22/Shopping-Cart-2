using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart_2.ViewModels
{
    public class BaseItemVM
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
       
        [Display(Name = "Category")]
        // to fill drop down list in view (category)
        public int CategoryId { get; set; } = 0; // asp-for
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>(); // asp-item


    }
}
