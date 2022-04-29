using DoctorAppointment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools
{
    public static class PatientFactory
    {
        public static Patient CreatePatient()
        {
            return new Patient
            {
                FirstName = "dummyPt",
                LastName = "dummyPtLn",
                NationalCode = "dummyCode",
            };
        }
    }
}
