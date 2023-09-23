using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        Task<bool> UpdateAsync(ProductImage productImage);
    }
}
