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
        public Ninty9KartStoreDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

    }
}
