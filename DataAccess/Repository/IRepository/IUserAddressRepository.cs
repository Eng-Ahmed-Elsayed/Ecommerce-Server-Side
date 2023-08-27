using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IUserAddressRepository : IRepository<UserAddress>
    {
        Task<bool> Update(UserAddress userAddress);
    }
}
