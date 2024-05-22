using Shopping_Cart_2.Models;

namespace Shopping_Cart_2.Services
{
    public interface IUserOrderService
    {
        
        Task<IEnumerable<Order>> UserOrders();
    }
}