using Autofac;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Appointments;
using DoctorAppointment.Persistence.EF.Doctors;
using DoctorAppointment.Persistence.EF.Patients;
using DoctorAppointment.Services.Appointments;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts;


namespace DoctorAppointment.RestAPI.Configurations
{
    public class RegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EFUnitOfWork>().As<UnitOfWork>();

            builder.RegisterType<DoctorAppService>().As<DoctorService>();
            builder.RegisterType<EFDoctorRepository>().As<DoctorRepository>();

            builder.RegisterType<PatientAppService>().As<PatientService>();
            builder.RegisterType<EFPatientRepository>().As<PatientRepository>(); 
            
            builder.RegisterType<AppointmentAppService>().As<AppointmentService>();
            builder.RegisterType<EFAppointmentRepository>().As<AppointmentRepository>();
        }
    }
}
