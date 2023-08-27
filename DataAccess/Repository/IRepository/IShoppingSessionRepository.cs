using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IShoppingSessionRepository : IRepository<ShoppingSession>
    {
        Task<bool> Update(ShoppingSession shoppingSession);
    }
}
