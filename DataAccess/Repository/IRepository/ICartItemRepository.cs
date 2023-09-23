using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<bool> UpdateAsync(CartItem cartItem);
    }
}
