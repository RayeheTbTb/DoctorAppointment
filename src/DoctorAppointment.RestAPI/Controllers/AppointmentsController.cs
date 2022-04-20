using DoctorAppointment.Services.Appointments.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DoctorAppointment.RestAPI.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : Controller
    {
        private readonly AppointmentService _service;

        public AppointmentsController(AppointmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddAppointmentDto dto)
        {
            _service.Add(dto);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

        [HttpGet("patient/{id}")]
        public List<PatientAppointmentsDto> GetPatientAppointments(int id)
        {
            return _service.GetPatientAppointments(id);
        }


        [HttpGet("doctor/{id}")]
        public List<DoctorAppointmentsDto> GetDoctorAppointments(int id)
        {
            return _service.GetDoctorAppointments(id);
        }

        [HttpPut("{id}")]
        public void Update(int id, AddAppointmentDto dto)
        {
            _service.Update(id, dto);
        }
    }
}
