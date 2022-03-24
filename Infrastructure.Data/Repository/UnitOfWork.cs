using NsdcTraingPartnerHub.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NsdcTpDbContext dbContext;

        public UnitOfWork(NsdcTpDbContext context)
        {
            this.dbContext = context;

            SponsoringBodies = new SponsoringBodyRepository(context);

            Students = new StudentRepository(context);

            TrainingPartners = new TrainingPartnerRepository(context);

            TrainingCenters = new TrainingCenterRepository(context);

            ApplicationUsers = new ApplicationUserRepository(context);

            CenterAuthorityMembers = new CenterAuthorityMemberRepository(context);
            TrainingCenterCourses = new TrainingCenterCourseRepository(context);

            Courses = new CourseRepository(context);
            CourseBatches = new CourseBatchRepository(context);

            JobSectors = new JobSectorRepository(context);

        }

        public ICourseRepository Courses { get; private set; }
        public ISponsoringBodyRepository SponsoringBodies { get; private set; }

        public ITrainingPartnerRepository TrainingPartners { get; private set; }

        public ITrainingCenterRepository TrainingCenters { get; private set; }

        public IApplicationUserRepository ApplicationUsers { get; private set; }

        public ICourseBatchRepository CourseBatches { get; private set; }
        public ITrainingCenterCourseRepository TrainingCenterCourses { get; private set; }

        public ICenterAuthorityMemberRepository CenterAuthorityMembers { get; private set; }
        public IStudentRepository Students { get; set; }

        public IJobSectorRepository JobSectors { get; private set; }
        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
