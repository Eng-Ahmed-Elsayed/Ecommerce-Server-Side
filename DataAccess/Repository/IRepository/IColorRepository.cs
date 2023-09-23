using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IColorRepository : IRepository<Color>
    {
        Task<bool> UpdateAsync(Color color);
    }
}
