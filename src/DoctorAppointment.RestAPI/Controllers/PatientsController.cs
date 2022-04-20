using DoctorAppointment.Services.Patients.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DoctorAppointment.RestAPI.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : Controller
    {
        private readonly PatientService _service;

        public PatientsController(PatientService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddPatientDto dto)
        {
            _service.Add(dto);
        }

        [HttpPut("{id}")]
        public void Update(int id, UpdatePatientDto dto)
        {
            _service.Update(id, dto);
        }

        [HttpGet]
        public List<GetPatientDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public GetPatientDto Get(int id)
        {
            return _service.Get(id);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }
    }
}
