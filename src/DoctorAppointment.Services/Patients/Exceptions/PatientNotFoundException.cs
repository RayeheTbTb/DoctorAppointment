using System;

namespace DoctorAppointment.Services.Patients.Exceptions
{
    internal class PatientNotFoundException : Exception
    {
        public PatientNotFoundException()
        {
        }

        public PatientNotFoundException(string message) : base(message)
        {
        }
    }
}