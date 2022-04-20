using System.Collections.Generic;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public class DoctorAppointmentsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Field { get; set; }
        public List<DoctorAppointmentDetailsDto> Appointments { get; set; }
    }
}