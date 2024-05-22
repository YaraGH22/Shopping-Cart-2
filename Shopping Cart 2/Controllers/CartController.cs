using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping_Cart_2.Models;
using Shopping_Cart_2.Services;
using System.Net;

namespace Shopping_Cart_2.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IActionResult> AddItem(int itemId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartService.AddItem(itemId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var cartCount = await _cartService.RemoveItem(itemId); 
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartService.GetUserCart();
            return View(cart);
        }
        // for script in _layout.cshtml // to get total number of items in this cart // then pass it to other place in html
        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartService.GetCartItemCount();
            return Ok(cartItem);
        }
    }
}
