using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Persistence.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(ApplicationDbContext).Assembly);
        }
    }
}
