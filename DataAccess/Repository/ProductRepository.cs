using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.DataQuerying;
using Models.Models;

namespace DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        private ISortHelper<Product> _sortHelper;
        public ProductRepository(ApplicationDbContext db, ISortHelper<Product> sortHelper) : base(db)
        {
            _db = db;
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Product>> GetPagedListAsync(ProductParameters? productParameters = null,
            string? includeProperties = null)
        {
            IQueryable<Product> query = dbSet;
            if (productParameters != null)
            {
                // Name filter
                if (productParameters.Name != null)
                {
                    query = query.Where(x => x.Name.Contains(productParameters.Name));
                }
                // Colors filter
                // Get all products if have at least one color from the colrs names list
                if (productParameters.Colors != null)
                {
                    query = query.Where(x =>
                         x.Colors.Any(color => productParameters.Colors.Contains(color.Name)));
                }
                // Sizes filter
                if (productParameters.Sizes != null)
                {
                    query = query.Where(x =>
                         x.Sizes.Any(size => productParameters.Sizes.Contains(size.Name)));
                }
                // Price range filter
                if (productParameters.ValidPriceRange)
                {
                    query = query.Where(x => x.Price >= productParameters.MinPrice
                                && x.Price <= productParameters.MaxPrice);
                }
                // Availability filter
                // We could use the Invnetory.status prorperty but we would need to include
                // the Inventory and we want to just use includeProperties from the input.
                if (productParameters.Availability != null && productParameters.Availability.Count() != 3)
                {
                    if (productParameters.Availability.Count() == 1)
                    {
                        var val = productParameters.Availability[0];
                        query = val switch
                        {
                            "IN STOCK" => query.Where(x => x.Inventory.Quantity >= 15),
                            "LOW STOCK" => query.Where(x => x.Inventory.Quantity > 0
                                        && x.Inventory.Quantity < 15),
                            "OUT OF STOCK" => query.Where(x => x.Inventory.Quantity <= 0),
                        };
                    }
                    else
                    {
                        var inStock = !productParameters.Availability.Find(x => x == "IN STOCK").IsNullOrEmpty();
                        var lowStock = !productParameters.Availability.Find(x => x == "LOW STOCK").IsNullOrEmpty();
                        var outOfStock = !productParameters.Availability.Find(x => x == "OUT OF STOCK").IsNullOrEmpty();
                        if (inStock && lowStock)
                        {
                            query = query.Where(x => x.Inventory.Quantity > 0);
                        }
                        else if (inStock && outOfStock)
                        {
                            query = query.Where(x => x.Inventory.Quantity >= 15 || x.Inventory.Quantity == 0);
                        }
                        else
                        {
                            query = query.Where(x => x.Inventory.Quantity < 15);
                        }
                    }
                }

                if (!productParameters.Category.IsNullOrEmpty())
                {
                    query = query.Where(x => x.Category.Name == productParameters.Category
                                && x.Category.IsDeleted != true);
                }

                // Featured Products filter
                if (productParameters.Featured == true)
                {
                    query = query.Where(x => x.Featured == true);
                }

                // Not deleted filter and status is publish
                query = query.Where(x => x.IsDeleted != true && x.Status == "publish");

            }



            // Include Properties
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            // Sorting
            if (productParameters.OrderBy != null)
            {
                query = _sortHelper.ApplySort(query, productParameters.OrderBy);
            }

            return await PagedList<Product>.ToPagedList(query,
                productParameters.PageNumber,
                productParameters.PageSize);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            if (product == null) { return await Task.FromResult(false); }
            _db.Products.Update(product);
            return await Task.FromResult(true);

        }
    }
}
