using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        private ApplicationDbContext _db;
        public InventoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(Inventory inventory)
        {
            if (inventory == null) { return await Task.FromResult(false); }
            _db.Inventories.Update(inventory);
            return await Task.FromResult(true);

        }
    }
}
