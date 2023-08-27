using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{

    public class PaymentDetailsRepository : Repository<PaymentDetails>, IPaymentDetailsRepository
    {
        private ApplicationDbContext _db;
        public PaymentDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> Update(PaymentDetails paymentDetails)
        {
            if (paymentDetails == null) { return await Task.FromResult(false); }
            _db.PaymentsDetails.Update(paymentDetails);
            return await Task.FromResult(true);

        }
    }
}
