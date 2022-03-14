using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;

namespace NsdcTraingPartnerHub.Data.Repository
{
    public class TrainingCenterRepository : GenericRepository<TrainingCenter>, ITrainingCenterRepository
    {
        private NsdcTpDbContext _db;

        public TrainingCenterRepository(NsdcTpDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TrainingCenter obj)
        {
            _db.TrainingCenters.Update(obj);
        }
    }
}
