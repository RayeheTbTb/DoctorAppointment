using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Appointments.Exceptions;
using System.Collections.Generic;

namespace DoctorAppointment.Services.Appointments
{
    public class AppointmentAppService : AppointmentService
    {
        private readonly AppointmentRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public AppointmentAppService(AppointmentRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public void Add(AddAppointmentDto dto)
        {
            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                Date = dto.Date
            };

            var appCount = _repository.GetAppointmentCount(appointment.DoctorId, appointment.Date);
            if (appCount == 5)
            {
                throw new AppointmentLimitReachedException();
            }

            var duplicateApp = _repository.DuplicateAppointment(dto);
            if (duplicateApp)
            {
                throw new DuplicateAppointmentException();
            }

            _repository.Add(appointment);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var appointment = _repository.FindById(id);
            var isAppointmentExist = _repository.IsExistId(id);

            if (!isAppointmentExist)
            {
                throw new AppointmentNotFoundException();
            }

            _repository.Delete(appointment);
            _unitOfWork.Commit();
        }

        public List<DoctorAppointmentsDto> GetDoctorAppointments(int id)
        {
            return _repository.GetDoctorAppointments(id);
        }

        public List<PatientAppointmentsDto> GetPatientAppointments(int id)
        {
            return _repository.GetPatientAppointments(id);
        }

        public void Update(int id, AddAppointmentDto dto)
        {
            var appointment = _repository.FindById(id);

            var isAppointmentExist = _repository.IsExistId(id);
            if (!isAppointmentExist)
            {
                throw new AppointmentNotFoundException();
            }

            appointment.DoctorId = dto.DoctorId;
            appointment.PatientId = dto.PatientId;
            appointment.Date = dto.Date;

            _unitOfWork.Commit();
        }
    }
}
