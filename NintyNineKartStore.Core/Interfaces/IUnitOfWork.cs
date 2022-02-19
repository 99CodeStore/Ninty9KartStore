
using System;
using System.Threading.Tasks;

namespace NintyNineKartStore.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task Save();
        ICategoryRepository Categories { get; }
        ICoverTypeRepository CoverTypes { get; }

        IProductRepository Products { get; }

        ICompanyRepository Companies { get; }

        IApplicationUserRepository ApplicationUsers { get; }

        IShoppingCartRepository ShoppingCarts { get; }

        IOrderHeaderRepository OrderHeaders { get; }

        IOrderDetailRepository OrderDetails { get; }

    }
}
