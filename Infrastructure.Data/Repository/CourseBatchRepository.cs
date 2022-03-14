using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;

namespace NsdcTraingPartnerHub.Data.Repository
{
    public class CourseBatchRepository : GenericRepository<CourseBatch>, ICourseBatchRepository
    {
        private NsdcTpDbContext _db;

        public CourseBatchRepository(NsdcTpDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(CourseBatch obj)
        {
            _db.CourseBatches.Update(obj);
        }
        //public int DecreaseQuantity(ShoppingCart cart, int Quantity)
        //{
        //    if (cart.Count - Quantity > 0)
        //    {
        //        cart.Count -= Quantity;
        //    }
        //    else
        //    {
        //        throw new System.Exception($"Product Quantity in the cart should be 1 or more.");
        //    }

        //    _db.ShoppingCarts.Update(cart);
        //    return cart.Count;
        //}

        //public int IncreaseQuantity(ShoppingCart cart, int Quantity)
        //{
        //    cart.Count += Quantity;

        //    _db.ShoppingCarts.Update(cart);

        //    return cart.Count;
        //}
    }
}
