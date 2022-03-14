using NsdcTraingPartnerHub.Core.Entities;

namespace NsdcTraingPartnerHub.Core.Interfaces
{
    public interface ITrainingCenterRepository : IGenericRepository<TrainingCenter>
    {
        void Update(TrainingCenter obj);
    }
}
