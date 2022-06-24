using Grooming.Data.Interfaces;
using Grooming.Data.Model;
using Grooming.Providers.Interfaces;
using Grooming.Utilities.HttpResponseExceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Grooming.Providers.Providers
{
    /// <summary>
    /// This class provides the implementation of the IAppointmentProvider interface, providing service methods for Appointments.
    /// </summary>
    public class AppointmentProvider : IAppointmentProvider
    {
        private readonly ILogger<AppointmentProvider> _logger;
        private readonly IAppointmentRepository _appointmentRepository;
        

        public AppointmentProvider(IAppointmentRepository appointmentRepository, ILogger<AppointmentProvider> logger)
        {
            _logger = logger;
            _appointmentRepository = appointmentRepository;
        }

        /// <summary>
        /// Retrieves all petAppointments from the database.
        /// </summary>
        /// <param name="id">Appointment ID associated with petAppointments.</param>
        /// <returns>All petAppointments with an that matches searched id.</returns>
        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            List<Appointment> petAppointments;

            try
            {
                petAppointments = await _appointmentRepository.GetAllAppointmentsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }
            return petAppointments;
        }

        /// <summary>
        /// Asynchronously retrieves the appointment with the provided id from the database.
        /// </summary>
        /// <param name="appointmentId">The id of the appointment to retrieve.</param>
        /// <returns>The appointment.</returns>
        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            Appointment appointment;

            try
            {
                appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            if (appointment == null || appointment == default)
            {
                _logger.LogInformation($"Appointment with id: {appointmentId} could not be found.");
                throw new NotFoundException($"Appointment with id: {appointmentId} could not be found.");
            }

            return appointment;
        }

        

        /// <summary>
        /// Updates a appointment and saves it in the database. Throws error if null, or if there is a problem connecting to database.
        /// </summary>
        /// <param name="newAppointment"> new Appointment sent in from appointmentController to add to database </param>
        /// <param name="appointmentId">appointmentId to target for update </param>
        /// <returns>savedAppointment from the database which will be sent to appointmentController</returns>
        public async Task<Appointment> UpdateAppointmentByIdAsync(Appointment newAppointment, int appointmentId)
        {
            ValidateAppointmentInputFields(newAppointment);

            Appointment updatedAppointment;

            try
            {
                updatedAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }
            if (updatedAppointment == default || updatedAppointment == null)
            {
                _logger.LogInformation($"Appointment with id: {appointmentId} could not be found.");
                throw new NotFoundException($"Appointment with id: {appointmentId} could not be found.");
            }

            try
            {
                newAppointment = await _appointmentRepository.UpdateAppointmentByIdAsync(newAppointment, appointmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            return newAppointment;
        }

        /// <summary>
        /// Persists a Appointment to the database.
        /// </summary>
        /// <param name="model">AppointmentDTO used to build the appointment.</param>
        /// <returns>The persisted petAppointment with IDs.</returns>
        public async Task<Appointment> CreateAppointmentAsync(Appointment newAppointment)
        {
            ValidateAppointmentInputFields(newAppointment);

            Appointment savedAppointment;

            try
            {
                savedAppointment = await _appointmentRepository.CreateAppointmentAsync(newAppointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }
            return savedAppointment;
        }

        /// <summary>
        /// If a create appointment field is invalid, 
        /// the error message is added to a list of Bad Request errors to be thrown simultaneously
        /// when this function is called (see CreatePetAsync)
        /// </summary>
        /// <param name="newAppointment">Appointment DTO used to build the appointment</param>
        /// <exception cref="BadRequestException">string of bad request exceptions that have been added to the list</exception>
        public void ValidateAppointmentInputFields(Appointment newAppointment)
        {
            List<string> appointmentExceptions = new();

            if (newAppointment.PetId == default)
            {
                appointmentExceptions.Add("Pet Id is required.");
            }
            if (ValidateIfEmptyOrNull(newAppointment.Shampoos))
            {
                appointmentExceptions.Add("Shampoo Type is required.");
            }
            if (ValidateIfEmptyOrNull(newAppointment.Conditioners))
            {
                appointmentExceptions.Add("Conditioner Type is required.");
            }
            if (ValidateIfEmptyOrNull(newAppointment.Groomer))
            {
                appointmentExceptions.Add("Groomer is required.");
            }
            if (ValidateIfEmptyOrNull(newAppointment.BathMethods))
            {
                appointmentExceptions.Add("Bath Methods is required.");
            }
            if (ValidateIfEmptyOrNull(newAppointment.BrushInfo))
            {
                appointmentExceptions.Add("Brush Info is required.");
            }
            if (ValidateIfEmptyOrNull(newAppointment.BehaviorNotes))
            {
                appointmentExceptions.Add("Behavior Notes is required.");
            }
            if (ValidateIfEmptyOrNull(newAppointment.OwnerDebrief))
            {
                appointmentExceptions.Add("Owner Debrief is required.");
            }
            if (newAppointment.TotalCost == 0 || newAppointment.TotalCost == default)
            {
                appointmentExceptions.Add("Total Cost is required.");
            }
            if (!ValidateMoneyFormat(newAppointment.TotalCost))
            {
                appointmentExceptions.Add("Total Cost format must be DD.DD.");
            }
            if (ValidateIfEmptyOrNull(newAppointment.Date))
            {
                appointmentExceptions.Add("Date is required.");
            }
            else if (!ValidateAppointmentDateFormat(newAppointment.Date))
            {
                appointmentExceptions.Add("Date format must be yyyy-MM-dd.");
            }
            if (appointmentExceptions.Count > 0)
            {
                _logger.LogInformation(" ", appointmentExceptions);
                throw new BadRequestException(String.Join(" ", appointmentExceptions));
            }
        }


        /// <summary>
        /// Validates if string input field is null or empty
        /// </summary>
        /// <param name="modelField">string input field</param>
        /// <returns>boolean, true if input field is null or empty</returns>

        public bool ValidateIfEmptyOrNull(string modelField)
        {
            return string.IsNullOrWhiteSpace(modelField);
        }

        /// <summary>
        /// Validates if appointment date input matches required format.
        /// </summary>
        /// <param name="date">date to validate</param>
        /// <returns>boolean, true if input is valid, false if not.</returns>

        public bool ValidateAppointmentDateFormat(string date)
        {
            return Regex.IsMatch(date, @"^(\d{4})([-]{1})(0[1-9]|1[0-2])([-]{1})\d{2}$");
        }

        /// <summary>
        /// Validates if appointment price input matches required format.
        /// </summary>
        /// <param name="price">price to validate</param>
        /// <returns>boolean, true if input is valid, false if not.</returns>
        public bool ValidateMoneyFormat(decimal money)
        {
            var Cost = money;
            var stringVersion = Cost.ToString();
            return Regex.IsMatch(stringVersion, @"\d+\.\d\d(?!\d)");
        }

        

    }

}


