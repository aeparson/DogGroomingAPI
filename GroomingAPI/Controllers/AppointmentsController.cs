using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grooming.Providers.Interfaces;
using Grooming.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Grooming.API.DTOMappings;
using Grooming.Data.Model;

namespace Grooming.API.Controllers
{
    /// <summary>
    /// The AppointmentsController exposes endpoints for appointment related actions.
    /// </summary>
    [ApiController]
    [Route("/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IAppointmentProvider _appointmentProvider;
        private readonly IMapper _mapper;

        public AppointmentsController(
            ILogger<AppointmentsController> logger,
            IAppointmentProvider appointmentProvider,
            IMapper mapper
        )
        {
            _logger = logger;
            _appointmentProvider = appointmentProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all of the appointments
        /// </summary>
        /// <returns> A 200 status code with a list of appointments.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAllAppointmentsAsync()
        {
            _logger.LogInformation("Request received for GetAllAppointmentsAsync");

            var appointments = await _appointmentProvider.GetAllAppointmentsAsync();
            var appointmentDTOs = _mapper.MapAppointmentsToAppointmentDtos(appointments);

            return Ok(appointmentDTOs);
        }


        /// <summary>
        /// Asynchronously retrieves the appointment with the provided id from the database.
        /// </summary>
        /// <param name="appointmentId">The id of the appointment to retrieve.</param>
        /// <returns>The appointment.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDTO>> GetAppointmentByIdAsync(int id)
        {
            _logger.LogInformation($"Request received for GetAppointmentByIdAsync for id: {id}");

            var appointment = await _appointmentProvider.GetAppointmentByIdAsync(id);
            var appointmentDTO = _mapper.MapAppointmentToAppointmentDto(appointment);

            return Ok(appointmentDTO);
        }


    }
}
