using System;

namespace DoctorAppointment.Services.Patients.Exceptions
{
    public class CannotUpdatePatientException : Exception
    {
        public CannotUpdatePatientException()
        {
        }

        public CannotUpdatePatientException(string message) : base(message)
        {
        }
    }
}