using DataAccess.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Roles Config
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            // Shipping options Config
            modelBuilder.ApplyConfiguration(new ShippingOptionConfiguration());

            // User relations
            modelBuilder.Entity<User>()
            .HasMany(e => e.UserAddresses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false);

            modelBuilder.Entity<User>()
            .HasMany(e => e.UserPayments)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false);

            modelBuilder.Entity<User>()
            .HasOne<OrderDetails>()
            .WithOne()
            .HasForeignKey<OrderDetails>(e => e.UserId)
            .IsRequired();

            modelBuilder.Entity<User>()
            .HasOne<ShoppingCart>()
            .WithOne()
            .HasForeignKey<ShoppingCart>(e => e.UserId);

            // Product relations
            modelBuilder.Entity<Category>()
            .HasMany<Product>()
            .WithOne(x => x.Category)
            .HasForeignKey(e => e.CategoryId)
            .IsRequired(false);

            modelBuilder.Entity<Inventory>()
            .HasMany(e => e.Products)
            .WithOne(e => e.Inventory)
            .HasForeignKey(e => e.InventoryId)
            .IsRequired(false);

            modelBuilder.Entity<Discount>()
            .HasMany(e => e.Products)
            .WithOne()
            .HasForeignKey(e => e.DiscoutId)
            .IsRequired(false);

            modelBuilder.Entity<Product>()
           .HasMany(e => e.ProductImages)
           .WithOne()
           .HasForeignKey(e => e.ProductId)
           .IsRequired(false);

            modelBuilder.Entity<Product>()
            .HasOne<OrderItem>()
            .WithOne();

            modelBuilder.Entity<CartItem>()
            .HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId)
            .IsRequired();

            modelBuilder.Entity<Product>()
            .HasMany(e => e.Tags)
            .WithMany();

            modelBuilder.Entity<Product>()
            .HasMany(e => e.Sizes)
            .WithMany();

            modelBuilder.Entity<Product>()
            .HasMany(e => e.Colors)
            .WithMany();


            // OrderDetails with OrderItem
            modelBuilder.Entity<OrderDetails>()
            .HasMany(e => e.OrderItems)
            .WithOne(e => e.OrderDetails)
            .HasForeignKey(e => e.OrderDetailsId)
            .IsRequired();


            // ShoppingCart with CartItem
            modelBuilder.Entity<ShoppingCart>()
            .HasMany(e => e.CartItems)
            .WithOne()
            .HasForeignKey(e => e.ShoppingCartId)
            .IsRequired();

            // OrderDetails Relations
            // OrderDetails with ShippingOption
            modelBuilder.Entity<OrderDetails>()
            .HasOne(e => e.ShippingOption)
            .WithMany()
            .HasForeignKey(e => e.ShippingOptionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);

            // OrderDetails with UserAddress
            modelBuilder.Entity<OrderDetails>()
            .HasOne(e => e.UserAddress)
            .WithMany()
            .HasForeignKey(e => e.UserAddressId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);

            // OrderDetails with UserPayment
            modelBuilder.Entity<OrderDetails>()
            .HasOne(e => e.UserPayment)
            .WithMany()
            .HasForeignKey(e => e.UserPaymentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);
        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<OrderDetails> OrdersDetails { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShippingOption> ShippingOptions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPayment> UserPayments { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Size> Sizes { get; set; }
    }
}
