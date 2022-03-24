using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;

namespace NsdcTraingPartnerHub.Data.Repository
{
    public class JobSectorRepository : GenericRepository<JobSector>, IJobSectorRepository
    {
        private NsdcTpDbContext _db;

        public JobSectorRepository(NsdcTpDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(JobSector obj)
        {
            _db.JobSectors.Update(obj);
        }
    }
}
