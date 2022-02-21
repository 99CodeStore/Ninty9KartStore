using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Data.Repository
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private Ninty9KartStoreDbContext _db;

        public OrderHeaderRepository(Ninty9KartStoreDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }

        public async void UpdateStatus(int Id, string orderStatus, string paymentStaus = null)
        {
            var orderHeader = await _db.OrderHeaders.FindAsync(Id);
            if (orderHeader == null)
            {
                orderHeader.OrderStatus = orderStatus;
                if (paymentStaus != null)
                {
                    orderHeader.PaymentStatus = paymentStaus;
                }
            }
        }

        public async void UpdatePaymentDetail(int Id, string sessionId, string? paymentIntentId = null)
        {
            var orderHeader = await _db.OrderHeaders.FindAsync(Id);
            if (orderHeader == null)
            {
                orderHeader.SessionId = sessionId;
                orderHeader.PaymentIntentId = paymentIntentId;
            }
        }
    }
}
