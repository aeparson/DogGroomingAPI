//using Grooming.Data.Interfaces;
//using Grooming.Data.Model;
//using Grooming.Data.SeedData;
//using Grooming.Providers.Providers;
//using Grooming.Utilities.HttpResponseExceptions;
//using FluentAssertions;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;
//using System.Net;
//using Grooming.API.Controllers;

//namespace Grooming.Tests.Unit
//{
//    public class PetProviderTests
//    {
//        private readonly PetFactory _factory = new();

//        private readonly Mock<IPetRepository> repositoryStub;
//        private readonly Mock<ILogger<PetProvider>> loggerStub;

//        private readonly PetProvider provider;

//        private readonly Pet testPet;
//        private readonly Pet newPet;
//        private readonly Pet nullPet;
//        private readonly List<Pet> testPets;
//        private readonly Pet validateValidPet;
//        private readonly Pet validateInvalidPet;


//        public PetProviderTests()
//        {
//            // Set up initial testing tools that most/all tests will use
//            repositoryStub = new Mock<IPetRepository>();
//            loggerStub = new Mock<ILogger<PetProvider>>();
//            provider = new PetProvider(repositoryStub.Object, loggerStub.Object);


//            newPet = _factory.CreateRandomPet(1);
//            repositoryStub.Setup(repo => repo.CreatePetAsync(newPet))
//                .ReturnsAsync(newPet);

//            testPet = _factory.CreateRandomPet(1);
//            repositoryStub.Setup(repo => repo.GetPetByIdAsync(1))
//                .ReturnsAsync(testPet);

//            testPets = _factory.GenerateRandomPets(4);
//            repositoryStub.Setup(repo => repo.GetPetsAsync())
//                .ReturnsAsync(testPets);
//            nullPet = new Pet { };

//            validateValidPet = new Pet { FirstName = "Firstname", LastName = "Lastname", SSN = "123-45-6789", Email = "user@domain.com", Age = 5, Height = 5, Weight = 200, Insurance = "Tricare", Gender = "Other", Street = "Sea Turle Way", City = "Waimea", State = "KS", Postal = "12345", };
//            validateInvalidPet = new Pet { FirstName = "1234", LastName = "5678", SSN = "123456789", Email = "userdomain.com", Age = 5, Height = 5, Weight = 200, Insurance = "", Gender = "Other", Street = "Sea Turle Way", City = "Waimea", State = "KS", Postal = "12345", };
//        }

//        /// <summary>
//        /// Tests that a call for existing pet returns that pet.
//        /// </summary>
//        /// <returns>pet with matching id</returns>
//        [Fact]
//        public async Task GetPetsByIdAsync_WithExistingPet_ReturnsExpectedPet()
//        {
//            // Act
//            var result = await provider.GetPetByIdAsync(1);

//            // Assert
//            result.Should().BeEquivalentTo(
//                testPet,
//                options => options.ComparingByMembers<Pet>());
//        }


//        /// <summary>
//        /// Tests that a call for a non-existent pet returns error.
//        /// </summary>
//        /// <returns>not found exception</returns>
//        [Fact]
//        public async Task GetPetsByIdAsync_WithNonExistingPet_ReturnsNotFoundException()
//        {
//            // Act
//            Func<Task> nonExistingPet = async () => { await provider.GetPetByIdAsync(99999999); };

//            // Assert 
//            await nonExistingPet.Should().ThrowAsync<NotFoundException>();
//        }

//        /// <summary>
//        /// Tests that a call for all pets returns all pets.
//        /// </summary>
//        /// <returns>list of all pets</returns>
//        [Fact]
//        public async Task GetAllPetsAsync_ReturnsAllPets()
//        {
//            // Act
//            var result = await provider.GetPetsAsync();

//            // Assert
//            result.Should().BeEquivalentTo(testPets);
//        }

//        /// <summary>
//        /// Tests that an invalid pet id returns a NotFoundException.
//        /// </summary>
//        /// <returns>not found exception</returns>
//        [Fact]
//        public async Task DeletePetByIdAsync_WithInvalidId_ReturnsNotFoundException()
//        {
//            // Act
//            Func<Task> result = async () => { await provider.DeletePetByIdAsync(99999999); };
//            // Assert
//            await result.Should().ThrowAsync<NotFoundException>();
//        }

//        /// <summary>
//        /// Tests that a valid created pet returns a saved pet
//        /// </summary>
//        [Fact]
//        public async Task CreatePetAsync_WithValidPet_ReturnsExpectedPet()
//        {
//            //Arrange

//            //Act
//            var result = await provider.CreatePetAsync(newPet);
//            //Assert
//            result.Should().BeEquivalentTo(newPet, options => options.ComparingByMembers<Pet>());
//        }

//        /// <summary>
//        /// tests what happens when you try to create a pet with invalid information
//        /// </summary>
//        /// <returns>bad request exception</returns>
//        [Fact]
//        public async void CreatePetAsync_WithInvalidPet_ReturnsBadRequestException()
//        {
            
//        Func<Task> result = async () => { await provider.CreatePetAsync(validateInvalidPet); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        /// <summary>
//        /// tests what happens when you try to create a pet with valid information
//        /// </summary>
//        /// <returns>no exception</returns>
//        [Fact]
//        public async void CreatePetAsync_WithValidPet_ShouldNotThrow()
//        {

//            Func<Task> result = async () => { await provider.CreatePetAsync(testPet); };
//            await result.Should().NotThrowAsync<Exception>();
//        }

//        /// <summary>
//        /// tests what happens when you try to create a pet with null information
//        /// </summary>
//        /// <returns>bad request exception</returns>
//        [Fact]
//        public async void CreatePetAsync_WithNullPet_ReturnsBadRequestException()
//        {

//            Func<Task> result = async () => { await provider.CreatePetAsync(nullPet); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        // <summary>
//        /// tests what happens when you try to create a pet with no database
//        /// </summary>
//        /// <returns>service unavailable exception</returns>
//        [Fact]
//        public async void CreatePetAsync_WithNoDatabase_ReturnsServiceUnavailableException()
//        {
//            //Arrange
//            repositoryStub.Setup(repo => repo.CreatePetAsync(newPet)).Throws(new ServiceUnavailableException("There was a problem connecting to the database."));
//            Func<Task> result = async () => { await provider.CreatePetAsync(newPet); };

//            await result.Should().ThrowAsync<ServiceUnavailableException>();
//        }

//        /// <summary>
//        /// tests what happens when you try to update a pet with valid information
//        /// </summary>
//        /// <returns>no exception</returns>
//        [Fact]
//        public async void UpdatePetAsync_WithValidPet_ShouldNotThrow()
//        {

//            Func<Task> result = async () => { await provider.UpdatePetByIdAsync(testPet, 1); };
//            await result.Should().NotThrowAsync<Exception>();
//        }

//        /// <summary>
//        /// tests what happens when you try to update a pet with invalid information
//        /// </summary>
//        /// <returns>bad request exception</returns>
//        [Fact]
//        public async void UpdatePetAsync_WithInvalidPet_ReturnsBadRequestException()
//        {

//            Func<Task> result = async () => { await provider.UpdatePetByIdAsync(validateInvalidPet, 1); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        /// <summary>
//        /// tests what happens when you try to update a pet with null information
//        /// </summary>
//        /// <returns>bad request exception</returns>
//        [Fact]
//        public async void UpdatePetAsync_WithNullPet_ReturnsBadRequestException()
//        {

//            Func<Task> result = async () => { await provider.UpdatePetByIdAsync(nullPet, 1); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        // <summary>
//        /// tests what happens when you try to create a pet with no database
//        /// </summary>
//        /// <returns>service unavailable exception</returns>
//        [Fact]
//        public async void UpdatePetAsync_WithNoDatabase_ReturnsServiceUnavailableException()
//        {
//            //Arrange
//            repositoryStub.Setup(repo => repo.UpdatePetByIdAsync(newPet, 1)).Throws(new ServiceUnavailableException("There was a problem connecting to the database."));
//            Func<Task> result = async () => { await provider.UpdatePetByIdAsync(newPet, 1); };

//            await result.Should().ThrowAsync<ServiceUnavailableException>();
//        }
//    }
//}