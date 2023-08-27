using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private ApplicationDbContext _db;
        public OrderItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> Update(OrderItem orderItem)
        {
            if (orderItem == null) { return await Task.FromResult(false); }
            _db.OrderItems.Update(orderItem);
            return await Task.FromResult(true);

        }
    }
}
