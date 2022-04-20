using System.Collections.Generic;

namespace DoctorAppointment.Services.Patients.Contracts
{
    public interface PatientService
    {
        void Add(AddPatientDto dto);
        void Update(int id, UpdatePatientDto dto);
        List<GetPatientDto> GetAll();
        GetPatientDto Get(int id);
        void Delete(int id);
    }
}
