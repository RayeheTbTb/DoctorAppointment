using System;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public class GetAppointmentDto
    {
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime Date { get; set; }
    }
}