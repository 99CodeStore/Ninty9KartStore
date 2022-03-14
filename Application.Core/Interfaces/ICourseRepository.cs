using NsdcTraingPartnerHub.Core.Entities;

namespace NsdcTraingPartnerHub.Core.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        void Update(Course obj);
    }
}
