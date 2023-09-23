using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private ApplicationDbContext _db;
        public DiscountRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(Discount discount)
        {
            if (discount == null) { return await Task.FromResult(false); }
            _db.Discounts.Update(discount);
            return await Task.FromResult(true);

        }
    }
}
