using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;

namespace NintyNineKartStore.Data.Repository
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private Ninty9KartStoreDbContext _db;

        public ShoppingCartRepository(Ninty9KartStoreDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecreaseQuantity(ShoppingCart cart, int Quantity)
        {
            if (cart.Count - Quantity > 0)
            {
                cart.Count -= Quantity;
            }
            else
            {
                throw new System.Exception($"Product Quantity in the cart should be 1 or more.");
            }

            _db.ShoppingCarts.Update(cart);
            return cart.Count;
        }

        public int IncreaseQuantity(ShoppingCart cart, int Quantity)
        {
            cart.Count += Quantity;

            _db.ShoppingCarts.Update(cart);

            return cart.Count;
        }
    }
}
