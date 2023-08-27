using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class UserAddressRepository : Repository<UserAddress>, IUserAddressRepository
    {
        private ApplicationDbContext _db;
        public UserAddressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> Update(UserAddress userAddress)
        {
            if (userAddress == null) { return await Task.FromResult(false); }
            _db.UserAddresses.Update(userAddress);
            return await Task.FromResult(true);

        }
    }
}
