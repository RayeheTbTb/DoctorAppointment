using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Patients.Contracts;
using DoctorAppointment.Services.Patients.Exceptions;
using System.Collections.Generic;

namespace DoctorAppointment.Services.Patients
{
    public class PatientAppService : PatientService
    {
        private readonly PatientRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public PatientAppService(PatientRepository patientRepository, UnitOfWork unitOfWork)
        {
            _repository = patientRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddPatientDto dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalCode = dto.NationalCode
            };

            var isPatientExist = _repository.IsExistNationalCode(patient.NationalCode);

            if (isPatientExist)
            {
                throw new PatientAlreadyExistException();
            }

            _repository.Add(patient);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var patient = _repository.FindByeId(id);
            var isPatientExist = _repository.IsExistId(id);
            if (!isPatientExist)
            {
                throw new PatientNotFoundException();
            }
            _repository.Delete(patient);
            _unitOfWork.Commit();
        }

        public GetPatientDto Get(int id)
        {
            var isPatientExist = _repository.IsExistId(id);
            if (!isPatientExist)
            {
                throw new PatientNotFoundException();
            }
            return _repository.Get(id);

        }

        public List<GetPatientDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdatePatientDto dto)
        {
            var patient = new Patient
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalCode = dto.NationalCode
            };
            var isPatientExist = _repository.IsExistId(id);
            if (!isPatientExist)
            {
                throw new PatientNotFoundException();
            }

            var checkDuplicatePatients = _repository.FindByNationalCode(patient.NationalCode);
            foreach (var checkDr in checkDuplicatePatients)
            {
                if (checkDr.Id != patient.Id)
                {
                    throw new DuplicateNationalCodeException();
                }
            }

            _repository.Update(patient);
            _unitOfWork.Commit();
        }
    }
}
