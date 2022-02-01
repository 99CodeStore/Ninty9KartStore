
using NintyNineKartStore.Core.Entities;
using System;
using System.Threading.Tasks;

namespace NintyNineKartStore.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task Save();
        IGenericRepository<Category> Categories { get; }
        //IGenericRepository<Ingredient> Ingredients { get; }
    }
    
}
