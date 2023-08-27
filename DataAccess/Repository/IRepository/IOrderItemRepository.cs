using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<bool> Update(OrderItem orderItem);
    }
}
