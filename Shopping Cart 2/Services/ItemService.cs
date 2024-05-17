using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<Item> GetAll()
        {
            var Item = _context.items.Include(x => x.Category)
                                     .AsNoTracking()
                                     .ToList();
            return Item;
        }

        public Item? GetById(int id)
        {
            var Item = _context.items.Include(x => x.Category)
                                     //.Include(x => x.Orders)
                                     .AsNoTracking()
                                     .SingleOrDefault(g => g.Id == id);
            return Item;
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

        public async Task<Item?> Update(EditItemVM vmItem)
        {
            // 4-  خدمة تعديل تجلب غرض اساسي من داتاسيت 
            // ثم تسند (البارمتر) الغرض الوسيط الى الاساسي 
            var item = await _context.items.Include(g => g.Category)
                                           .SingleOrDefaultAsync(g => g.Id == vmItem.Id);
            if (item == null) return null;

            item.Name = vmItem.Name;
            item.Description = vmItem.Description;
            item.Price = vmItem.Price;
            item.CategoryId = vmItem.CategoryId;

            //this for new cover
            var hasNewCover = vmItem.Cover is not null;
            var oldCover = item.Cover; // to delete the oldcover if there is new one

            if (hasNewCover)
            {
                item.Cover = await SaveCover(vmItem.Cover!);
            }
            //for delete old cover
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }
                return item;
            }
            else
            { // if update fail after choice new cover then delete new cover 
                var cover = Path.Combine(_imagesPath, item.Cover);
                File.Delete(cover);
                return null;
            }
        }

        public bool Delete(int id)
        {

            var isDeleted = false;

            var item = _context.items.Find(id);

            if (item is null)
                return isDeleted;

            _context.Remove(item);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;

                var cover = Path.Combine(_imagesPath, item.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }
    }
}
