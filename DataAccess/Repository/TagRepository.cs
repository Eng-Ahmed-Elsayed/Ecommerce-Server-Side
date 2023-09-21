using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{

    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private ApplicationDbContext _db;
        public TagRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> Update(Tag tag)
        {
            if (tag == null) { return await Task.FromResult(false); }
            _db.Tags.Update(tag);
            return await Task.FromResult(true);

        }
    }
}
