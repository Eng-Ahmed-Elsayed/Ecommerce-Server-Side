using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<bool> UpdateAsync(Tag tag);
    }
}
