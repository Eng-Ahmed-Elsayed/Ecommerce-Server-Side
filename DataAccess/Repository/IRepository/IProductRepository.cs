using Models.DataQuerying;
using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> UpdateAsync(Product product);
        Task<PagedList<Product>> GetPagedListAsync(ProductParameters? productParameters = null,
            string? includeProperties = null);
    }
}
