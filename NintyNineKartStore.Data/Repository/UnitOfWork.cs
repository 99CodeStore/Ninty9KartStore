using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace NintyNineKartStore.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Ninty9KartStoreDbContext dbContext;
        private IGenericRepository<Category> _categories;
        //private IGenericRepository<Ingredient> _ingredients;

        public UnitOfWork(Ninty9KartStoreDbContext context)
        {
            this.dbContext = context;
        }

        public IGenericRepository<Category> Categories => _categories ??= new GenericRepository<Category>(dbContext);

        //public IGenericRepository<Ingredient> Ingredients => _ingredients ??= new GenericRepository<Ingredient>(recipeBookDbContext);


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
