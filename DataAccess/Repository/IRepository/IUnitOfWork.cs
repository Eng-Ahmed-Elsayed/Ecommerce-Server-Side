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
        IShippingOptionRepository ShippingOption { get; }
        IProductRepository Product { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IUserAddressRepository UserAddress { get; }
        IUserPaymentRepository UserPayment { get; }
        ITagRepository Tag { get; }
        ISizeRepository Size { get; }
        IColorRepository Color { get; }
        IProductImageRepository ProductImage { get; }
        ICheckListRepository CheckList { get; }
        ICheckListItemRepository CheckListItem { get; }

        Task SaveAsync();
    }
}
