using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DoctorAppointment.Services.Appointments;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Entities;
using FluentAssertions;
using DoctorAppointment.Services.Appointments.Exceptions;

namespace DoctorAppointment.Services.Test.Unit.Appointments
{
    public class AppointmentServiceTestsMoq
    {
        AppointmentAppService _sut;
        Mock<AppointmentRepository> _repository;
        Mock<UnitOfWork> _unitOfWork;

        public AppointmentServiceTestsMoq()
        {
            _repository = new Mock<AppointmentRepository>();
            _unitOfWork = new Mock<UnitOfWork>();
            _sut = new AppointmentAppService(_repository.Object, _unitOfWork.Object);
        }

        [Fact]
        public void Add_adds_appointment_properly()
        {
            var dto = new AddAppointmentDto
            {
                DoctorId = 1,
                PatientId = 1,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };

            _sut.Add(dto);

            _repository.Verify(_ => _.Add(It.Is<Appointment>(_ => _.DoctorId == dto.DoctorId)));
            _repository.Verify(_ => _.Add(It.Is<Appointment>(_ => _.PatientId == dto.PatientId)));
            _unitOfWork.Verify(_ => _.Commit());
        }

        [Fact]
        public void Add_throws_AppointmentLimitReachedException_when_trying_to_add_appointment_to_a_doctor_with_five_appointments()
        {
            var dto = new AddAppointmentDto
            {
                DoctorId = 1,
                PatientId = 1,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
            _repository.Setup(_ => _.GetAppointmentCount(dto.DoctorId, dto.Date)).Returns(5);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<AppointmentLimitReachedException>();
        }

        [Fact]
        public void Add_throws_DuplicateAppointmentException_when_there_is_already_an_appointment_with_all_values_equal_to_the_given_appointment_to_add()
        {
            var dto = new AddAppointmentDto
            {
                DoctorId = 1,
                PatientId = 1,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
            var appointment = new Appointment
            {
                DoctorId = 1,
                PatientId = 1,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
            _repository.Setup(_ => _.DuplicateAppointment(dto)).Returns(true);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<DuplicateAppointmentException>();

        }

        [Fact]
        public void GetPatientAppointments_returns_given_patient_id_appointments()
        {
            int dummyId = 1;
            _repository.Setup(_ => _.GetPatientAppointments(dummyId))
                .Returns(GeneratePatientAppointmentsList());

            var expected = _sut.GetPatientAppointments(dummyId);

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.FirstName == "dummy");
        }

        

        [Fact]
        public void GetDoctorAppointments_returns_given_doctor_id_appointments()
        {
            int dummyId = 1;
            _repository.Setup(_ => _.GetDoctorAppointments(dummyId))
                .Returns(GenerateDoctorAppointmentsList());

            var expected = _sut.GetDoctorAppointments(dummyId);

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.FirstName == "dummy");
        }

        

        [Fact]
        public void Update_updates_appointment_porperly()
        {
            var dto = new UpdateAppointmentDto
            {
                DoctorId = 2,
                PatientId = 2,
                Date = DateTime.Parse("2022-05-27T05:22:05.264Z")
            };
            var appointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
            _repository.Setup(_ => _.FindById(appointment.Id)).Returns(appointment);
            _repository.Setup(_ => _.IsExistId(appointment.Id)).Returns(true);

            _sut.Update(appointment.Id, dto);

            _unitOfWork.Verify(_ => _.Commit());
        }

        [Fact]
        public void Update_throws_AppointmentNotFoundException_when_appointment_with_given_id_does_not_exist()
        {
            var dummyId = 1;
            var dto = new UpdateAppointmentDto
            {
                DoctorId = 2,
                PatientId = 2,
                Date = DateTime.Parse("2022-05-27T05:22:05.264Z")
            };

            Action expected = () => _sut.Update(dummyId, dto);

            expected.Should().ThrowExactly<AppointmentNotFoundException>();
        }


        [Fact]
        public void Delete_deletes_appointment_properly()
        {
            var appointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
            _repository.Setup(_ => _.FindById(appointment.Id)).Returns(appointment);
            _repository.Setup(_ => _.IsExistId(appointment.Id)).Returns(true);

            _sut.Delete(appointment.Id);

            _repository.Verify(_ => _.Delete(It.Is<Appointment>(_ => _.Id == appointment.Id)));
            _unitOfWork.Verify(_ => _.Commit());
        }

        private static List<DoctorAppointmentsDto> GenerateDoctorAppointmentsList()
        {
            return new List<DoctorAppointmentsDto>
            {
                new DoctorAppointmentsDto
                {
                    FirstName = "dummy",
                    LastName = "dummyLn",
                    Field = "dummyFIeld",
                    Appointments = new List<DoctorAppointmentDetailsDto>
                    {
                        new DoctorAppointmentDetailsDto
                        {
                            PatientFirstName = "dummyPt",
                            PatientLastName = "dummyPtLn",
                            Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
                        }
                    }
                }
            };
        }

        private static List<PatientAppointmentsDto> GeneratePatientAppointmentsList()
        {
            return new List<PatientAppointmentsDto>
            {
                new PatientAppointmentsDto
                {
                    FirstName = "dummy",
                    LastName = "dummyLn",
                    Appointments = new List<DoctorAppointmentDateDto>
                    {
                        new DoctorAppointmentDateDto
                        {
                            DoctorFirstname = "dummyDr",
                            DoctorLastName = "dummDrLn",
                            DoctorId = 1,
                            Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
                        }
                    }
                }
            };
        }
    }
}
