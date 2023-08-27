﻿using Models.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IUserPaymentRepository : IRepository<UserPayment>
    {
        Task<bool> Update(UserPayment userPayments);
    }
}
