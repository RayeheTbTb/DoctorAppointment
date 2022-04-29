using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Patients;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts;
using DoctorAppointment.Services.Patients.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointment.Services.Test.Unit.Patients
{
    public class PatientServiceTest
    {
        PatientService _sut;
        ApplicationDbContext _dataContext;
        PatientRepository _repository;
        UnitOfWork _unitOfWork;

        public PatientServiceTest()
        {
            _dataContext = new EFInMemoryDatabase().CreateDataContext<ApplicationDbContext>();
            _repository = new EFPatientRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new PatientAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_patient_properly()
        {
            AddPatientDto dto = GenerateAddPatientDto();

            _sut.Add(dto);

            _dataContext.Patients.Should().Contain(_ => _.FirstName == dto.FirstName);
            _dataContext.Patients.Should().Contain(_ => _.LastName == dto.LastName);
            _dataContext.Patients.Should().Contain(_ => _.NationalCode == dto.NationalCode);
        }

        [Fact]
        public void Add_throws_PatientAlreadyExistException_when_patient_with_given_nationalCode_exists()
        {
            var patient = GeneratePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            var dto = GenerateAddPatientDto();

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<PatientAlreadyExistException>();
        }

        [Fact]
        public void Delete_deletes_patient_properly()
        {
            var patient = GeneratePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            _sut.Delete(patient.Id);

            _dataContext.Patients.Should().NotContain(_ => _.Id == patient.Id);
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
            var patient = GeneratePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.FirstName == patient.FirstName);
            expected.Should().Contain(_ => _.LastName == patient.LastName);
            expected.Should().Contain(_ => _.NationalCode == patient.NationalCode);
        }

        [Fact]
        public void Get_returns_patient_with_given_id()
        {
            var patient = GeneratePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            var expected = _sut.Get(patient.Id);

            expected.FirstName.Should().Be(patient.FirstName);
            expected.LastName.Should().Be(patient.LastName);
            expected.NationalCode.Should().Be(patient.NationalCode);
        }

        [Fact]
        public void Update_updates_patient_properly()
        {
            var dto = GenerateUpdatePatientDto();
            var patient = GeneratePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            _sut.Update(patient.Id, dto);

            var expected = _dataContext.Patients.FirstOrDefault(_ => _.Id == patient.Id);
            expected.FirstName.Should().Be(dto.FirstName);
            expected.LastName.Should().Be(dto.LastName);
            expected.NationalCode.Should().Be(dto.NationalCode);
        }

        [Fact]
        public void Updates_throws_PatientNotFoundException_when_patient_with_given_id_does_not_exist()
        {
            var dummyId = 10;
            var dto = GenerateUpdatePatientDto();

            Action expected = () => _sut.Update(dummyId, dto);

            expected.Should().ThrowExactly<PatientNotFoundException>();
        }

        [Fact]
        public void Update_throws_DuplicateNationalCodeException_when_another_patient_with_given_nationalCode_exists()
        {
            var dto = GenerateUpdatePatientDto("2");
            var patient1 = GeneratePatient("1");
            _dataContext.Manipulate(_ => _.Patients.Add(patient1));
            var patient2 = GeneratePatient("2");
            _dataContext.Manipulate(_ => _.Patients.Add(patient2));

            Action expected = () => _sut.Update(patient1.Id, dto);
            expected.Should().ThrowExactly<DuplicateNationalCodeException>();

        }

        private static UpdatePatientDto GenerateUpdatePatientDto()
        {
            return new UpdatePatientDto
            {
                FirstName = "dummyFn",
                LastName = "dummyLn",
                NationalCode = "dummyCode"
            };
        }

        private static UpdatePatientDto GenerateUpdatePatientDto(string addition)
        {
            return new UpdatePatientDto
            {
                FirstName = "dummyFn" + addition,
                LastName = "dummyLn" + addition,
                NationalCode = "dummyCode" + addition
            };
        }

        private static AddPatientDto GenerateAddPatientDto()
        {
            return new AddPatientDto
            {
                FirstName = "dummyFn",
                LastName = "dummyLn",
                NationalCode = "dummyCode"
            };
        }

        private static AddPatientDto GenerateAddPatientDto(string addition)
        {
            return new AddPatientDto
            {
                FirstName = "dummyFn" + addition,
                LastName = "dummyLn" + addition,
                NationalCode = "dummyCode" + addition
            };
        }

        private static Patient GeneratePatient()
        {
            return new Patient
            {
                FirstName = "dummyFn",
                LastName = "dummyLn",
                NationalCode = "dummyCode"
            };
        }

        private static Patient GeneratePatient(string addition)
        {
            return new Patient
            {
                FirstName = "dummyFn" + addition,
                LastName = "dummyLn" + addition,
                NationalCode = "dummyCode" + addition
            };
        }
    }
}
