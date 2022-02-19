using Microsoft.EntityFrameworkCore;
using NintyNineKartStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Data
{
    public class Ninty9KartStoreDbContext : DbContext
    {
        public Ninty9KartStoreDbContext(DbContextOptions<Ninty9KartStoreDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CoverType> CoverTypes { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);         
        }
    }
}
