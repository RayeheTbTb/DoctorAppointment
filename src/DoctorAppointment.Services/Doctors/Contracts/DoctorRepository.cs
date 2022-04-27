using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using System.Collections.Generic;

namespace DoctorAppointment.Services.Doctors.Contracts
{
    public interface DoctorRepository : Repository
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
