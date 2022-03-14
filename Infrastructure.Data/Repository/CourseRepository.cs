using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;

namespace NsdcTraingPartnerHub.Data.Repository
{
    internal class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private NsdcTpDbContext context;

        public CourseRepository(NsdcTpDbContext context):base(context)
        {
            this.context = context;
        }

        public void Update(Course obj)
        {
            context.Courses.Update(obj);
        }
    }
}