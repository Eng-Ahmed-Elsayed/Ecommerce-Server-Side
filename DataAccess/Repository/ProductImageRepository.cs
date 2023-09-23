using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(ProductImage productImage)
        {
            if (productImage == null) { return await Task.FromResult(false); }
            _db.ProductImages.Update(productImage);
            return await Task.FromResult(true);

        }
    }
}
