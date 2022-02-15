using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;

namespace NintyNineKartStore.Data.Repository
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private Ninty9KartStoreDbContext _db;

        public CompanyRepository(Ninty9KartStoreDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
