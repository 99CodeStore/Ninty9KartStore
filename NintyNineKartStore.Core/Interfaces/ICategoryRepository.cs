using NintyNineKartStore.Core.Entities;

namespace NintyNineKartStore.Core.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        void Update(Category obj);
    }
}
