using Microsoft.AspNetCore.Mvc;
using workshop.wwwapi.DTO;
using workshop.wwwapi.Models;
using workshop.wwwapi.Repository;

namespace workshop.wwwapi.Endpoints
{
    public static class SurgeryEndpoint
    {
        //TODO:  add additional endpoints in here according to the requirements in the README.md 
        public static void ConfigurePatientEndpoint(this WebApplication app)
        {
            var surgeryGroup = app.MapGroup("surgery");

            surgeryGroup.MapGet("/patients", GetPatients);
            surgeryGroup.MapGet("/patients/{id}", GetPatientById);
            surgeryGroup.MapPost("/patients/{id}", CreatePatient);
            surgeryGroup.MapGet("/doctors", GetDoctors);
            surgeryGroup.MapGet("/appointmentsbydoctor/{id}", GetAppointmentsByDoctor);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> CreatePatient(IRepository<Patient> repository, PatientPost model)
        {

            var newPatient = new Patient()
            {
                FullName = model.FullName
            };
            var createdPatient = await repository.Insert(newPatient);

            var patientDTO = new PatientDTO()
            {
                Id = createdPatient.Id,
                FullName = createdPatient.FullName
            };

            return TypedResults.Created($"patients/{createdPatient.Id}", patientDTO);
   
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetPatients(IRepository<Patient> repository)
        {
            var patients = await repository.Get();
            var patientDTOs = new List<PatientDTO>();

            foreach (var p in patients)
            {
                var patientDTO = new PatientDTO()
                {
                    Id = p.Id,
                    FullName = p.FullName
                };
                patientDTOs.Add(patientDTO);
            }
            return TypedResults.Ok(patientDTOs);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetPatientById(IRepository<Patient> repository, int id)
        {

            var patient = await repository.GetById(id);
            if (patient == null) return TypedResults.NotFound("Patient not found");
            var patientDTO = new PatientDTO()
            {
                Id = patient.Id,
                FullName = patient.FullName
            };
            return TypedResults.Ok(patientDTO);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetDoctors(IRepository<Doctor> repository)
        {
            return TypedResults.Ok(await repository.Get());
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAppointmentsByDoctor(IRepository<Appointment> repository, int id)
        {
            return TypedResults.Ok(await repository.GetById(id));
        }
    }
}
