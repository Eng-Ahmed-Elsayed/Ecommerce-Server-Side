using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        private ApplicationDbContext _db;
        public SizeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(Size size)
        {
            if (size == null) { return await Task.FromResult(false); }
            _db.Sizes.Update(size);
            return await Task.FromResult(true);

        }
    }
}
