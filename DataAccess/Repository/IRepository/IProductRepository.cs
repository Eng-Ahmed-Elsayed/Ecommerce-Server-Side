using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> UpdateAsync(Product product);
    }
}
