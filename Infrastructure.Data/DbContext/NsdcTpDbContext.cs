using Microsoft.EntityFrameworkCore;
using NsdcTraingPartnerHub.Core.Entities;

namespace NsdcTraingPartnerHub.Data
{
    public class NsdcTpDbContext : DbContext
    {
        public NsdcTpDbContext(DbContextOptions<NsdcTpDbContext> options) : base(options)
        {

        }
        public DbSet<SponsoringBody> SponsoringBodyRepositories { get; set; }

        public DbSet<TrainingCenter> TrainingCenters { get; set; }

        public DbSet<TrainingPartner> TrainingPartners { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseBatch> CourseBatches { get; set; }
        public DbSet<TrainingCenterCourse> TrainingCenterCourses { get; set; }
        public DbSet<CenterAuthorityMember> CenterAuthorityMembers { get; set; }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Student References
            builder.Entity<Student>().HasOne(p => p.SponsoringBody).WithOne().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Student>().HasOne(p => p.ApplicationUser).WithOne().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Student>().HasOne(p => p.Course).WithOne().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Student>().HasOne(p => p.TrainingCenter).WithOne().OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<TrainingCenterCourse>().HasOne(p => p.Course).WithOne().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<TrainingCenterCourse>().HasOne(p => p.TrainingCenter).WithOne().OnDelete(DeleteBehavior.NoAction);

            builder.Entity<TrainingCenter>().HasOne(p => p.TrainingPartner).WithOne().OnDelete(DeleteBehavior.NoAction);
            
        }
    }
}
