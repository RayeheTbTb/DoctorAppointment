using DoctorAppointment.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorAppointment.Persistence.EF.Patients
{
    public class PatientEntityMap : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> _)
        {
            _.ToTable("Patients");
            _.HasKey(_ => _.Id);

            _.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            _.Property( _=> _.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.LastName)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.NationalCode)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}
