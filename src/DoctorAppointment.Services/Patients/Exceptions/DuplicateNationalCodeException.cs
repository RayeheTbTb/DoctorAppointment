using System;

namespace DoctorAppointment.Services.Patients.Exceptions
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