using System;

namespace DoctorAppointment.Services.Appointments.Exceptions
{
    public class AppointmentLimitReachedException : Exception
    {
        public AppointmentLimitReachedException()
        {
        }

        public AppointmentLimitReachedException(string message) : base(message)
        {
        }
    }
}