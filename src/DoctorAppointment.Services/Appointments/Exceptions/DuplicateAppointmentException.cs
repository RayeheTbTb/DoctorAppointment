using System;

namespace DoctorAppointment.Services.Appointments.Exceptions
{
    public class DuplicateAppointmentException : Exception
    {
        public DuplicateAppointmentException()
        {
        }

        public DuplicateAppointmentException(string message) : base(message)
        {
        }
    }
}