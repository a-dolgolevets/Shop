using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using Shop.Domain.Base;
using Shop.Domain.Entities.Identity;
using Shop.Domain.Entities.Shop;

namespace Shop.Repositories.Context
{
    public class DatabaseContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>, IDatabaseContext
    {
        // Migrations constructor
        public DatabaseContext()
        {

        }

        public DatabaseContext(string connectionString, bool sqlDebugLogEnabled)
            : base(connectionString)
        {
            if (sqlDebugLogEnabled)
            {
                Database.Log = x => Debug.WriteLine(x);
            }
        }

        public new DbSet<T> Set<T>() where T : class, IBaseEntity
        {
            return base.Set<T>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Shop
            modelBuilder.Entity<ProductCategory>().HasMany(e => e.Products)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Cart>().HasMany(e => e.Items)
                .WithRequired(e => e.Cart)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CartItem>().HasRequired(e => e.Product);

            modelBuilder.Entity<Order>().HasMany(e => e.Items)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<OrderItem>().HasRequired(e => e.Product);
            #endregion

            #region Identity
            modelBuilder.Entity<User>().HasMany(e => e.Carts)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>().HasMany(e => e.Orders)
                .WithOptional(e => e.Customer)
                .WillCascadeOnDelete(true);
            #endregion
        }
    }
}