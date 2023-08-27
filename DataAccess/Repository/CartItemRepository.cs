﻿using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Models;

namespace DataAccess.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private ApplicationDbContext _db;
        public CartItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> Update(CartItem cartItem)
        {
            if (cartItem == null) { return await Task.FromResult(false); }
            _db.CartItems.Update(cartItem);
            return await Task.FromResult(true);

        }
    }
}