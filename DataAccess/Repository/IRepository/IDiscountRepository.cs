using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<bool> Update(Discount discount);
    }
}
