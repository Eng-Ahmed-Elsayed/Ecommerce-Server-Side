using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class ShoppingSessionRepository : Repository<ShoppingSession>, IShoppingSessionRepository
    {
        private ApplicationDbContext _db;
        public ShoppingSessionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> Update(ShoppingSession shoppingSession)
        {
            if (shoppingSession == null) { return await Task.FromResult(false); }
            _db.ShoppingSessions.Update(shoppingSession);
            return await Task.FromResult(true);

        }
    }
}
