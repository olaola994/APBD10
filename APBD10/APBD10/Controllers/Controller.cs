using APBD10.Data;
using APBD10.models;
using APBD10.models.DTOs;
using APBD10.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace APBD10.Controllers
{
    [ApiController]
    [Route("/api/Medicaments")]
    public class Controller : ControllerBase
    {
        private readonly Service _services;
        private readonly context _context;

        public Controller(Service service, context context)
        {
            _services = service;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescpription(PrescriptionDTO request)
        {
            try
            {
                var result = await _services.AddPrescription(request);
                if (result.StartsWith("Error"))
                {
                    return BadRequest(result);  // Zwracanie szczegółowych błędów
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                // Logowanie wyjątku
                Console.Error.WriteLine($"Error occurred: {e.Message}");
                return StatusCode(500, $"Internal server error: {e.Message}");  // Szczegółowe odpowiedzi błędów
            }
        }

        [HttpGet]
        [Route("prescriptions/{id}")]
        public async Task<IActionResult> GetPrescription(int id)
        {
            try
            {
                var prescription = await _context.Prescriptions
                    .Include(p => p.Patient)
                    .Include(p => p.Doctor)
                    .Include(p => p.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
                    .FirstOrDefaultAsync(p => p.IdPrescription == id);

                if (prescription == null)
                    return NotFound("Prescription not found");

                var response = new PrescriptionDTO
                {
                    Date = prescription.Date,
                    DueDate = prescription.DueDate,
                    Patient = new PatientDTO
                    {
                        IdPatient = prescription.Patient.IdPatient,
                        FirstName = prescription.Patient.FirstName,
                        LastName = prescription.Patient.LastName,
                        BirthDate = prescription.Patient.BirthDate
                    },
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = prescription.Doctor.IdDoctor,
                        FirstName = prescription.Doctor.FirstName,
                        LastName = prescription.Doctor.LastName,
                        Email = prescription.Doctor.Email
                    },
                    MedicamentList = prescription.PrescriptionMedicaments.Select(pm => new MedicamentDTO
                    {
                        IdMedicament = pm.IdMedicament,
                        Dose = pm.Dose,
                        Description = pm.Details
                    }).ToList()
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                // Logowanie wyjątku
                Console.Error.WriteLine($"Error occurred: {e.Message}");
                return StatusCode(500, $"Internal server error: {e.Message}");  // Szczegółowe odpowiedzi błędów
            }
        }

        [HttpGet]
        [Route("patients/{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var result = await _services.GetPatient(id);
            if (result == null)
            {
                return NotFound(new { message = "Patient not found." });
            }
            return Ok(result);
        }
        
    }
}
