using NintyNineKartStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Core.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        void Update(Company obj);
    }
}
