using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        Task<bool> Update(OrderDetails orderDetails);

    }
}
