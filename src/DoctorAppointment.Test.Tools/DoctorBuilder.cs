using DoctorAppointment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools
{
    public class DoctorBuilder
    {
        private Doctor doctor;

        public DoctorBuilder()
        {
            doctor = new Doctor
            {
                FirstName = "dummy",
                LastName = "dummyLn",
                Field = "dummyField",
                NationalCode = "dummyCode"
            };
        }

        public DoctorBuilder DoctorWithAppointment(string patientFirstName, string patientLastName, string nationalCode, DateTime date)
        {
            doctor.Appointments
                .Add(new Appointment
                {
                    Date = date,
                    Patient = new Patient
                    {
                        FirstName = patientFirstName,
                        LastName = patientLastName,
                        NationalCode = nationalCode
                    }
                });
            return this;
        }

        public Doctor Build()
        {
            return doctor;
        }
    }
}
