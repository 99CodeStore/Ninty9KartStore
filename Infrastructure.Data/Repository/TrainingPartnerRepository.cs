using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;

namespace NsdcTraingPartnerHub.Data.Repository
{
    public class TrainingPartnerRepository : GenericRepository<TrainingPartner>, ITrainingPartnerRepository
    {
        private NsdcTpDbContext _db;

        public TrainingPartnerRepository(NsdcTpDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TrainingPartner obj)
        {
            _db.TrainingPartners.Update(obj);
        }
    }
}
