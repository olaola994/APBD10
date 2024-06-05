using APBD10.Data;
using APBD10.models;
using APBD10.models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APBD10.Services;

public class Service
{
    private readonly IConfiguration _configuration;
    private readonly context _context;

    public Service(IConfiguration configuration, context context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<string> AddPrescriptionToWarehouse(PrescriptionDTO request)
    {
        if (request.MedicamentList.Count > 10) return "Error: A prescription can contain a maximum of 10 medicaments.";
        if (request.DueDate >= request.Date) return "Error: Due date cannot be earlier than the prescription date.";
        
        var doesPatientExist = await _context.Patients.FindAsync(request.Patient.IdPatient);
        if (doesPatientExist == null)
        {
            var newPatient = new Patient()
            {
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                BirthDate = request.Patient.BirthDate
            };
            _context.Add(newPatient);
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
                Medicament = doesMedicamentExist,
                Dose = med.Dose,
                Details = med.Description
            });
            
        }
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
        return "Prescription added successfully.";
    }
}