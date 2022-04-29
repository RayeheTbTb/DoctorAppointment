using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Infrastructure.Test;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Appointments;
using DoctorAppointment.Services.Appointments;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Appointments.Exceptions;
using DoctorAppointment.Test.Tools;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointment.Services.Test.Unit.Appointments
{
    public class AppointmentServiceTest
    {
        AppointmentService _sut;
        ApplicationDbContext _dataContext;
        AppointmentRepository _repository;
        UnitOfWork _unitOfWork;

        public AppointmentServiceTest()
        {
            _dataContext = new EFInMemoryDatabase().CreateDataContext<ApplicationDbContext>();
            _repository = new EFAppointmentRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new AppointmentAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_appointment_properly()
        {
            var dto = CreateAddAppointmentDto();
            
            _sut.Add(dto);

            var expected = _dataContext.Appointments.FirstOrDefault(_ => _.DoctorId == dto.DoctorId);
            expected.PatientId.Should().Be(dto.PatientId);
            expected.Date.Should().Be(dto.Date);
        }

        [Fact]
        public void Add_throws_AppointmentLimitReachedException_when_trying_to_add_appointment_to_a_doctor_with_five_appointments()
        {
            Doctor doctor = CreateDoctorWith5AppointmentsInADayInDatabase();
            Patient patient = CreatePatientInDatabase();
            var dto = CreateAddAppointmentDtoForSpecificDoctorPatient(doctor.Id, patient.Id);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<AppointmentLimitReachedException>();
        }

        [Fact]
        public void Add_throws_DuplicateAppointmentException_when_there_is_already_an_appointment_with_all_values_equal_to_the_given_appointment_to_add()
        {
            Appointment appointment = AddAppointmentToDatabase();
            var dto = CreateAddAppointmentDtoForSpecificDoctorPatient(appointment.DoctorId, appointment.PatientId);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<DuplicateAppointmentException>();
        }

        [Fact]
        public void GetPatientAppointments_returns_given_patient_id_appointments()
        {
            var appointment = AddAppointmentToDatabase();

            var expected = _sut.GetPatientAppointments(appointment.PatientId);

            expected.Should().Contain(_ => _.FirstName == appointment.Patient.FirstName);
            expected.Should().Contain(_ => _.LastName == appointment.Patient.LastName);
            expected.Should().Contain(_ => _.Appointments.First().DoctorId == appointment.DoctorId);
            expected.Should().Contain(_ => _.Appointments.First().Date == appointment.Date);

        }

        [Fact]
        public void GetDoctorAppointments_returns_given_doctor_id_appointments()
        {
            var appointment = AddAppointmentToDatabase();

            var expected = _sut.GetDoctorAppointments(appointment.DoctorId);

            expected.Should().Contain(_ => _.FirstName == appointment.Doctor.FirstName);
            expected.Should().Contain(_ => _.LastName == appointment.Doctor.LastName);
            expected.Should().Contain(_ => _.Appointments.First().PatientFirstName == appointment.Patient.FirstName);
            expected.Should().Contain(_ => _.Appointments.First().PatientLastName == appointment.Patient.LastName);
            expected.Should().Contain(_ => _.Appointments.First().Date == appointment.Date);
            expected.Should().Contain(_ => _.Field == appointment.Doctor.Field);
        }

        [Fact]
        public void Update_updates_appointments_properly()
        {
            var appointment = AddAppointmentToDatabase();
            var dto = CreateUpdateAppointmentDto();

            _sut.Update(appointment.Id, dto);

            var expected = _dataContext.Appointments.FirstOrDefault(_ => _.Id == appointment.Id);
            expected.Id.Should().Be(appointment.Id);
            expected.DoctorId.Should().Be(dto.DoctorId);
            expected.PatientId.Should().Be(dto.PatientId);
            expected.Date.Should().Be(dto.Date);
        }

        [Fact]
        public void Update_throws_AppointmentNotFoundException_when_appointment_with_given_id_does_not_exist()
        {
            var dummyId = 1;
            var dto = CreateUpdateAppointmentDto();

            Action expected = () => _sut.Update(dummyId, dto);

            expected.Should().ThrowExactly<AppointmentNotFoundException>();
        }

        [Fact]
        public void Update_throws_AppointmentLimitReachedException_when_trying_to_assign_more_than_five_appointments_in_a_day_to_a_doctor()
        {
            var appointment = AddAppointmentToDatabase();
            var doctor = CreateDoctorWith5AppointmentsInADayInDatabase();
            var patient = CreatePatientInDatabase();
            var dto = CreateUpdateAppointmentDtoForSpecificDoctorPatient(doctor.Id, patient.Id);

            Action expected = () => _sut.Update(appointment.Id, dto);

            expected.Should().ThrowExactly<AppointmentLimitReachedException>();
        }

        [Fact]
        public void Delete_deletes_appointment_properly()
        {
            Appointment appointment = AddAppointmentToDatabase();

            _sut.Delete(appointment.Id);

            _dataContext.Appointments.Should().NotContain(appointment);
        }

        private Patient CreatePatientInDatabase()
        {
            var patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            return patient;
        }

        private Appointment AddAppointmentToDatabase()
        {
            Doctor doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

            Patient patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            Appointment appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
            _dataContext.Manipulate(_ => _.Appointments.Add(appointment));

            return appointment;
        }

        private AddAppointmentDto CreateAddAppointmentDto()
        {
            Doctor doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

            Patient patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            return new AddAppointmentDto
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
        }

        private AddAppointmentDto CreateAddAppointmentDtoForSpecificDoctorPatient(int doctorId, int patientId)
        {
            return new AddAppointmentDto
            {
                DoctorId = doctorId,
                PatientId = patientId,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
        }

        private UpdateAppointmentDto CreateUpdateAppointmentDto()
        {
            Doctor doctor = DoctorFactory.CreateDoctor();
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

            Patient patient = PatientFactory.CreatePatient();
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            return new UpdateAppointmentDto
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
        }

        private UpdateAppointmentDto CreateUpdateAppointmentDtoForSpecificDoctorPatient(int doctorId, int patientId)
        {
            return new UpdateAppointmentDto
            {
                DoctorId = doctorId,
                PatientId = patientId,
                Date = DateTime.Parse("2022-04-27T05:22:05.264Z")
            };
        }

        private Doctor CreateDoctorWith5AppointmentsInADayInDatabase()
        {
            var doctor =  new DoctorBuilder()
                .DoctorWithAppointment("sth1", "sth1Ln", "11", DateTime.Parse("2022-04-27T05:22:05.264Z"))
                .DoctorWithAppointment("sth2", "sth2Ln", "12", DateTime.Parse("2022-04-27T05:22:05.264Z"))
                .DoctorWithAppointment("sth3", "sth3Ln", "13", DateTime.Parse("2022-04-27T05:22:05.264Z"))
                .DoctorWithAppointment("sth4", "sth4Ln", "14", DateTime.Parse("2022-04-27T05:22:05.264Z"))
                .DoctorWithAppointment("sth5", "sth5Ln", "15", DateTime.Parse("2022-04-27T05:22:05.264Z"))
                .Build();

            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            return doctor;
        }
    }
}
