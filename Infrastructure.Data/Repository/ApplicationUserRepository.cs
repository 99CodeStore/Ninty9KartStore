using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;

namespace NsdcTraingPartnerHub.Data.Repository
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private NsdcTpDbContext _db;

        public ApplicationUserRepository(NsdcTpDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
