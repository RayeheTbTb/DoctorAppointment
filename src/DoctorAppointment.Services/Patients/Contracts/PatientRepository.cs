using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using System.Collections.Generic;

namespace DoctorAppointment.Services.Patients.Contracts
{
    public interface PatientRepository : Repository
    {
        void Add(Patient patient);
        void Delete(Patient patient);
        List<GetPatientDto> GetAll();
        GetPatientDto Get(int id);
        bool IsExistNationalCode(string nationalCode);
        bool IsExistId(int id);
        Patient FindByeId(int id);
        List<Patient> FindByNationalCode(string nationalCode);
    }
}
