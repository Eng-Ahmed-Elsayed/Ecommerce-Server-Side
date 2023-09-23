using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        private ApplicationDbContext _db;
        public ColorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(Color color)
        {
            if (color == null) { return await Task.FromResult(false); }
            _db.Colors.Update(color);
            return await Task.FromResult(true);

        }
    }
}
