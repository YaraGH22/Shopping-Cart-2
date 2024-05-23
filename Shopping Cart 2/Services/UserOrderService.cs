using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart_2.Data;
using Shopping_Cart_2.Models;

namespace Shopping_Cart_2.Services
{
    public class UserOrderService : IUserOrderService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User; //currently authenticated user
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        // get all orders for one user 
        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders =await _db.Orders
                           .Include(x => x.OrderStatus)
                           .Include(x => x.OrderDetail)
                           .ThenInclude(x => x.Item)
                           .ThenInclude(x => x.Category)
                           .Where(a => a.UserId == userId)
                           .ToListAsync();

            return orders;
        }
        public async Task<Order> GetOrderDetail(int orderId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var order = await _db.Orders.Include(x => x.OrderStatus)
                           .Include(x => x.OrderDetail)
                           .ThenInclude(x => x.Item)
                           .ThenInclude(x => x.Category)
                           .Where(a => a.UserId == userId)
                           .SingleOrDefaultAsync(x => x.Id == orderId);
            return order;
        }
    }
}
