using DoctorAppointment.Infrastructure.Application;
using System.Collections.Generic;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public interface AppointmentService : Service
    {
        void Add(AddAppointmentDto dto);
        void Update(int id, UpdateAppointmentDto dto);
        void Delete(int id);
        List<PatientAppointmentsDto> GetPatientAppointments(int id);
        List<DoctorAppointmentsDto> GetDoctorAppointments(int id);
    }
}
