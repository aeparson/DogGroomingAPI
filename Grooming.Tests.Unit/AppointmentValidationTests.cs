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
//using Grooming.Providers.Interfaces;

//namespace Grooming.Tests.Unit
//{
//    public class AppointmentProviderValidationTests
//    {
//        public class AppointmentProviderTests
//        {
//            private readonly AppointmentFactory _factory = new();

//            private readonly Mock<IAppointmentRepository> repositoryStub;
//            private readonly Mock<ILogger<AppointmentProvider>> loggerStub;
//            private readonly IAppointmentProvider petProvider;

//            private readonly AppointmentProvider provider;

//            private readonly Appointment testAppointment;
//            private readonly List<Appointment> testAppointments;
//            private readonly Appointment validateValidAppointment;

//            public AppointmentProviderTests()
//            {
//                // Set up initial testing tools that most/all tests will use
//                repositoryStub = new Mock<IAppointmentRepository>();
//                loggerStub = new Mock<ILogger<AppointmentProvider>>();
//                provider = new AppointmentProvider(repositoryStub.Object, loggerStub.Object);

//                testAppointments = new List<Appointment> { testAppointment };

//                validateValidAppointment = new Appointment { PetId = 1, Notes = "Notes", VisitCode = "H7J 8W2", Provider = "Tricare", BillingCode = "111.111.111-11", ICD10 = "A22", TotalCost = 100, Copay = 20, ChiefComplaint = "Complaint", Pulse = 100, Diastolic = 100, Systolic = 100, Date = "2020-09-09" };
//            }

//            /// <summary>
//            /// Tests that if a field is empty or null, returns true if empty.
//            /// </summary>
//            [Fact]
//            public void ValidiateIfEmptyOrNull_ReturnsTrueIfEmpty()
//            {
//                var given = string.Empty;
//                var actual = provider.ValidateIfEmptyOrNull(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests that if a field is empty or null, returns true if null.
//            /// </summary>
//            [Fact]
//            public void ValidateIfEmptyOrNull_ReturnsTrueIfNull()
//            {
//                string given = null;
//                var actual = provider.ValidateIfEmptyOrNull(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests that if a field is empty or null, returns true if only spaces are entered.
//            /// </summary>
//            [Fact]
//            public void ValidateIfEmptyOrNull_ReturnsTrueIfOnlySpaces()
//            {
//                var given = "   ";
//                var actual = provider.ValidateIfEmptyOrNull(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests that if a field is empty or null, returns false if not empty/null.
//            /// </summary>
//            [Fact]
//            public void ValidateIfEmptyOrNull_ReturnsFalseIfNotEmptyOrNull()
//            {
//                var given = "This is my test string with text, 123456789, )*&%@)#%";
//                var actual = provider.ValidateIfEmptyOrNull(given);
//                actual.Should().BeFalse();
//            }

//            /// <summary>
//            /// Tests that if a field is empty or null, returns true if contains only white space.
//            /// </summary>
//            [Fact]
//            public void ValidateIfEmptyOrNull_ReturnsTrueIfContainsWhitespace()
//            {
//                var given = "   ";
//                var actual = provider.ValidateIfEmptyOrNull(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests if entered date matches required format, returns true if date is valid.
//            /// </summary>
//            [Fact]
//            public void ValidateAppointmentDateFormat_ReturnsTrueIfMatches()
//            {
//                var given = "2020-09-09";
//                var actual = provider.ValidateAppointmentDateFormat(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests if entered date matches required format, returns false if date is invalid.
//            /// </summary>
//            [Fact]
//            public void ValidateAppointmentDateFormat_ReturnsFalseIfDoesNotMatch()
//            {
//                var given = "20200909";
//                var actual = provider.ValidateAppointmentDateFormat(given);
//                actual.Should().BeFalse();
//            }

//            /// <summary>
//            /// Tests if entered totalCost or Copay matches required format, returns false if totalCost or Copay is invalid.
//            /// </summary>
//            [Fact]
//            public void ValidateMoneyFormat_ReturnsTrueIfMatches()
//            {
//                var given = 200;
//                var actual = provider.ValidateMoneyFormat(given);
//                actual.Should().BeFalse();
//            }

//            /// <summary>
//            /// Tests if entered visitCode matches required format, returns true if visitCode is valid.
//            /// </summary>
//            [Fact]
//            public void ValidateVisitCodeFormat_ReturnsTrueIfMatches()
//            {
//                var given = "A1A 1A1";
//                var actual = provider.ValidateVisitCodeFormat(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests if entered visitCode matches required format, returns false if visitCode is invalid.
//            /// </summary>
//            [Fact]
//            public void ValidateVisitCodeFormat_ReturnsFalseIfDoesNotMatch()
//            {
//                var given = "100A";
//                var actual = provider.ValidateVisitCodeFormat(given);
//                actual.Should().BeFalse();
//            }

//            /// <summary>
//            /// Tests if entered Billing Code matches required format, returns true if Billing Code is valid.
//            /// </summary>
//            [Fact]
//            public void ValidateBillingCodeFormat_ReturnsTrueIfMatches()
//            {
//                var given = "111.111.111-11";
//                var actual = provider.ValidateBillingCodeFormat(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests if entered Billing Code matches required format, returns false if Billing Code is invalid.
//            /// </summary>
//            [Fact]
//            public void ValidateBillingCodeFormat_ReturnsFalseIfDoesNotMatch()
//            {
//                var given = "100A";
//                var actual = provider.ValidateBillingCodeFormat(given);
//                actual.Should().BeFalse();
//            }

//            /// <summary>
//            /// Tests if entered ICD10 Code matches required format, returns true if ICD10 Code is valid.
//            /// </summary>
//            [Fact]
//            public void ValidateICD10Format_ReturnsTrueIfMatches()
//            {
//                var given = "A22";
//                var actual = provider.ValidateICD10Format(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests if entered ICD10 Code matches required format, returns false if ICD10 Code is invalid.
//            /// </summary>
//            [Fact]
//            public void ValidateICD10Format_ReturnsFalseIfDoesNotMatch()
//            {
//                var given = "100A";
//                var actual = provider.ValidateICD10Format(given);
//                actual.Should().BeFalse();
//            }

//            /// <summary>
//            /// Tests if entered ICD10 Code matches required format, returns true if ICD10 Code is valid.
//            /// </summary>
//            [Fact]
//            public void ValidateNumericOnly_ReturnsTrueIfMatches()
//            {
//                var given = 22;
//                var actual = provider.ValidateNumericOnly(given);
//                actual.Should().BeTrue();
//            }

//            [Fact]
//            public void ValidateAppointmentInputFields_ThrowsBadRequestExceptionIfInvalidFields()
//            {
//                validateValidAppointment.PetId = 0;
//                validateValidAppointment.Notes = " ";
//                validateValidAppointment.VisitCode = "1234567890";
//                validateValidAppointment.Provider = " ";
//                validateValidAppointment.BillingCode = "123";
//                validateValidAppointment.ICD10 = "123";
//                validateValidAppointment.TotalCost = 0;
//                validateValidAppointment.Copay = 0;
//                validateValidAppointment.ChiefComplaint = " ";
//                validateValidAppointment.Pulse = 0;
//                validateValidAppointment.Systolic = 0;
//                validateValidAppointment.Diastolic = 0;
//                validateValidAppointment.Date = "Date";
//                Func<int> validAppointment = () => { provider.ValidateAppointmentInputFields(validateValidAppointment); return 0; };
//                validAppointment.Should().Throw<BadRequestException>();
//            }

//            [Fact]
//            public void ValidateAppointmentInputFields_ThrowsBadRequestExceptionIfThereAreEmptyRequiredFields()
//            {
//                validateValidAppointment.PetId = 0;
//                validateValidAppointment.Notes = string.Empty;
//                validateValidAppointment.VisitCode = string.Empty;
//                validateValidAppointment.Provider = string.Empty;
//                validateValidAppointment.BillingCode = string.Empty;
//                validateValidAppointment.ICD10 = string.Empty;
//                validateValidAppointment.TotalCost = 0;
//                validateValidAppointment.Copay = 0;
//                validateValidAppointment.ChiefComplaint = string.Empty;
//                validateValidAppointment.Pulse = 0;
//                validateValidAppointment.Systolic = 0;
//                validateValidAppointment.Diastolic = 0;
//                validateValidAppointment.Date = string.Empty;
//                Func<int> validAppointment = () => { provider.ValidateAppointmentInputFields(validateValidAppointment); return 0; };
//                validAppointment.Should().Throw<BadRequestException>();
//            }

            
//        }
//    }
//}
