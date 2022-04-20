using System.Collections.Generic;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public class PatientAppointmentsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<DoctorAppointmentDateDto> Appointments { get; set; }
    }
}