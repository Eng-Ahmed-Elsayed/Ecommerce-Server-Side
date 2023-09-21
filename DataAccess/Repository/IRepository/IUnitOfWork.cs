namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICartItemRepository CartItem { get; }
        ICategoryRepository Category { get; }
        IDiscountRepository Discount { get; }
        IInventoryRepository Inventory { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderItemRepository OrderItem { get; }
        IPaymentDetailsRepository PaymentDetails { get; }
        IProductRepository Product { get; }
        IShoppingSessionRepository ShoppingSession { get; }
        IUserAddressRepository UserAddress { get; }
        IUserPaymentRepository UserPayment { get; }
        ITagRepository Tag { get; }
        IColorRepository Color { get; }
        IProductImageRepository ProductImage { get; }

        Task SaveAsync();
    }
}
