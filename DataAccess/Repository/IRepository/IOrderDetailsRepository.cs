using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        Task<bool> UpdateAsync(OrderDetails orderDetails);

    }
}
