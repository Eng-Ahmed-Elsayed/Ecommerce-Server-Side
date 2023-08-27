using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IPaymentDetailsRepository : IRepository<PaymentDetails>
    {
        Task<bool> Update(PaymentDetails paymentDetails);
    }
}
