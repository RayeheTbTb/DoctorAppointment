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

        [Fact]
        public void GetAll_returns_all_patients()
        {
            _repository.Setup(_ => _.GetAll())
                .Returns(new List<GetPatientDto>
                {
                    new GetPatientDto
                    {
                        Id = 1,
                        FirstName = "dummyFn",
                        LastName = "dummyLn",
                        NationalCode = "dummyCode"
                    }
                });

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Id == 1);
            expected.Should().Contain(_ => _.NationalCode == "dummyCode");
        }

        [Fact]
        public void Get_returns_patient_with_given_id()
        {
            var dto = new GetPatientDto
            {
                Id = 1,
                FirstName = "dummyFn",
                LastName = "dummyLn",
                NationalCode = "dummyCode"
            };
            _repository.Setup(_ => _.Get(dto.Id)).Returns(dto);
            _repository.Setup(_ => _.IsExistId(dto.Id)).Returns(true);

            var expected = _sut.Get(dto.Id);

            expected.Id.Should().Be(1);
        }

        [Fact]
        public void Update_updates_patient_properly()
        {
            var dto = new UpdatePatientDto
            {
                FirstName = "UpdatedDummyFn",
                LastName = "UpdatedDummyLn",
                NationalCode = "UpdatedDummyCode"
            };
            var patient = new Patient
            {
                Id = 1,
                FirstName = "dummyFn",
                LastName = "dummyLn",
                NationalCode = "dummyCode"
            };
            List<Patient> dupPatients = new List<Patient>
            {
                patient
            };
            _repository.Setup(_ => _.IsExistId(patient.Id)).Returns(true);
            _repository.Setup(_ => _.FindByNationalCode(patient.NationalCode)).Returns(dupPatients);
            _repository.Setup(_ => _.FindByeId(patient.Id)).Returns(patient);

            _sut.Update(patient.Id, dto);

            _unitOfWork.Verify(_ => _.Commit());
        }

        [Fact]
        public void Updates_throws_PatientNotFoundException_when_patient_with_given_id_does_not_exist()
        {
            var dto = new UpdatePatientDto
            {
                FirstName = "UpdatedDummyFn",
                LastName = "UpdatedDummyLn",
                NationalCode = "UpdatedDummyCode"
            };
            var dummyId = 10;

            Action expected = () => _sut.Update(dummyId, dto);

            expected.Should().ThrowExactly<PatientNotFoundException>();
        }

        [Fact]
        public void Update_throws_DuplicateNationalCodeException_when_another_patient_with_given_nationalCode_exists()
        {
            var dto = new UpdatePatientDto
            {
                FirstName = "UpdatedDummyFn",
                LastName = "UpdatedDummyLn",
                NationalCode = "dummyCode2"
            };
            Patient patient1 = new Patient
            {
                Id = 1,
                FirstName = "dummyFn",
                LastName = "dummyLn",
                NationalCode = "dummyCode"
            };
            Patient patient2 = new Patient
            {
                Id = 2,
                FirstName = "dummyFn2",
                LastName = "dummyLn2",
                NationalCode = "dummyCode2"
            };
            List<Patient> dupPatients = new List<Patient>
            {
                patient1, patient2
            };
            _repository.Setup(_ => _.FindByeId(patient1.Id)).Returns(patient1);
            _repository.Setup(_ => _.IsExistId(patient1.Id)).Returns(true);
            _repository.Setup(_ => _.FindByNationalCode(patient1.NationalCode)).Returns(dupPatients);

            Action expected = () => _sut.Update(patient1.Id, dto);

            expected.Should().ThrowExactly<DuplicateNationalCodeException>();
        }
    }
}
