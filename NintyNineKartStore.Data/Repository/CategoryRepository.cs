using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;

namespace NintyNineKartStore.Data.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private Ninty9KartStoreDbContext _db;

        public CategoryRepository(Ninty9KartStoreDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
