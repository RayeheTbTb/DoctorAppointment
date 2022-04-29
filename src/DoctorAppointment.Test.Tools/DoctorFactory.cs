using DoctorAppointment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Test.Tools
{
    public static class DoctorFactory
    {
        public static Doctor CreateDoctor()
        {
            return new Doctor
            {
                FirstName = "dummy",
                LastName = "dummyLn",
                Field = "dummyField",
                NationalCode = "dummyCode"
            };
        }
    }
}
