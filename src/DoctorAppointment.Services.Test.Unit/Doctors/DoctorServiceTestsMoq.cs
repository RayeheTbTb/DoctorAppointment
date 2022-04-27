using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Doctors.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointment.Services.Test.Unit.Doctors
{
    public class DoctorServiceTestsMoq
    {
        DoctorAppService _sut;
        Mock<DoctorRepository> _repository;
        Mock<UnitOfWork> _unitOfWork;

        public DoctorServiceTestsMoq()
        {
            _repository = new Mock<DoctorRepository>();
            _unitOfWork = new Mock<UnitOfWork>();
            _sut = new DoctorAppService(_repository.Object, _unitOfWork.Object);
        }

        [Fact]
        public void Add_adds_doctor_properly()
        {
            var dto = new AddDoctorDto
            {
                FirstName = "dummyDr",
                LastName = "dummyDrLn",
                Field = "dummyField",
                NationalCode = "dummyCode"
            };

            _sut.Add(dto);

            _repository.Verify(_ => _.Add(It.Is<Doctor>(_ => _.FirstName == dto.FirstName)));
            _unitOfWork.Verify(_ => _.Commit());
        }

        [Fact]
        public void Add_throws_DoctorAlreadyExistException_when_given_national_code_already_belongs_to_another_doctor_in_database()
        {
            var dto = new AddDoctorDto
            {
                FirstName = "dummyDr",
                LastName = "dummyDrLn",
                Field = "dummyField",
                NationalCode = "dummyCode"
            };
            _repository.Setup(_ => _.IsExistNationalCode(dto.NationalCode)).Returns(true);

            Action expected  = () => _sut.Add(dto);

            expected.Should().ThrowExactly<DoctorAlreadyExistException>();

        }

        [Fact]
        public void Update_updates_doctor_properly()
        {
            var dto = new AddDoctorDto
            {
                FirstName = "UpdatedDummyDr",
                LastName = "UpdatedDummyDrLn",
                Field = "UpdatedDummyField",
                NationalCode = "UpdatedDummyCode"
            };
            var doctor = new Doctor
            {
                Id = 1,
                FirstName = "dummy",
                LastName = "dummyLn",
                Field = "dummyField",
                NationalCode = "dummy"
            };
            List<Doctor> dupDocs = new List<Doctor>
            {
                doctor
            };
            _repository.Setup(_ => _.FindById(doctor.Id)).Returns(doctor);
            _repository.Setup(_ => _.IsExistId(doctor.Id)).Returns(true);
            _repository.Setup(_ => _.FindByNationalCode(doctor.NationalCode)).Returns(dupDocs);


            _sut.Update(doctor.Id, dto);

            _unitOfWork.Verify(_ => _.Commit());
        }

        [Fact]
        public void Delete_deletes_doctor_properly()
        {
            var doctor = new Doctor
            {
                Id = 1,
                FirstName = "dummy",
                LastName = "dummyLn",
                Field = "dummyField",
                NationalCode = "dummy"
            };
            _repository.Setup(_ => _.IsExistId(doctor.Id)).Returns(true);
            _repository.Setup(_ => _.FindById(doctor.Id)).Returns(doctor);

            _sut.Delete(doctor.Id);

            _repository.Verify(_ => _.Delete(It.Is<Doctor>(_ => _.Id == doctor.Id)));
            _unitOfWork.Verify(_ => _.Commit());
        }

        [Fact]
        public void Delete_throws_DoctorNotFoundException_when_doctor_with_given_id_does_not_exist()
        {
            var dummyId = 10;

            Action expected = () => _sut.Delete(dummyId);

            expected.Should().ThrowExactly<DoctorNotFoundException>();
        }

        [Fact]
        public void GetAll_returns_all_doctors()
        {
            _repository.Setup(_ => _.GetAll())
                .Returns(new List<GetDoctorDto>
                {
                    new GetDoctorDto
                    {
                        Id = 1,
                        FirstName = "dummy",
                        LastName = "dummyLn",
                        Field = "dummyField",
                        NationalCode = "dummy"
                    }
                });

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.NationalCode == "dummy");
            expected.Should().Contain(_ => _.Id == 1);
        }

        [Fact]
        public void Get_returns_doctor_with_given_id()
        {
            var dto = new GetDoctorDto
            {
                Id = 1,
                FirstName = "dummy",
                LastName = "dummyLn",
                Field = "dummyField",
                NationalCode = "dummy"
            };
            _repository.Setup(_ => _.Get(dto.Id)).Returns(dto);

            var expected = _sut.Get(dto.Id);

            expected.Id.Should().Be(dto.Id);
        }
    }
}
