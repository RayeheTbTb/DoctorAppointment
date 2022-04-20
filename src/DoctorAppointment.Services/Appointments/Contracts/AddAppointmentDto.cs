using System;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public class AddAppointmentDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime Date { get; set; }
    }
}