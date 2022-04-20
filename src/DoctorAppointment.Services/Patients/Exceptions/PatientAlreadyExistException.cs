using System;

namespace DoctorAppointment.Services.Patients.Exceptions
{
    internal class PatientAlreadyExistException : Exception
    {
        public PatientAlreadyExistException()
        {
        }

        public PatientAlreadyExistException(string message) : base(message)
        {
        }
    }
}