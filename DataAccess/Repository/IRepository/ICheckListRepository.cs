using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ICheckListRepository : IRepository<CheckList>
    {
        Task<bool> UpdateAsync(CheckList favorite);
    }
}
