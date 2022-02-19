using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Data.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private Ninty9KartStoreDbContext _db;

        public OrderDetailRepository(Ninty9KartStoreDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
