﻿using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public ICartItemRepository CartItem { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public IDiscountRepository Discount { get; private set; }

        public IInventoryRepository Inventory { get; private set; }

        public IOrderDetailsRepository OrderDetails { get; private set; }

        public IOrderItemRepository OrderItem { get; private set; }

        public IPaymentDetailsRepository PaymentDetails { get; private set; }

        public IProductRepository Product { get; private set; }

        public IShoppingSessionRepository ShoppingSession { get; private set; }

        public IUserAddressRepository UserAddress { get; private set; }

        public IUserPaymentRepository UserPayment { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CartItem = new CartItemRepository(_db);
            Category = new CategoryRepository(_db);
            Discount = new DiscountRepository(_db);
            Inventory = new InventoryRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            OrderItem = new OrderItemRepository(_db);
            PaymentDetails = new PaymentDetailsRepository(_db);
            Product = new ProductRepository(_db);
            ShoppingSession = new ShoppingSessionRepository(_db);
            UserAddress = new UserAddressRepository(_db);
            UserPayment = new UserPaymentRepository(_db);

        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}