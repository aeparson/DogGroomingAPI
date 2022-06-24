using Grooming.Data.Interfaces;
using Grooming.Data.Model;
using Grooming.Data.SeedData;
using Grooming.Providers.Interfaces;
using Grooming.Utilities.HttpResponseExceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Grooming.Providers.Providers
{
    /// <summary>
    /// This class provides the implementation of the IPetProvider interface, providing service methods for pets.
    /// </summary>
    public class PetProvider : IPetProvider
    {
        private readonly ILogger<PetProvider> _logger;
        private readonly IPetRepository _petRepository;


        public PetProvider(IPetRepository petRepository, ILogger<PetProvider> logger)
        {
            _logger = logger;
            _petRepository = petRepository;

        }

        /// <summary>
        /// Asynchronously retrieves the appointments with the provided pet id from the database.
        /// </summary>
        /// <param name="petId">The id of the appointment to retrieve.</param>
        /// <returns>The appointment.</returns>
        public async Task<List<Appointment>> GetAppointmentsByPetIdAsync(int petId)
        {
            List<Appointment> appointment;

            try
            {
                appointment = await _petRepository.GetAppointmentsByPetIdAsync(petId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            if (appointment == null || appointment == default)
            {
                _logger.LogInformation($"Pet with id: {petId} has no appointments.");
                throw new NotFoundException($"Pet with id: {petId} has no appointments.");
            }

            return appointment;
        }
        /// <summary>
        /// Asynchronously retrieves the pet with the provided id from the database.
        /// </summary>
        /// <param name="petId">The id of the pet to retrieve.</param>
        /// <returns>The pet.</returns>
        public async Task<Pet> GetPetByIdAsync(int petId)
        {
            Pet pet;

            try
            {
                pet = await _petRepository.GetPetByIdAsync(petId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            if (pet == null || pet == default)
            {
                _logger.LogInformation($"Pet with id: {petId} could not be found.");
                throw new NotFoundException($"Pet with id: {petId} could not be found.");
            }

            return pet;
        }

        /// <summary>
        /// Asynchronously retrieves all pets from the database.
        /// </summary>
        /// <returns>All pets in the database.</returns>
#nullable enable
        public async Task<IEnumerable<Pet>> GetPetsAsync()
        {

            IEnumerable<Pet> pets;

            try
            {
                pets = await _petRepository.GetPetsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            return pets;
        }

        /// <summary>
        /// Creates a new pet and saves it in the database. Throws error if null, or if there is a problem connecting to database.
        /// </summary>
        /// <param name="newPet">new Pet sent in from petController to add to database</param>
        /// <returns>savedPet from the database which will be sent to petController</returns>
        public async Task<Pet> CreatePetAsync(Pet newPet)
        {

            if (await ValidateSingleEmail(newPet))
            {
                throw new ConflictException("That Email is already in use");
            }
            ValidatePetInputFields(newPet);

            if (newPet == null)
            {
                _logger.LogError("Pet must have value.");
                throw new BadRequestException("Pet must have value.");
            }

            var myPetFactory = new PetFactory();

            Pet savedPet;

            try
            {
                savedPet = await _petRepository.CreatePetAsync(newPet);

                _logger.LogInformation("Pet saved.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }
            return savedPet;
        }

        /// <summary>
        /// Updates a pet and saves it in the database. Throws error if null, or if there is a problem connecting to database.
        /// </summary>
        /// <param name="newPet"> new Pet sent in from petController to add to database </param>
        /// <param name="petId">petId to target for update </param>
        /// <returns>savedPet from the database which will be sent to petController</returns>
        public async Task<Pet> UpdatePetByIdAsync(Pet newPet, int petId)
        {

            ValidatePetInputFields(newPet);

            Pet updatedPet;

            try
            {
                updatedPet = await _petRepository.GetPetByIdAsync(petId);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }
            if (updatedPet == default)
            {
                _logger.LogInformation($"Pet with id: {petId} could not be found.");
                throw new NotFoundException($"Pet with id: {petId} could not be found.");
            }
            if (newPet.OwnerEmail != updatedPet.OwnerEmail)

                if (await ValidateSingleEmail(newPet))
                {
                    throw new ConflictException("That email is already in use");
                }


            try
            {
                newPet = await _petRepository.UpdatePetByIdAsync(newPet, petId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            return newPet;
        }


        /// <summary>
        /// Deletes a pet from the database.
        /// </summary>
        /// <param name="petId">petId to target for delete request</param> 
        /// <returns> empty space where pet was before</returns>
        public async Task<Pet> DeletePetByIdAsync(int petId)
        {
            if (await IsDeleteable(petId))
            {
                var pet = await _petRepository.DeletePetByIdAsync(petId);
                return pet;
            }
            throw new ConflictException("Pet has appointments and cannot be deleted");
        }

        /// <summary>
        /// Checks if a pet can be deleted from the database.
        /// </summary>
        /// <param name="petId">petId to target for delete request</param> 
        /// <returns> true if pet has no appointments and can be deleted, false if not.</returns>
        public async Task<bool> IsDeleteable(int petId)
        {
            var pet = await GetPetByIdAsync(petId);
            var appointments = pet.Appointments;
            if (appointments == null)
            {
                return true;
            }
            if (appointments.Count == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// If a create pet field is invalid, 
        /// the error message is added to a list of Bad Request errors to be thrown simultaneously
        /// when this function is called (see CreatePetAsync)
        /// </summary>
        /// <param name="newPet">Pet DTO used to build the pet</param>
        /// <exception cref="BadRequestException">string of bad request exceptions that have been added to the list</exception>
        public void ValidatePetInputFields(Pet newPet)
        {
            List<string> petExceptions = new();

            if (ValidateIfEmptyOrNull(newPet.PetName))
            {
                petExceptions.Add("Pet Name is required.");
            }
            else if (!ValidateIfAlphabeticHyphenAndApostropheOnly(newPet.PetName))
            {
                petExceptions.Add("Pet name must contain only alphabetic letters, hyphens(-), or apostrophes(').");
            }
            if (newPet.Age == 0 || newPet.Age == default)
            {
                petExceptions.Add("Age is required.");
            }
            else if (newPet.Age < 0)
            {
                petExceptions.Add("Age must be a whole number larger than zero.");
            }
            if (newPet.Weight == 0 || newPet.Weight == default)
            {
                petExceptions.Add("Weight is required.");
            }
            else if (newPet.Weight < 0)
            {
                petExceptions.Add("Weight must be a whole number larger than zero.");
            }
            if (ValidateIfEmptyOrNull(newPet.Species))
            {
                petExceptions.Add("Species is required.");
            }
            else if (!ValidateIfAlphabeticHyphenAndApostropheOnly(newPet.Species))
            {
                petExceptions.Add("Species must contain only alphabetic letters, hyphens(-), or apostrophes(').");
            }
            if (ValidateIfEmptyOrNull(newPet.Breed))
            {
                petExceptions.Add("Breed is required.");
            }
            else if (!ValidateIfAlphabeticHyphenAndApostropheOnly(newPet.Breed))
            {
                petExceptions.Add("Breed must contain only alphabetic letters, hyphens(-), or apostrophes(').");
            }
            if (ValidateIfEmptyOrNull(newPet.Fur))
            {
                petExceptions.Add("Fur Type is required.");
            }
            else if (!ValidateIfAlphabeticHyphenAndApostropheOnly(newPet.Fur))
            {
                petExceptions.Add("Fur Type must contain only alphabetic letters, hyphens(-), or apostrophes(').");
            }
            if (ValidateIfEmptyOrNull(newPet.OwnerName))
            {
                petExceptions.Add("Owner Name is required.");
            }
            else if (!ValidateIfAlphabeticHyphenAndApostropheOnly(newPet.OwnerName))
            {
                petExceptions.Add("Owner Name must contain only alphabetic letters, hyphens(-), or apostrophes(').");
            }
            if (ValidateIfEmptyOrNull(newPet.OwnerEmail))
            {
                petExceptions.Add("Owner Email is required.");
            }
            else if (!ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain(newPet.OwnerEmail))
            {
                petExceptions.Add("Owner Email must be formatted user@email.com.");
            }
            if (ValidateIfEmptyOrNull(newPet.OwnerPhone))
            {
                petExceptions.Add("Owner Phone is required.");
            }
            else if (!ValidatePhoneNumberIsTenDigits(newPet.OwnerPhone))
            {
                petExceptions.Add("Owner Phone number must be 10 digits e.g. (123) 456 - 7890.");
            }
            if (petExceptions.Count > 0)
            {
                _logger.LogInformation(" ", petExceptions);
                throw new BadRequestException(String.Join(" ", petExceptions));
            }
        }
        /// <summary>
        /// Validates if gender input is a valid input
        /// </summary>
        /// <param name="modelField">string input field</param>
        /// <returns>boolean, true if input is invalid</returns>

        public bool ValidateGender(string gender)
        {
            return Regex.IsMatch(gender, @"^(Male|Female|Other)$");
        }

        /// <summary>
        /// Validates if state input is a valid input
        /// </summary>
        /// <param name="modelField">string input field</param>
        /// <returns>boolean, true if input is invalid</returns>

        public bool ValidateState(string state)
        {
            return Regex.IsMatch(state, @"^(AL|AK|AS|AZ|AR|CA|CO|CT|DE|DC|FM|FL|GA|GU|HI|ID|IL|IN|IA|KS|KY|LA|ME|MH|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|MP|OH|OK|OR|PW|PA|PR|RI|SC|SD|TN|TX|UT|VT|VI|VA|WA|WV|WI|WY)$");
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
        ///Validates if user phone number is 10 numberical digits (PHONE NUMBER)
        /// </summary>
        /// <param name="phoneNumber">string phone unmber input field</param>
        /// <returns>boolean, true if the phone number is valid</returns>

        public bool ValidatePhoneNumberIsTenDigits(string phoneNumber)
        {
            var phoneNumberCheck = new Regex(@"^\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$");
            return phoneNumberCheck.IsMatch(phoneNumber);
        }

        /// <summary>
        /// Validates if an email has already been used.
        /// </summary>
        /// <param name="newPet">newPet to validate the email on</param>
        /// <returns>boolean, true if input is invalid, false if not.</returns>

        public async Task<bool> ValidateSingleEmail(Pet newPet)
        {
            var pets = await GetPetsAsync();
            var petEmailList = new List<string>();

            foreach (var pet in pets)
            {
                petEmailList.Add(pet.OwnerEmail);
            }

            if (petEmailList.Contains(newPet.OwnerEmail))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// validates if a string only contains alphabetic characters, hyphens, and apostrophes. Allows spaces. (FIRST AND LAST NAMES)
        /// </summary>
        /// <param name="modelField">string name input field</param>
        /// <returns>boolean, true if input matches regex</returns>
        public bool ValidateIfAlphabeticHyphenAndApostropheOnly(string modelField)
        {
            var nameCheck = new Regex(@"^[\p{L} \'\-]+$");
            return nameCheck.IsMatch(modelField);
        }

        /// <summary>
        /// Validates if user email address is formatted correctly (user@email.com) with alphanumeric username and alphatic domain (EMAIL)
        /// </summary>
        /// <param name="emailAddress">string email input field</param>
        /// <returns>boolean, true if the email is formatted correctly</returns>
        public bool ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain(string emailAddress)
        {
            var emailCheck = new Regex(@"^\w+@([a-z]+\.)+[a-z]+$");
            return emailCheck.IsMatch(emailAddress);
        }

        
    }
}

