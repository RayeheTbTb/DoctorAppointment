using System;

namespace DoctorAppointment.Services.Doctors.Exceptions
{
    public class CannotUpdateDoctorException : Exception
    {
        public CannotUpdateDoctorException()
        {
        }

        public CannotUpdateDoctorException(string message) : base(message)
        {
        }

        
    }
}