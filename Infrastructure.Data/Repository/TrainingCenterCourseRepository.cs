using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Data.Repository
{
    public class TrainingCenterCourseRepository : GenericRepository<TrainingCenterCourse>, ITrainingCenterCourseRepository
    {
        private NsdcTpDbContext _db;

        public TrainingCenterCourseRepository(NsdcTpDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TrainingCenterCourse orderHeader)
        {
            _db.TrainingCenterCourses.Update(orderHeader);
        }

        //public async Task UpdateStatus(int Id, string orderStatus, string paymentStaus = null)
        //{
        //    var orderHeader = await _db.OrderHeaders.FindAsync(Id);
        //    if (orderHeader != null)
        //    {
        //        orderHeader.OrderStatus = orderStatus;
        //        if (paymentStaus != null)
        //        {
        //            orderHeader.PaymentStatus = paymentStaus;
        //        }
        //    }
        //}

        //public async Task UpdatePaymentDetail(int Id, string sessionId, string? paymentIntentId = null)
        //{
        //    var orderHeader = await _db.OrderHeaders.FindAsync(Id);
        //    if (orderHeader != null)
        //    {
        //        orderHeader.SessionId = sessionId;
        //        orderHeader.PaymentIntentId = paymentIntentId;
        //    }
        //}
    }
}
