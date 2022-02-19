using NintyNineKartStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Core.Interfaces
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int Id,string orderStatus,string? paymentStaus=null );
    }
}
