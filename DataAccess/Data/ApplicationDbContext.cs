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

            //Roles Config
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            //User relations
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
            .HasOne<ShoppingSession>()
            .WithOne()
            .HasForeignKey<ShoppingSession>(e => e.UserId);

            // Product relations
            modelBuilder.Entity<Category>()
            .HasMany<Product>()
            .WithOne()
            .HasForeignKey(e => e.CategoryId)
            .IsRequired();

            modelBuilder.Entity<Inventory>()
            .HasMany<Product>()
            .WithOne()
            .HasForeignKey(e => e.InventoryId)
            .IsRequired(false);

            modelBuilder.Entity<Discount>()
            .HasMany(e => e.Products)
            .WithOne(e => e.Discount)
            .HasForeignKey(e => e.DiscoutId)
            .IsRequired(false);

            modelBuilder.Entity<Product>()
            .HasOne<OrderItem>()
            .WithOne();

            modelBuilder.Entity<Product>()
            .HasOne<CartItem>()
            .WithOne();

            // OrderDetails with OrderItem
            modelBuilder.Entity<OrderDetails>()
            .HasMany(e => e.OrderItems)
            .WithOne(e => e.OrderDetails)
            .HasForeignKey(e => e.OrderDetailsId)
            .IsRequired();

            // OrderDetails with PaymentDetails
            modelBuilder.Entity<PaymentDetails>()
            .HasOne<OrderDetails>()
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

            // ShoppingSession with CartItem
            modelBuilder.Entity<ShoppingSession>()
            .HasMany<CartItem>()
            .WithOne();
        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<OrderDetails> OrdersDetails { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentDetails> PaymentsDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingSession> ShoppingSessions { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPayment> UserPayments { get; set; }
    }
}
