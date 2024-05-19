using Shopping_Cart_2.Models;

namespace Shopping_Cart_2.Services
{
    public interface ICartService
    {
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> AddItem(int bookId, int qty);
    }
}
