using System;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public class DoctorAppointmentDetailsDto
    {
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime Date { get; set; }
    }
}
