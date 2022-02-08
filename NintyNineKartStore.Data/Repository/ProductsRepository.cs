using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;

namespace NintyNineKartStore.Data.Repository
{
    internal class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private Ninty9KartStoreDbContext context;

        public ProductRepository(Ninty9KartStoreDbContext context):base(context)
        {
            this.context = context;
        }

        public void Update(Product obj)
        {
            context.Products.Update(obj);
        }
    }
}