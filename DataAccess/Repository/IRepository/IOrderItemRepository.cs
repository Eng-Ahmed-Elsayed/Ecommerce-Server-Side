using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<bool> UpdateAsync(OrderItem orderItem);
    }
}
