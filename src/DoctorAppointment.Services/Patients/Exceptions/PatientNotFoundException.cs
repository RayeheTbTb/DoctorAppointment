using System;

namespace DoctorAppointment.Services.Patients.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException()
        {
        }

        public PatientNotFoundException(string message) : base(message)
        {
        }
    }
}