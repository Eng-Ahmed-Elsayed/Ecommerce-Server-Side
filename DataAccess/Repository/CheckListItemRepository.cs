using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class CheckListItemRepository : Repository<CheckListItem>, ICheckListItemRepository
    {
        private ApplicationDbContext _db;
        public CheckListItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(CheckListItem checkListItem)
        {
            if (checkListItem == null) { return await Task.FromResult(false); }
            _db.CheckListItems.Update(checkListItem);
            return await Task.FromResult(true);

        }
    }


}
