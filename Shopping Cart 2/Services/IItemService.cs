using Shopping_Cart_2.Models;
using Shopping_Cart_2.ViewModels;

namespace Shopping_Cart_2.Services
{
    public interface IItemService
    {
        IEnumerable<Item> GetAll();
        Item? GetById(int id);
        Task Create (CreateItemVM vmItem);
        Task<Item?> Update (EditItemVM vmItem);
        
    }
}
