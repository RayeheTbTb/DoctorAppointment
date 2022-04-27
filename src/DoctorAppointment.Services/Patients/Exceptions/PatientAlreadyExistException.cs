using System;

namespace DoctorAppointment.Services.Patients.Exceptions
{
    public class PatientAlreadyExistException : Exception
    {
        public PatientAlreadyExistException()
        {
        }

        public PatientAlreadyExistException(string message) : base(message)
        {
        }
    }
}