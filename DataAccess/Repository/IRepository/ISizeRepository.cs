using Models.Models;

namespace DataAccess.Repository.IRepository
{

    public interface ISizeRepository : IRepository<Size>
    {
        Task<bool> UpdateAsync(Size size);
    }
}
