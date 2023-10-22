using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null) { return await Task.FromResult(false); }
            _db.ShoppingCarts.Update(shoppingCart);
            return await Task.FromResult(true);

        }
    }
}
