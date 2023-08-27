using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> Update(Product product)
        {
            if (product == null) { return await Task.FromResult(false); }
            _db.Products.Update(product);
            return await Task.FromResult(true);

        }
    }
}
