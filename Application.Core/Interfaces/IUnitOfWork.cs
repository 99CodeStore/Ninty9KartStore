
using System;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task Save();

        IStudentRepository Students { get; }

        IApplicationUserRepository ApplicationUsers { get; }

        ICenterAuthorityMemberRepository CenterAuthorityMembers { get; }
        ITrainingPartnerRepository TrainingPartners { get; }
        ITrainingCenterRepository TrainingCenters { get; }
        ICourseRepository Courses { get; }
        ISponsoringBodyRepository SponsoringBodies { get; }
        ITrainingCenterCourseRepository TrainingCenterCourses { get; }
        ICourseBatchRepository CourseBatches { get; }
        IJobSectorRepository JobSectors { get; }

    }
}
