using NintyNineKartStore.Core.Entities;

namespace NintyNineKartStore.Core.Interfaces
{
    public interface ICoverTypeRepository : IGenericRepository<CoverType>
    {
        void Update(CoverType obj);
    }

}
