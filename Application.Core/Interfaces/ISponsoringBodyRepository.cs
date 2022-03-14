using NsdcTraingPartnerHub.Core.Entities;

namespace NsdcTraingPartnerHub.Core.Interfaces
{
    public interface ISponsoringBodyRepository : IGenericRepository<SponsoringBody>
    {
        void Update(SponsoringBody obj);
    }

}
