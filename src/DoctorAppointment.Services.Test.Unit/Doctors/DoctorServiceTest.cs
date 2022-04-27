using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Doctors;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Doctors.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointment.Services.Test.Unit.Doctors
{
    public class DoctorServiceTest
    {
        DoctorService _sut;
        DoctorRepository _repository;
        UnitOfWork _unitOfWork;
        ApplicationDbContext _dataContext;

        public DoctorServiceTest()
        {
            _dataContext = new EFInMemoryDatabase().CreateDataContext<ApplicationDbContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFDoctorRepository(_dataContext);
            _sut = new DoctorAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_doctor_properly()
        {
            AddDoctorDto dto = GenerateAddDoctorDto();

            _sut.Add(dto);

            _dataContext.Doctors.Should().Contain(_ => _.FirstName == dto.FirstName);
            _dataContext.Doctors.Should().Contain(_ => _.LastName == dto.LastName);
            _dataContext.Doctors.Should().Contain(_ => _.Field == dto.Field);
            _dataContext.Doctors.Should().Contain(_ => _.NationalCode == dto.NationalCode);
        }

        [Fact]
        public void Add_throws_DoctorAlreadyExistException_when_given_national_code_already_belongs_to_another_doctor_in_database()
        {
            Doctor doctor = GenerateDoctor();
            AddDoctorDto dto = GenerateAddDoctorDto();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<DoctorAlreadyExistException>();
        }

        [Fact]
        public void Update_updates_doctor_properly()
        {
            Doctor doctor = GenerateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            AddDoctorDto dto = GenerateAddDoctorDto("Updated");

            _sut.Update(doctor.Id, dto);

            var expected = _dataContext.Doctors.FirstOrDefault(_ => _.Id == doctor.Id);
            expected.FirstName.Should().Be(dto.FirstName);
            expected.LastName.Should().Be(dto.LastName);
            expected.Field.Should().Be(dto.Field);
            expected.NationalCode.Should().Be(dto.NationalCode);
        }

        [Fact]
        public void Updates_throws_DoctorNotFoundException_when_patient_with_given_id_does_not_exist()
        {
            int dummyId = 1;
            AddDoctorDto dto = GenerateAddDoctorDto();
            Action expected = () => _sut.Update(dummyId, dto);

            expected.Should().ThrowExactly<DoctorNotFoundException>();
        }

        /*[Fact]
        public void Update_throws_DuplicateNationalCodeException_when_another_doctor_with_given_nationalCode_exists()
        {
            AddDoctorDto dto = GenerateAddDoctorDto("2");
            var doctor1 = GenerateDoctor("1");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor1));
            var doctor2 = GenerateDoctor("2");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor2));

            Action expected = () => _sut.Update(doctor1.Id, dto);
            
            expected.Should().ThrowExactly<DuplicateNationalCodeException>();
        }*/

        [Fact]
        public void Delete_deletes_doctor_properly()
        {
            var doctor = GenerateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

            _sut.Delete(doctor.Id);
            _dataContext.Doctors.Should().NotContain(_ => _.Id == doctor.Id);
        }

        [Fact]
        public void Delete_throws_DoctorNotFoundException_when_doctor_with_given_id_does_not_exist()
        {
            int dummyId = 10;

            Action expected = () => _sut.Delete(dummyId);
            expected.Should().ThrowExactly<DoctorNotFoundException>();
        }

        [Fact]
        public void GetAll_returns_all_doctors()
        {
            var doctor = GenerateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.NationalCode == doctor.NationalCode);
            expected.Should().Contain(_ => _.FirstName == doctor.FirstName);
            expected.Should().Contain(_ => _.LastName == doctor.LastName);
            expected.Should().Contain(_ => _.Field == doctor.Field);
        }

        [Fact]
        public void Get_returns_doctor_with_given_id()
        {
            var doctor = GenerateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

            var expected = _sut.Get(doctor.Id);

            expected.FirstName.Should().Be(doctor.FirstName);
            expected.LastName.Should().Be(doctor.LastName);
            expected.Field.Should().Be(doctor.Field);
            expected.NationalCode.Should().Be(doctor.NationalCode);
        }

        private static AddDoctorDto GenerateAddDoctorDto(string addition)
        {
            return new AddDoctorDto
            {
                FirstName = "dummyDr" + addition,
                LastName = "dummyDrLn" + addition,
                Field = "dummyField" + addition,
                NationalCode = "dummyCode" + addition
            };
        }

        private static AddDoctorDto GenerateAddDoctorDto()
        {
            return new AddDoctorDto
            {
                FirstName = "dummyDr",
                LastName = "dummyDrLn",
                Field = "dummyField",
                NationalCode = "dummyCode"
            };
        }

        private static Doctor GenerateDoctor()
        {
            return new Doctor
            {
                Id = 1,
                FirstName = "dummyDr",
                LastName = "dummyDrLn",
                Field = "dummyField",
                NationalCode = "dummyCode"
            };
        }

        private static Doctor GenerateDoctor(string addition)
        {
            return new Doctor
            {
                Id = 1,
                FirstName = "dummyDr" + addition,
                LastName = "dummyDrLn" + addition,
                Field = "dummyField" + addition,
                NationalCode = "dummyCode" + addition
            };
        }
    }
}
