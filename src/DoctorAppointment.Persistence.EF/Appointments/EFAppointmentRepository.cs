﻿using DoctorAppointment.Entities;
using DoctorAppointment.Services.Appointments.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoctorAppointment.Persistence.EF.Appointments
{
    public class EFAppointmentRepository : AppointmentRepository
    {
        private readonly DbSet<Appointment> _appointments;
        private readonly DbSet<Doctor> _doctors;
        private readonly DbSet<Patient> _patients;

        public EFAppointmentRepository(ApplicationDbContext dbcontext)
        {
            _appointments = dbcontext.Set<Appointment>();
            _doctors = dbcontext.Set<Doctor>();
            _patients = dbcontext.Set<Patient>();
        }

        public void Add(Appointment appointment)
        {
            _appointments.Add(appointment);
        }

        public bool DuplicateAppointment(Appointment appointment)
        {
            foreach(var app in _appointments)
            {
                if ((app.Date.Date == appointment.Date.Date) && (app.DoctorId == appointment.DoctorId)
                    && (app.PatientId == appointment.PatientId))
                {
                    return true;
                }
            }
            return false;
        }

        public int GetAppointmentCount(int doctorId, DateTime Date)
        {
            return _appointments.Where(_ => _.Date.Date == Date.Date).Select(_ => _.DoctorId == doctorId).Count();
        }

        public void Delete(int id)
        {
            _appointments.Remove(FindById(id));
        }

        public Appointment FindById(int id)
        {
            return _appointments.Find(id);
        }

        public List<DoctorAppointmentsDto> GetDoctorAppointments(int id)
        {
            return _doctors.Where(_ => _.Id == id)
                .Select(_ => new DoctorAppointmentsDto
                {
                    FirstName = _.FirstName,
                    LastName = _.LastName,
                    Field = _.Field,
                    Appointments = _.Appointments
                    .Select(_ => new DoctorAppointmentDetailsDto
                    {
                        PatientFirstName = _.Patient.FirstName,
                        PatientLastName = _.Patient.LastName,
                        Date = _.Date
                    }).ToList()
                }).ToList();
        }

        public List<PatientAppointmentsDto> GetPatientAppointments(int id)
        {
            return _patients.Where(_ => _.Id == id)
                .Select(_ => new PatientAppointmentsDto
                {
                    FirstName = _.FirstName,
                    LastName = _.LastName,
                    Appointments = _.Appointments
                .Select(_ => new DoctorAppointmentDateDto
                {
                    DoctorFirstname = _.Doctor.FirstName,
                    DoctorLastName = _.Doctor.LastName,
                    DoctorId = _.DoctorId,
                    Date = _.Date
                }).ToList()
            }).ToList();
        }

        public bool IsExistId(int id)
        {
            return _appointments.AsNoTracking().Any(_ => _.Id == id);
        }

        public void Update(Appointment appointment)
        {
            _appointments.Update(appointment);
        }
    }
}