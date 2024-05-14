using Microsoft.AspNetCore.Mvc;
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
            return View();
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
        public async Task<IActionResult> Create(CreateItemVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _itemService.Create(model);
            return RedirectToAction ("Index");
        }
    }
}
