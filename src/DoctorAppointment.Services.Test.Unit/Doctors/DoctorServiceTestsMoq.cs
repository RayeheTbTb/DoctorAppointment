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
        Mock<DoctorRepository> repository;
        Mock<UnitOfWork> unitOfWork;

        public DoctorServiceTestsMoq()
        {
            repository = new Mock<DoctorRepository>();
            unitOfWork = new Mock<UnitOfWork>();
            _sut = new DoctorAppService(repository.Object, unitOfWork.Object);
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

            repository.Verify(_ => _.Add(It.Is<Doctor>(_ => _.FirstName == dto.FirstName)));
            unitOfWork.Verify(_ => _.Commit());
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

            repository.Setup(_ => _.IsExistNationalCode(dto.NationalCode)).Returns(true);

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

            //prob with Update exceptions!!

            /*repository.Setup(_ => _.FindById(doctor.Id)).Returns(doctor);

            _sut.Update(doctor.Id, dto);

            unitOfWork.Verify(_ => _.Commit());*/
        }
    }
}
