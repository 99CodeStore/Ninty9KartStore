using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Data.Repository
{
    internal class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private NsdcTpDbContext context;

        public StudentRepository(NsdcTpDbContext context) : base(context)
        {
            this.context = context;
        }

        public Task Update(Student obj)
        {
            context.Students.Update(obj);
            return Task.CompletedTask;
        }

    }
}