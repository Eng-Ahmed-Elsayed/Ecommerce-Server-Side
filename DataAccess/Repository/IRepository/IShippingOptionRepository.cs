using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IShippingOptionRepository : IRepository<ShippingOption>
    {
        Task<bool> UpdateAsync(ShippingOption shippingOption);
    }
}
