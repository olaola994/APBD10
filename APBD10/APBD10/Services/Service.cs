using APBD10.Data;
using APBD10.models;
using APBD10.models.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APBD10.Services
{
    public class Service
    {
        private readonly IConfiguration _configuration;
        private readonly context _context;

        public Service(IConfiguration configuration, context context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> AddPrescription(PrescriptionDTO request)
        {
            if (request.MedicamentList.Count > 10) return "Error: A prescription can contain a maximum of 10 medicaments.";
            if (request.DueDate < request.Date) return "Error: Due date cannot be earlier than the prescription date.";

            var doesPatientExist = await _context.Patients.FindAsync(request.Patient.IdPatient);
            if (doesPatientExist == null)
            {
                doesPatientExist = new Patient()
                {
                    FirstName = request.Patient.FirstName,
                    LastName = request.Patient.LastName,
                    BirthDate = request.Patient.BirthDate
                };
                _context.Patients.Add(doesPatientExist);
                await _context.SaveChangesAsync();
            }
            var doctor = await _context.Doctors.FindAsync(request.Doctor.IdDoctor);
            if (doctor == null)
                return "Error: Doctor not found.";
            var prescription = new Prescription
            {
                Date = request.Date,
                DueDate = request.DueDate,
                IdPatient = doesPatientExist.IdPatient,
                IdDoctor = doctor.IdDoctor,
                PrescriptionMedicaments = new List<Prescription_Medicament>()
            };

            foreach (var med in request.MedicamentList)
            {
                var doesMedicamentExist = await _context.Medicaments.FindAsync(med.IdMedicament);
                if (doesMedicamentExist == null)
                {
                    return $"Error: Medicament with ID {med.IdMedicament} not found.";
                }
                prescription.PrescriptionMedicaments.Add(new Prescription_Medicament
                {
                    IdMedicament = doesMedicamentExist.IdMedicament,
                    Dose = med.Dose,
                    Details = med.Description,
                    IdPrescription = prescription.IdPrescription
                });
            }
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return "Prescription added successfully.";
        }
        public async Task<PatientPrescriptionDTO> GetPatient(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
            {
                return null;
            }

            var prescriptions = await _context.Prescriptions
                .Where(p => p.IdPatient == patientId)
                .OrderBy(p => p.DueDate)
                .ToListAsync();
            var response = new PatientPrescriptionDTO()
            {
                Patient = new PatientDTO
                {
                    IdPatient = patient.IdPatient,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    BirthDate = patient.BirthDate
                },
                PrescriptionList = new List<PrescriptionDTO>()
            };
            foreach (var prescription in prescriptions)
            {
                var doctor = await _context.Doctors.FindAsync(prescription.IdDoctor);
                var prescriptionMedicaments = await _context.PrescriptionMedicaments
                    .Where(pm => pm.IdPrescription == prescription.IdPrescription)
                    .ToListAsync();
                var medicamentList = new List<MedicamentDTO>();
                foreach (var pm in prescriptionMedicaments)
                {
                    var medicament = await _context.Medicaments.FindAsync(pm.IdMedicament);
                    medicamentList.Add(new MedicamentDTO()
                    {
                        IdMedicament = medicament.IdMedicament,
                        Name = medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Details
                    });
                }
                response.PrescriptionList.Add(new PrescriptionDTO
                {
                    IdPrescription= prescription.IdPrescription,
                    Date = prescription.Date,
                    DueDate = prescription.DueDate,
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = doctor.IdDoctor,
                        FirstName = doctor.FirstName,
                        LastName = doctor.LastName
                    },
                    MedicamentList = medicamentList
                });
            }
            
            return response;
        }
    }
}
