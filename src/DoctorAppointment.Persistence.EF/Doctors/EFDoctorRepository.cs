using DoctorAppointment.Entities;
using DoctorAppointment.Services.Doctors.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DoctorAppointment.Persistence.EF.Doctors
{
    public class EFDoctorRepository : DoctorRepository
    {
        private readonly DbSet<Doctor> _doctors;

        public EFDoctorRepository(ApplicationDbContext dbcontext)
        {
            _doctors = dbcontext.Set<Doctor>();
        }

        public void Add(Doctor doctor)
        {
            _doctors.Add(doctor);
        }

        public void Delete(int id)
        {
            _doctors.Remove(FindById(id));
        }

        public Doctor FindById(int id)
        {
            return _doctors.Find(id);
        }

        public List<Doctor> FindByNationalCode(string nationalCode)
        {
            return _doctors.AsNoTracking().Where(_ => _.NationalCode == nationalCode).ToList();
        }

        public GetDoctorDto Get(int id)
        {
            var doctor =  _doctors.Where(_ => _.Id == id).FirstOrDefault();
            return new GetDoctorDto
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Field = doctor.Field,
                NationalCode = doctor.NationalCode
            };
        }

        public List<GetDoctorDto> GetAll()
        {
            return _doctors.Select(_ => new GetDoctorDto
            {
                Id = _.Id,
                FirstName = _.FirstName,
                LastName = _.LastName,
                Field = _.Field,
                NationalCode = _.NationalCode
            }).ToList();
        }

        public bool IsExistId(int id)
        {
            return _doctors.AsNoTracking().Any(_ => _.Id == id);
        }

        public bool IsExistNationalCode(string nationalCode)
        {
            return _doctors.AsNoTracking().Any(_ => _.NationalCode == nationalCode);
        }
    }
}
