using NsdcTraingPartnerHub.Core.Entities;

namespace NsdcTraingPartnerHub.Core.Interfaces
{
    public interface IJobSectorRepository : IGenericRepository<JobSector>
    {
        void Update(JobSector obj);
    }
}
