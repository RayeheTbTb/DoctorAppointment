using System;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public class DoctorAppointmentDateDto
    {
        public string DoctorFirstname { get; set; }
        public string DoctorLastName { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
    }
}
