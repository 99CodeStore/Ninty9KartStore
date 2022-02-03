using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;

namespace NintyNineKartStore.Data.Repository
{
    internal class CoverTypeRepository : GenericRepository<CoverType>, ICoverTypeRepository
    {
        private Ninty9KartStoreDbContext context;

        public CoverTypeRepository(Ninty9KartStoreDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(CoverType obj)
        {
            context.CoverTypes.Update(obj);
        }

    }
}