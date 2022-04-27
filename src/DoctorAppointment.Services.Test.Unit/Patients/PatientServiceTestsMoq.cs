using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Patients.Contracts;
using DoctorAppointment.Entities;
using DoctorAppointment.Services.Patients.Exceptions;
using FluentAssertions;

namespace DoctorAppointment.Services.Test.Unit.Patients
{
    public class PatientServiceTestsMoq
    {
        PatientAppService _sut;
        Mock<PatientRepository> _repository;
        Mock<UnitOfWork> _unitOfWork;

        public PatientServiceTestsMoq()
        {
            _repository = new Mock<PatientRepository>();
            _unitOfWork = new Mock<UnitOfWork>();
            _sut = new PatientAppService(_repository.Object, _unitOfWork.Object);
        }

        [Fact]
        public void Add_adds_patient_properly()
        {
            var dto = new AddPatientDto
            {
                FirstName = "dummyFn",
                LastName = "dummyLn",
                NationalCode = "dummyCode"
            };

            _sut.Add(dto);

            _repository.Verify(_ => _.Add(It.Is<Patient>(_ => _.NationalCode == dto.NationalCode)));
            _unitOfWork.Verify(_ => _.Commit());
        }

        [Fact]
        public void Add_throws_PatientAlreadyExistException_when_patient_with_given_nationalCode_exists()
        {
            var dto = new AddPatientDto
            {
                FirstName = "dummyFn",
                LastName = "dummyLn",
                NationalCode = "dummyCode"
            };

            _repository.Setup(_ => _.IsExistNationalCode(dto.NationalCode)).Returns(true);
            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<PatientAlreadyExistException>();
        }

        [Fact]
        public void Delete_deletes_patient_properly()
        {
            var patient = new Patient
            {
                Id = 1,
                FirstName = "dummyFn",
                LastName = "dummyLn",
                NationalCode = "dummyCode"
            };
            _repository.Setup(_ => _.FindByeId(patient.Id)).Returns(patient);
            _repository.Setup(_ => _.IsExistId(patient.Id)).Returns(true);

            _sut.Delete(patient.Id);

            _repository.Verify(_ => _.Delete(It.Is<Patient>(_ => _.Id == patient.Id)));
            _unitOfWork.Verify(_ => _.Commit());
        }

        [Fact]
        public void Delete_throws_PatientNotFoundException_when_patient_with_given_id_does_not_exist()
        {
            var dummyId = 10;

            Action expected = () => _sut.Delete(dummyId);

            expected.Should().ThrowExactly<PatientNotFoundException>();
        }
    }
}
