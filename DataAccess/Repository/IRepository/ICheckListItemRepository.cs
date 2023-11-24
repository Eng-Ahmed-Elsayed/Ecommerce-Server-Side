using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ICheckListItemRepository : IRepository<CheckListItem>
    {
        Task<bool> UpdateAsync(CheckListItem favoriteItem);
    }
}
