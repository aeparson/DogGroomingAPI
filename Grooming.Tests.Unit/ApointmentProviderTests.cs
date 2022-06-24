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
//    public class AppointmentProviderTests
//    {
//        private readonly AppointmentFactory _factory = new();

//        private readonly Mock<IAppointmentRepository> repositoryStub;
//        private readonly Mock<IAppointmentRepository> patrepositoryStub;
//        private readonly Mock<ILogger<AppointmentProvider>> loggerStub;

//        private readonly AppointmentProvider provider;

//        private readonly Appointment testAppointment;
//        private readonly Appointment nullAppointment;
//        private readonly List<Appointment> testAppointments;
//        private readonly Appointment validateValidAppointment;
//        private readonly Appointment validateInvalidAppointment;

//        public AppointmentProviderTests()
//        {
//            repositoryStub = new Mock<IAppointmentRepository>();
//            patrepositoryStub = new Mock<IAppointmentRepository>();
//            loggerStub = new Mock<ILogger<AppointmentProvider>>();
//            provider = new AppointmentProvider(repositoryStub.Object, loggerStub.Object);


//            testAppointment = _factory.CreateRandomAppointment(5);
//            repositoryStub.Setup(repo => repo.GetAppointmentByIdAsync(1))
//                .ReturnsAsync(testAppointment);


//            testAppointments = _factory.GenerateRandomAppointments(5);
//            repositoryStub.Setup(repo => repo.GetAllAppointmentsAsync())
//                .ReturnsAsync(testAppointments);
//            nullAppointment = new Appointment { };

//            validateValidAppointment = new Appointment { Id = 1, PetId = 1, Notes = "notes", VisitCode = "A1A 1A1", Provider = "Provider", BillingCode = "111.111.111-11", ICD10 = "A11", TotalCost = 100, Copay = 10, ChiefComplaint = "Complaint", Pulse = 100, Diastolic = 70, Systolic = 100, Date = "2020-09-09" };
//            validateInvalidAppointment = new Appointment { Id = 1, PetId = 1, Notes = " ", VisitCode = "A1A1A1", Provider = " ", BillingCode = "11111111111", ICD10 = "111", TotalCost = 100, Copay = 10, ChiefComplaint = " ", Pulse = 100, Diastolic = 70, Systolic = 100, Date = "2020/09/09" };
//        }

//        /// <summary>
//        /// Tests that a call for all appointments returns all appointments.
//        /// </summary>
//        /// <returns>list of all appointments</returns>
//        [Fact]
//        public async Task GetAllAppointmentsAsync_ReturnsAllAppointments()
//        {
//            // Act
//            var result = await provider.GetAllAppointmentsAsync();

//            // Assert
//            result.Should().BeEquivalentTo(testAppointments);
//        }

//        /// <summary>
//        /// Tests that a call for existing appointment returns that appointment.
//        /// </summary>
//        /// <returns>appointment with matching id</returns>
//        [Fact]
//        public async Task GetAppointmentsByIdAsync_WithExistingAppointment_ReturnsExpectedAppointment()
//        {
//            // Act
//            var result = await provider.GetAppointmentByIdAsync(1);

//            // Assert
//            result.Should().BeEquivalentTo(
//                testAppointment,
//                options => options.ComparingByMembers<Appointment>());
//        }


//        /// <summary>
//        /// Tests that a call for a non-existent appointment returns error.
//        /// </summary>
//        /// <returns>not found exception</returns>
//        [Fact]
//        public async Task GetAppointmentsByIdAsync_WithNonExistingAppointment_ReturnsNotFoundException()
//        {
//            // Act
//            Func<Task> nonExistingAppointment = async () => { await provider.GetAppointmentByIdAsync(99999999); };

//            // Assert 
//            await nonExistingAppointment.Should().ThrowAsync<NotFoundException>();
//        }

//        /// <summary>
//        /// tests what happens when you try to update a appointment with valid information
//        /// </summary>
//        /// <returns>no exception</returns>
//        [Fact]
//        public async void UpdateAppointmentAsync_WithValidAppointment_ShouldNotThrow()
//        {

//            Func<Task> result = async () => { await provider.UpdateAppointmentByIdAsync(validateValidAppointment, 1); };
//            await result.Should().NotThrowAsync<Exception>();
//        }

//        /// <summary>
//        /// tests what happens when you try to update a appointment with invalid information
//        /// </summary>
//        /// <returns>bad request exception</returns>
//        [Fact]
//        public async void UpdateAppointmentAsync_WithInvalidAppointment_ReturnsBadRequestException()
//        {

//            Func<Task> result = async () => { await provider.UpdateAppointmentByIdAsync(validateInvalidAppointment, 1); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        /// <summary>
//        /// tests what happens when you try to update a appointment with null information
//        /// </summary>
//        /// <returns>bad request exception</returns>
//        [Fact]
//        public async void UpdateAppointmentAsync_WithNullAppointment_ReturnsBadRequestException()
//        {

//            Func<Task> result = async () => { await provider.UpdateAppointmentByIdAsync(nullAppointment, 1); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        // <summary>
//        /// tests what happens when you try to update an appointment with no database
//        /// </summary>
//        /// <returns>service unavailable exception</returns>
//        [Fact]
//        public async void UpdateAppointmentAsync_WithNoDatabase_ReturnsServiceUnavailableException()
//        {
//            //Arrange
//            repositoryStub.Setup(repo => repo.UpdateAppointmentByIdAsync(testAppointment, 1)).Throws(new ServiceUnavailableException("There was a problem connecting to the database."));
//            Func<Task> result = async () => { await provider.UpdateAppointmentByIdAsync(testAppointment, 1); };

//            await result.Should().ThrowAsync<ServiceUnavailableException>();
//        }

//        /// <summary>
//        /// tests what happens when you try to create a appointment with invalid information
//        /// </summary>
//        /// <returns>bad request exception</returns>
//        [Fact]
//        public async void CreateAppointmentAsync_WithInvalidAppointment_ReturnsBadRequestException()
//        {

//            Func<Task> result = async () => { await provider.CreateAppointmentAsync(validateInvalidAppointment); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        /// <summary>
//        /// tests what happens when you try to create a appointment with valid information
//        /// </summary>
//        /// <returns>no exception</returns>
//        [Fact]
//        public async void CreateAppointmentAsync_WithValidAppointment_ShouldNotThrow()
//        {

//            Func<Task> result = async () => { await provider.CreateAppointmentAsync(testAppointment); };
//            await result.Should().NotThrowAsync<Exception>();
//        }

//        /// <summary>
//        /// tests what happens when you try to create a appointment with null information
//        /// </summary>
//        /// <returns>bad request exception</returns>
//        [Fact]
//        public async void CreateAppointmentAsync_WithNullAppointment_ReturnsBadRequestException()
//        {

//            Func<Task> result = async () => { await provider.CreateAppointmentAsync(nullAppointment); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        // <summary>
//        /// tests what happens when you try to create an appointment with no database
//        /// </summary>
//        /// <returns>service unavailable exception</returns>
//        [Fact]
//        public async void CreateAppointmentAsync_WithNoDatabase_ReturnsServiceUnavailableException()
//        {
//            //Arrange
//            repositoryStub.Setup(repo => repo.CreateAppointmentAsync(testAppointment)).Throws(new ServiceUnavailableException("There was a problem connecting to the database."));
//            Func<Task> result = async () => { await provider.CreateAppointmentAsync(testAppointment); };

//            await result.Should().ThrowAsync<ServiceUnavailableException>();
//        }
//    }
//}
