using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Task<bool> UpdateAsync(Inventory inventory);
    }
}
