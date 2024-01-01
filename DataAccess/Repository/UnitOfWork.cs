using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.DataQuerying;
using Models.Models;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        private ISortHelper<Product> _productSortHelper;

        public ICartItemRepository CartItem { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IDiscountRepository Discount { get; private set; }
        public IInventoryRepository Inventory { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IOrderItemRepository OrderItem { get; private set; }
        public IShippingOptionRepository ShippingOption { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IUserAddressRepository UserAddress { get; private set; }
        public IUserPaymentRepository UserPayment { get; private set; }
        public IColorRepository Color { get; private set; }
        public ITagRepository Tag { get; private set; }
        public ISizeRepository Size { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }
        public ICheckListRepository CheckList { get; private set; }
        public ICheckListItemRepository CheckListItem { get; private set; }
        public IReviewRepository Review { get; private set; }

        public UnitOfWork(ApplicationDbContext db, ISortHelper<Product> productSortHelper)
        {
            _db = db;
            _productSortHelper = productSortHelper;

            CartItem = new CartItemRepository(_db);
            Category = new CategoryRepository(_db);
            Discount = new DiscountRepository(_db);
            Inventory = new InventoryRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            OrderItem = new OrderItemRepository(_db);
            ShippingOption = new ShippingOptionRepository(_db);
            Product = new ProductRepository(_db, _productSortHelper);
            ShoppingCart = new ShoppingCartRepository(_db);
            UserAddress = new UserAddressRepository(_db);
            UserPayment = new UserPaymentRepository(_db);
            Color = new ColorRepository(_db);
            Tag = new TagRepository(_db);
            Size = new SizeRepository(_db);
            ProductImage = new ProductImageRepository(_db);
            CheckList = new CheckListRepository(_db);
            CheckListItem = new CheckListItemRepository(_db);
            Review = new ReviewRepository(_db);

        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
