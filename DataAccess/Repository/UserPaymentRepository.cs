using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class UserPaymentRepository : Repository<UserPayment>, IUserPaymentRepository
    {
        private ApplicationDbContext _db;
        public UserPaymentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> Update(UserPayment userPayment)
        {
            if (userPayment == null) { return await Task.FromResult(false); }
            _db.UserPayments.Update(userPayment);
            return await Task.FromResult(true);

        }
    }
}
