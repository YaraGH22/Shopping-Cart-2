using Humanizer;
using Shopping_Cart_2.Data;
using Shopping_Cart_2.Models;
using Shopping_Cart_2.ViewModels;

namespace Shopping_Cart_2.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public ItemService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}/assets/images/items";
            
        }
        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return coverName;
        }


        public async Task Create(CreateItemVM vmItem)
        {
            var coverName = await SaveCover(vmItem.Cover);
            
            Item item = new()
            {
                Name = vmItem.Name,
                Description = vmItem.Description,
                Price = vmItem.Price,
                Cover = coverName,
                CategoryId = vmItem.CategoryId,
                
                 
            };
            await _context.items.AddAsync(item);
            await _context.SaveChangesAsync();
        }
    }
}
