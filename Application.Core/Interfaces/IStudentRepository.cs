using NsdcTraingPartnerHub.Core.Entities;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Core.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task Update(Student obj);
    }
}
