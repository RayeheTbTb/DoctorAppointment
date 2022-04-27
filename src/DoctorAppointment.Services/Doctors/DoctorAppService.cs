using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Doctors.Exceptions;
using System.Collections.Generic;

namespace DoctorAppointment.Services.Doctors
{
    public class DoctorAppService : DoctorService
    {
        private readonly DoctorRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public DoctorAppService(
            DoctorRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddDoctorDto dto)
        {
            var doctor = new Doctor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Field = dto.Field,
                NationalCode = dto.NationalCode,
            };

            var isDoctorExist = _repository
                .IsExistNationalCode(doctor.NationalCode);

            if(isDoctorExist)
            {
                throw new DoctorAlreadyExistException();
            }

            _repository.Add(doctor);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var doctor = _repository.FindById(id);

            var isDoctorExist = _repository.IsExistId(id);
            if (!isDoctorExist)
            {
                throw new DoctorNotFoundException();
            }

            _repository.Delete(doctor);
            _unitOfWork.Commit();
        }

        public GetDoctorDto Get(int id)
        {
            return _repository.Get(id);
        }

        public List<GetDoctorDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, AddDoctorDto dto)
        {
            var doctor = _repository.FindById(id);

            var isDoctorExist = _repository.IsExistId(doctor.Id);
            if (!isDoctorExist)
            {
                throw new DoctorNotFoundException();
            }
            var checkDuplicateDoctors = _repository.FindByNationalCode(doctor.NationalCode);
            foreach (var checkDr in checkDuplicateDoctors)
            {
                if (checkDr.Id != doctor.Id)
                {
                    throw new DuplicateNationalCodeException();
                }
            }

            doctor.FirstName = dto.FirstName;
            doctor.LastName = dto.LastName;
            doctor.Field = dto.Field;
            doctor.NationalCode = dto.NationalCode;

            _unitOfWork.Commit();
        }
    }
}
