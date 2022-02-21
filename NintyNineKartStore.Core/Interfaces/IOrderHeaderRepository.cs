using NintyNineKartStore.Core.Entities;

namespace NintyNineKartStore.Core.Interfaces
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int Id,string orderStatus,string? paymentStaus=null );

        void UpdatePaymentDetail(int Id, string sessionId, string? paymentIntent = null);

    }
}
