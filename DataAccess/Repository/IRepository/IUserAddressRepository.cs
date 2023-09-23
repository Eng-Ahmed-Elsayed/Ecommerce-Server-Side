using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IUserAddressRepository : IRepository<UserAddress>
    {
        Task<bool> UpdateAsync(UserAddress userAddress);
    }
}
