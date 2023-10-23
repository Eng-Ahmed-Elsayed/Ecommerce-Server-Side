using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{

    public class ShippingOptionRepository : Repository<ShippingOption>, IShippingOptionRepository
    {
        private ApplicationDbContext _db;
        public ShippingOptionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(ShippingOption shippingOption)
        {
            if (shippingOption == null) { return await Task.FromResult(false); }
            _db.ShippingOptions.Update(shippingOption);
            return await Task.FromResult(true);

        }
    }
}
