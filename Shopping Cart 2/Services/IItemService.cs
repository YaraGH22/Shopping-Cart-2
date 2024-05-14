using Shopping_Cart_2.ViewModels;

namespace Shopping_Cart_2.Services
{
    public interface IItemService
    {
        Task Create (CreateItemVM vmItem);
    }
}
