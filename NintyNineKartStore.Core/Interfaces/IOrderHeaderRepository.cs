using NintyNineKartStore.Core.Entities;
using System.Threading.Tasks;

namespace NintyNineKartStore.Core.Interfaces
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        Task UpdateStatus(int Id,string orderStatus,string? paymentStaus=null );

        Task UpdatePaymentDetail(int Id, string sessionId, string? paymentIntent = null);

    }
}
