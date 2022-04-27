using DoctorAppointment.Entities;
using DoctorAppointment.Infrastructure.Application;
using System;
using System.Collections.Generic;

namespace DoctorAppointment.Services.Appointments.Contracts
{
    public interface AppointmentRepository : Repository
    {
        void Add(Appointment appointment);
        bool IsExistId(int id);
        void Delete(Appointment appointment);
        List<PatientAppointmentsDto> GetPatientAppointments(int id);
        List<DoctorAppointmentsDto> GetDoctorAppointments(int id);
        Appointment FindById(int id);
        bool DuplicateAppointment(Appointment appointment);
        int GetAppointmentCount(int doctorId, DateTime Date);
    }
}
