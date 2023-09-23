using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IPaymentDetailsRepository : IRepository<PaymentDetails>
    {
        Task<bool> UpdateAsync(PaymentDetails paymentDetails);
    }
}
