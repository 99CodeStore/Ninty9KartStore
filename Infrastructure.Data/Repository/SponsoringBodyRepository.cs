using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;

namespace NsdcTraingPartnerHub.Data.Repository
{
    public class SponsoringBodyRepository : GenericRepository<SponsoringBody>, ISponsoringBodyRepository
    {
        private NsdcTpDbContext _db;

        public SponsoringBodyRepository(NsdcTpDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SponsoringBody obj)
        {
            _db.SponsoringBodyRepositories.Update(obj);
        }
    }
}
