using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace NintyNineKartStore.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Ninty9KartStoreDbContext dbContext;
        //private IGenericRepository<Ingredient> _ingredients;

        public UnitOfWork(Ninty9KartStoreDbContext context)
        {
            this.dbContext = context;

            Categories = new CategoryRepository(context);
            CoverTypes = new CoverTypeRepository(context);

            Products = new ProductRepository(context);

        }

        public ICategoryRepository Categories { get; private set; }
        public ICoverTypeRepository CoverTypes { get; private set; }

        public IProductRepository Products { get; private set; }


        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
