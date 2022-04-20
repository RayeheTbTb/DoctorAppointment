using System.Collections.Generic;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public interface AppointmentService
    {
        void Add(AddAppointmentDto dto);
        void Update(int id, AddAppointmentDto dto);
        void Delete(int id);
        List<PatientAppointmentsDto> GetPatientAppointments(int id);
        List<DoctorAppointmentsDto> GetDoctorAppointments(int id);
    }
}
