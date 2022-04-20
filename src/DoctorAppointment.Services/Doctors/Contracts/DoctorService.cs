using System.Collections.Generic;

namespace DoctorAppointment.Services.Doctors.Contracts
{
    public interface DoctorService
    {
        void Add(AddDoctorDto dto);
        void Update(int id, AddDoctorDto dto);
        void Delete(int id);
        List<GetDoctorDto> GetAll();
        GetDoctorDto Get(int id);
    }
}
