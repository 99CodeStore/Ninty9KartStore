using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;

namespace NintyNineKartStore.Data.Repository
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private Ninty9KartStoreDbContext _db;

        public ApplicationUserRepository(Ninty9KartStoreDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
