using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grooming.DTOs;
using Grooming.Providers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Grooming.Data.Model;
using Grooming.API.DTOMappings;

namespace Grooming.Controllers
{
    /// <summary>
    /// The PetController exposes endpoints for pet related actions.
    /// </summary>
    [ApiController]
    [Route("/pets")]
    public class PetsController : ControllerBase
    {
        private readonly ILogger<PetsController> _logger;
        private readonly IPetProvider _petProvider;
        private readonly IAppointmentProvider _appointmentProvider;
        private readonly IMapper _mapper;

        public PetsController(
            ILogger<PetsController> logger,
            IPetProvider petProvider,
            IAppointmentProvider appointmentProvider,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _petProvider = petProvider;
            _appointmentProvider = appointmentProvider;
        }

        /// <summary>
        /// Asynchronously retrieves all pets from the database.
        /// </summary>
        /// <returns>All pets in the database.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetDTO>>> GetPetsAsync()
        {
            _logger.LogInformation("Request received for GetPetsAsync");

            var pets = await _petProvider.GetPetsAsync();
            var petDTOs = _mapper.Map<IEnumerable<PetDTO>>(pets);

            return Ok(petDTOs);
        }

        /// <summary>
        /// Asynchronously retrieves the pet with the provided id from the database.
        /// </summary>
        /// <param name="petId">The id of the pet to retrieve.</param>
        /// <returns>The pet.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PetDTO>> GetPetByIdAsync(int id)
        {
            _logger.LogInformation($"Request received for GetPetByIdAsync for id: {id}");

            var pet = await _petProvider.GetPetByIdAsync(id);
            var petDTO = _mapper.Map<PetDTO>(pet);

            return Ok(petDTO);
        }

        /// <summary>
        /// Creates a new Pet and then maps it back to a petDTO.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PetDTO>> CreatePetAsync([FromBody] PetDTO newPetForm)
        {
            var newPet = _mapper.Map<Pet>(newPetForm);
            _logger.LogInformation("Request received for CreatePetAsync");
            var pet = await _petProvider.CreatePetAsync(newPet);
            var petDTO = _mapper.Map<PetDTO>(pet);

            return Created("/pets", petDTO);
        }

        /// <summary>
        /// Updates a Pet and then maps it back to a petDTO.
        /// </summary>
        [HttpPut("{petId}")]
        public async Task<ActionResult<PetDTO>> UpdatePetByIdAsync(Pet newPet, int petId)
        {
            _logger.LogInformation("Request received for UpdatePetByIdAsync");
            try
            {
                var updatedPet = await _petProvider.UpdatePetByIdAsync(newPet, petId);
                var petDTO = _mapper.Map<PetDTO>(updatedPet);
                return Ok(updatedPet);
            }
            catch (System.ArgumentNullException)
            {
                return NotFound($"No pet with ID: {petId} exists in the database");
            }
        }

        /// <summary>
        /// Finds a pet by ID and removes it from the repository if it has no reviews
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The deleted pet, or an error</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<PetDTO>> DeletePetByIdAsync(int id)
        {
            _logger.LogInformation("Request received for DeletePetByIdAsync");
            try
            {
                var pet = await _petProvider.DeletePetByIdAsync(id);
                return NoContent();
            }
            catch (System.ArgumentNullException)
            {
                return NotFound($"No pet with ID: {id} exists in the database");
            }

        }

        /// <summary>
        /// Updates a Appointment and then maps it back to a appointmentDTO.
        /// </summary>
        [HttpPut("{id}/appointments/{appointmentId}")]
        public async Task<ActionResult<AppointmentDTO>> UpdateAppointmentByIdAsync(Appointment newAppointment, int appointmentId)
        {
            Appointment updatedAppointment;
            _logger.LogInformation("Request received for UpdateAppointmentByIdAsync");
            try
            {
                updatedAppointment = await _appointmentProvider.UpdateAppointmentByIdAsync(newAppointment, appointmentId);
            }
            catch (System.ArgumentNullException)
            {
                return NotFound($"No appointment with ID: {appointmentId} exists in the database");
            }
            var appointmentDTO = _mapper.Map<AppointmentDTO>(updatedAppointment);
            return Ok(updatedAppointment);
        }


        /// <summary>
        /// Creates a new Appointment and then maps it back to a appointmentDTO.
        /// </summary>
        [HttpPost("{id}/appointments")]
        public async Task<ActionResult<AppointmentDTO>> CreateAppointmentAsync([FromBody] AppointmentDTO newAppointmentForm)
        {
            var newAppointment = _mapper.Map<Appointment>(newAppointmentForm);
            _logger.LogInformation("Request received for CreateAppointmentAsync");
            var appointment = await _appointmentProvider.CreateAppointmentAsync(newAppointment);
            var appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);

            return Created("/appointments", appointmentDTO);
        }

        /// <summary>
        /// Gets appointments by pet id
        /// </summary>
        /// <returns> A 200 status code with a list of appointments for that pet.</returns>
        [HttpGet("{petId}/appointments")]
        public async Task<ActionResult> GetAppointmentsByPetIdAsync(int petId)
        {
            List<Appointment> appointments;

            _logger.LogInformation("Request received for GetAppointmentsByPetIdAsync");

            appointments = await _petProvider.GetAppointmentsByPetIdAsync(petId);
            var appointmentDTOs = _mapper.Map<List<AppointmentDTO>>(appointments);

            return Ok(appointmentDTOs);
        }
    }
}