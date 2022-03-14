using NsdcTraingPartnerHub.Core.Entities;

namespace NsdcTraingPartnerHub.Core.Interfaces
{
    public interface ITrainingCenterCourseRepository : IGenericRepository<TrainingCenterCourse>
    {
        void Update(TrainingCenterCourse obj);
    }
}
