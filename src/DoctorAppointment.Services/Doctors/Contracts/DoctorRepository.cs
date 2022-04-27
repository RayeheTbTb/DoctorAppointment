using DoctorAppointment.Entities;
using System.Collections.Generic;

namespace DoctorAppointment.Services.Doctors.Contracts
{
    public interface DoctorRepository 
    {
        void Add(Doctor doctor);
        List<GetDoctorDto> GetAll();
        GetDoctorDto Get(int id);
        void Delete(Doctor doctor);
        bool IsExistNationalCode(string nationalCode);
        Doctor FindById(int id);
        bool IsExistId(int id);
        List<Doctor> FindByNationalCode(string nationalCode);
    }
}
