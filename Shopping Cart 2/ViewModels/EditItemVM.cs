using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping_Cart_2.Attributes;
using Shopping_Cart_2.Sittings;
using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart_2.ViewModels
{
    public class EditItemVM : BaseItemVM
    {
        public int Id { get; set; }

        public string? CurrentCover { get; set; }

        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile? Cover { get; set; } = default!;

        

    }
}
