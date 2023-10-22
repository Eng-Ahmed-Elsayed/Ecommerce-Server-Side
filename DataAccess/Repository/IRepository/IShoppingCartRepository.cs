using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task<bool> UpdateAsync(ShoppingCart shoppingCart);
    }
}
