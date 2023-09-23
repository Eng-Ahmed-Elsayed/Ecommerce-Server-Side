using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(OrderDetails orderDetails)
        {
            if (orderDetails == null) { return await Task.FromResult(false); }
            _db.OrdersDetails.Update(orderDetails);
            return await Task.FromResult(true);

        }
    }
}
