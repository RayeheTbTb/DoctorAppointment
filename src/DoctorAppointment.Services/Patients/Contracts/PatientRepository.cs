using DoctorAppointment.Entities;
using System.Collections.Generic;

namespace DoctorAppointment.Services.Patients.Contracts
{
    public interface PatientRepository
    {
        void Add(Patient patient);
        void Delete(Patient patient);
        void Update(Patient patient);
        List<GetPatientDto> GetAll();
        GetPatientDto Get(int id);
        bool IsExistNationalCode(string nationalCode);
        bool IsExistId(int id);
        Patient FindByeId(int id);
        List<Patient> FindByNationalCode(string nationalCode);
    }
}
