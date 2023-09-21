using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        Task<bool> Update(ProductImage productImage);
    }
}
