﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart_2.Data;
using Shopping_Cart_2.Models;
using System.Net;

namespace Shopping_Cart_2.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        

        //retrieves the user ID associated with the currently authenticated user
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User; //currently authenticated user
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
         
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
        public async Task<int> AddItem(int itmId, int qty)
        {
            //userId => ShCart => cartDItem

            // begin Transaction
            using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                // الحصول على 1 id المستخدم  
                
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("user is not logged-in");
                }
                   
                //2- جلب السلة الخاصة بالمستخدم
                var ShCart = await GetCart(userId);
                if (ShCart is null)
                {
                    ShCart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    await _db.ShoppingCarts.AddAsync(ShCart);
                }
                _db.SaveChanges();

                //-3-  جبلي تفااااااصيل السلة يلي
                //الايدي تبعها يساوي ايدي سلة المستخدم الحالي
                //و يلي ايدي عنصرها يساوي الايدي الممرر بالضغط على الزر
                // cart detail section
                var cartDItem =await _db.CartDetails
                                  .FirstOrDefaultAsync(a => a.ShoppingCartId == ShCart.Id && a.ItemId == itmId);
                if (cartDItem is not null)
                {
                    cartDItem.Quantity += qty;
                }
                else
                {
                    var item =await _db.items.FindAsync(itmId);
                    
                    cartDItem = new CartDetail
                    {
                        ItemId = itmId,
                        ShoppingCartId = ShCart.Id,
                        Quantity = qty,
                        UnitPrice = item.Price  
                    };
                    await _db.CartDetails.AddAsync(cartDItem);
                }
                await _db.SaveChangesAsync();
                transaction.Commit();
                
            }
            catch (Exception ex) { ; }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;

        }
        public async Task<int> RemoveItem(int itmId)
        {
            
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var ShCart = await GetCart(userId);
                if (ShCart is null)
                    throw new InvalidOperationException("Invalid cart");
                // cart detail section
                var cartDItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == ShCart.Id && a.ItemId == itmId);
                if (cartDItem is null)
                    throw new InvalidOperationException("Not items in cart");
                else if (cartDItem.Quantity == 1)
                    _db.CartDetails.Remove(cartDItem);
                else
                    cartDItem.Quantity = cartDItem.Quantity - 1;
                _db.SaveChanges();
                 
            }
            catch (Exception ex) {   }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<ShoppingCart> GetUserCart()
        { // للحصول على كل عناصر الكارت الواحد مع ارتباطاتها في الجداول المرتبطة بها
            var userId = GetUserId();
            if (userId == null)
                throw new InvalidOperationException("Invalid userid");
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Item)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }
        public async Task<int> GetCartItemCount(string userId = "")
        { // لمعرفة عدد العناصر في كل كارت
            if (string.IsNullOrEmpty(userId)) // updated line
            {
                userId = GetUserId();
            }
            //var data = _db.ShoppingCarts.Include(a => a.CartDetails)
            //                            .Where(a => a.UserId == userId)
            //                            .ToListAsync();
            var data = await (from cart in _db.ShoppingCarts
                              join cartDetail in _db.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == userId // updated line
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }
    }
}
