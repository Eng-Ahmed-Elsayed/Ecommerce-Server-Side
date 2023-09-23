using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(Category category)
        {
            if (category == null) { return await Task.FromResult(false); }
            _db.Categories.Update(category);
            return await Task.FromResult(true);

        }
    }
}
