using NintyNineKartStore.Core.Entities;

namespace NintyNineKartStore.Core.Interfaces
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        int IncreaseQuantity(ShoppingCart obj,int Quantity);
        int DecreaseQuantity(ShoppingCart obj,int Quantity);
    }
}
