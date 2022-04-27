using DoctorAppointment.Entities;
using DoctorAppointment.Services.Patients.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace DoctorAppointment.Persistence.EF.Patients
{
    public class EFPatientRepository : PatientRepository
    {
        private readonly DbSet<Patient> _patients;

        public EFPatientRepository(ApplicationDbContext dbContext)
        {
            _patients = dbContext.Set<Patient>();
        }

        public void Add(Patient patient)
        {
            _patients.Add(patient);
        }

        public void Delete(Patient patient)
        {
            _patients.Remove(patient);
        }

        public Patient FindByeId(int id)
        {
            return _patients.Find(id);
        }

        public List<Patient> FindByNationalCode(string nationalCode)
        {
            return _patients.AsNoTracking().Where(_ => _.NationalCode == nationalCode).ToList();
        }

        public GetPatientDto Get(int id)
        {
            var patient = _patients.Where(_ => _.Id == id).FirstOrDefault();
            return new GetPatientDto
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                NationalCode = patient.NationalCode
            };
        }

        public List<GetPatientDto> GetAll()
        {
            return _patients.Select(_ => new GetPatientDto
            {
                Id = _.Id,
                FirstName = _.FirstName,
                LastName = _.LastName,
                NationalCode = _.NationalCode
            }).ToList();
        }

        public bool IsExistId(int id)
        {
            return _patients.AsNoTracking().Any(_ => _.Id == id);
        }

        public bool IsExistNationalCode(string nationalCode)
        {
            return _patients.AsNoTracking().Any(_ => _.NationalCode == nationalCode);
        }

        public void Update(Patient patient)
        {
            _patients.Update(patient);
        }
    }
}
