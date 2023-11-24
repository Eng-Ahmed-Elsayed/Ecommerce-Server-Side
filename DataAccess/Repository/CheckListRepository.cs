using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{

    public class CheckListRepository : Repository<CheckList>, ICheckListRepository
    {
        private ApplicationDbContext _db;
        public CheckListRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> UpdateAsync(CheckList checkList)
        {
            if (checkList == null) { return await Task.FromResult(false); }
            _db.CheckLists.Update(checkList);
            return await Task.FromResult(true);

        }
    }
}
