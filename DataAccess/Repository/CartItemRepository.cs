using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private ApplicationDbContext _db;
        public CartItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        //public async Task<bool> UpdateAsync(CartItem cartItem)
        //{
        //    if (cartItem == null) { return await Task.FromResult(false); }
        //    cartItem.UpdatedAt = DateTime.Now;
        //    _db.CartItems.Attach(cartItem);
        //    _db.Entry(cartItem).Property(x => x.Quantity).IsModified = true;
        //    _db.Entry(cartItem).Property(x => x.UpdatedAt).IsModified = true;
        //    return await Task.FromResult(true);

        //}
        public async Task<bool> UpdateAsync(CartItem cartItem)
        {
            if (cartItem == null) { return await Task.FromResult(false); }
            _db.CartItems.Update(cartItem);
            return await Task.FromResult(true);

        }
    }
}
