using System;

namespace DoctorAppointment.Services.Doctors.Exceptions
{
    public class DuplicateNationalCodeException : Exception
    {
        public DuplicateNationalCodeException()
        {
        }

        public DuplicateNationalCodeException(string message) : base(message)
        {
        }
    }
}