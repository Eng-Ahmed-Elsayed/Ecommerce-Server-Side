using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> Update(Category category);
    }
}
