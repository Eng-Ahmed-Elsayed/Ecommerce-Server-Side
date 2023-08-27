using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<bool> Update(CartItem cartItem);
    }
}
