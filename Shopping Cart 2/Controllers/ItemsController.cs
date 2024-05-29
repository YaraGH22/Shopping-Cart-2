using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart_2.Services;
using Shopping_Cart_2.ViewModels;

namespace Shopping_Cart_2.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IItemService _itemService;

        public ItemsController(ICategoryService categoryService, IItemService itemService)
        {
            _categoryService = categoryService;
            _itemService = itemService;
        }
        public IActionResult Index()
        {
            var items = _itemService.GetItemsByUserId();
            return View(items);
        }
        public IActionResult Details(int id)
        {
            var item =_itemService.GetById(id);
            if (item is null) return NotFound();
            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateItemVM vm = new()
            {
                Categories = _categoryService.GetSelectList(),
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateItemVM model , Stock stock)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _itemService.Create(model,stock);
            return RedirectToAction ("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // 1- get item by id //اساسي
            var item = _itemService.GetById(id);
            if (item is null) return NotFound();
            // 3- create vm object
            // تعريف غرض وسيط و اسناد الاساسي للوسيط
            EditItemVM model = new()
            {
                Id = id,
                Name = item.Name,
                Description = item.Description,
                CategoryId = item.CategoryId,
                Price = item.Price,
                Categories = _categoryService.GetSelectList(),
                CurrentCover = item.Cover,
                 

            };
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditItemVM model)
        {
            // 5- تطبيق الخدمة على البارمتر الغرض الوسيط
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var item =await _itemService.Update(model);
            if (item is null) return BadRequest();
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _itemService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}
