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
//    public class PetProviderValidationTests
//    {
//        public class PetProviderTests
//        {
//            private readonly PetFactory _factory = new();

//            private readonly Mock<IPetRepository> repositoryStub;
//            private readonly Mock<ILogger<PetProvider>> loggerStub;
//            private readonly IPetProvider petProvider;
//            private readonly Pet testPetSameEmail;
//            private readonly Pet testPetDiffEmail;

//            private readonly PetProvider provider;

//            private readonly Pet testPet;
//            private readonly List<Pet> testPets;
//            private readonly Pet validateValidPet;

//            public PetProviderTests()
//            {
//                // Set up initial testing tools that most/all tests will use
//                repositoryStub = new Mock<IPetRepository>();
//                loggerStub = new Mock<ILogger<PetProvider>>();
//                provider = new PetProvider(repositoryStub.Object, loggerStub.Object);

//                testPet = new Pet { Email = "abc@123.org" };
//                testPets = new List<Pet> { testPet, testPetSameEmail, testPetDiffEmail };

//                validateValidPet = new Pet { FirstName = "Firstname", LastName = "Lastname", SSN = "123-45-6789", Email = "user@domain.com", Age = 5, Height = 5, Weight = 200, Insurance = "Tricare", Gender = "Other", Street = "Sea Turle Way", City = "Waimea", State = "KS", Postal = "12345", };
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
//            /// Tests that if a field should have only alphabetic characters, apostrophes, or hypens returns true if contains apostrophe.
//            /// </summary>
//            [Fact]
//            public void ValidateIfAlphabeticHyphenAndApostropheOnly_ReturnsTrueWithApostrophe()
//            {
//                var given = "Timothy O'Niel";
//                var actual = provider.ValidateIfAlphabeticHyphenAndApostropheOnly(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests that if a field should have only alphabetic characters, apostrophes, or hypens returns true if contains hyphen.
//            /// </summary>
//            [Fact]
//            public void ValidateIfAlphabeticHyphenAndApostropheOnly_ReturnsTrueWithHyphen()
//            {
//                var given = "Beth-Anne Wallace";
//                var actual = provider.ValidateIfAlphabeticHyphenAndApostropheOnly(given);
//                actual.Should().BeTrue();
//            }

//            /// <summary>
//            /// Tests that if a field should have only alphabetic characters, apostrophes, or hypens returns false if contains numbers.
//            /// </summary>
//            [Fact]
//            public void ValidateIfAlphabeticHyphenAndApostropheOnly_ReturnsFalseIfNumeric()
//            {
//                var given = "1243450";
//                var actual = provider.ValidateIfAlphabeticHyphenAndApostropheOnly(given);
//                actual.Should().BeFalse();
//            }

//            /// <summary>
//            /// Tests that if a field should have only alphabetic characters, apostrophes, or hypens returns true if contains all valid characters.
//            /// </summary>
//            [Fact]
//            public void ValidateIfAlphabeticHyphenAndApostropheOnly_ReturnsTrueIfContainsAllValidCharacters()
//            {
//                var given = "Terry-Anne O'Brien";
//                var actual = provider.ValidateIfAlphabeticHyphenAndApostropheOnly(given);
//                actual.Should().BeTrue();
//            }
//            /// <summary>
//            /// Tests if an entered email is a valid email format. Returns true if username & domain is alphanumeric
//            /// </summary>
//            [Fact]
//            public void ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain_ReturnsTrueIfUsernameAndDomainValid()
//            {
//                var given = "user@domainofchoice.com";
//                var actual = provider.ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain(given);
//                Assert.True(actual);
//            }

//            /// <summary>
//            /// Tests if an entered email is a valid email format. Returns true if subdomain is valid
//            /// </summary>
//            [Fact]
//            public void ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain_ReturnsTrueWithValidSubDomain()
//            {
//                var given = "user@domainofchoice.subdomain.edu";
//                var actual = provider.ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain(given);
//                Assert.True(actual);
//            }

//            /// <summary>
//            /// Tests if an entered email is a valid email format. Returns false if username is invalid
//            /// </summary>
//            [Fact]
//            public void ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain_ReturnsFalseIfUsernameInvalid()
//            {
//                var given = "user)*&123@domainofchoice.net";
//                var actual = provider.ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain(given);
//                Assert.False(actual);
//            }

//            /// <summary>
//            /// Tests if an entered email is a valid email format. Returns false if domain is invalid
//            /// </summary>
//            [Fact]
//            public void ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain_ReturnsFalseIfDomainInvalid()
//            {
//                var given = "user@123domainofchoice.com";
//                var actual = provider.ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain(given);
//                Assert.False(actual);
//            }

//            /// <summary>
//            /// Tests if an entered email is a valid email format. Returns false if subdomain is invalid
//            /// </summary>
//            [Fact]
//            public void ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain_ReturnsFalseIfSubDomainInvalid()
//            {
//                var given = "username@domain.123subdomain.com";
//                var actual = provider.ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain(given);
//                Assert.False(actual);
//            }

//            /// <summary>
//            /// Tests if an entered email is a valid email format. Returns false if format is invalid
//            /// </summary>
//            [Fact]
//            public void ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain_ReturnsFalseIfInvalidFormat()
//            {
//                var given = "@userdomain.com";
//                var actual = provider.ValidateEmailIsAlphaNumericUserNameAndAlphabeticDomain(given);
//                Assert.False(actual);
//            }

//            /// <summary>
//            /// Tests if an entered zip code is only 5 or 9 digits. Returns true if zip is valid.
//            /// </summary>
//            [Fact]
//            public void ValidateZipCodeIsOnlyFiveOrNineDigits_ReturnsTrueIfValid()
//            {
//                var given = "12345";
//                var actual = provider.ValidateZipCodeIsOnlyFiveOrNineDigits(given);
//                Assert.True(actual);
//            }

//            /// <summary>
//            /// Tests if an entered zip code is only 5 or 9 digits. Returns false if zip is invalid.
//            /// </summary>
//            [Fact]
//            public void ValidateZipCodeIsOnlyFiveOrNineDigits_ReturnsFalseIfInvalid()
//            {
//                var given = "1234";
//                var actual = provider.ValidateZipCodeIsOnlyFiveOrNineDigits(given);
//                Assert.False(actual);
//            }

//            /// <summary>
//            /// Tests if an entered ssn folows format DDD-DD-DDDD. Returns false if ssn is invalid.
//            /// </summary>
//            [Fact]
//            public void ValidateSSNFormatIsValid_ReturnsFalseIfInvalid()
//            {
//                var given = "1234";
//                var actual = provider.ValidateSSNFormat(given);
//                Assert.False(actual);
//            }

//            /// <summary>
//            /// Tests if an entered ssn folows format DDD-DD-DDDD. Returns true if ssn is valid.
//            /// </summary>
//            [Fact]
//            public void ValidateSSNFormatIsValid_ReturnsTrueIfValid()
//            {
//                var given = "123-34-5678";
//                var actual = provider.ValidateSSNFormat(given);
//                Assert.True(actual);
//            }

//            [Fact]
//            public void ValidatePetInputFields_ReturnsTrueIfListContainsWhatItIsSupposedTo()
//            {   //The int is a placeholder only in order to Func to cooperate, return 0; is never (in theory) reached
//                Func<int> validPet = () => { provider.ValidatePetInputFields(validateValidPet); return 0; };
//                validPet.Should().NotThrow<BadRequestException>();
//            }

//            [Fact]
//            public void ValidatePetInputFields_ThrowsBadRequestExceptionIfInvalidFields()
//            {
//                validateValidPet.FirstName = "!123";
//                validateValidPet.LastName = "123?";
//                validateValidPet.SSN = "1234567890";
//                validateValidPet.Email = "Email*email.com";
//                validateValidPet.Street = "!@?";
//                validateValidPet.City = "!@?";
//                validateValidPet.Postal = "!@?";
//                validateValidPet.Age = 12;
//                validateValidPet.Height = 12;
//                validateValidPet.Weight = 12;
//                validateValidPet.Insurance = "";
//                validateValidPet.Gender = "Nonbinary";
//                Func<int> validPet = () => { provider.ValidatePetInputFields(validateValidPet); return 0; };
//                validPet.Should().Throw<BadRequestException>();
//            }

//            [Fact]
//            public void ValidatePetInputFields_ThrowsBadRequestExceptionIfThereAreEmptyRequiredFields()
//            {
//                validateValidPet.FirstName = string.Empty;
//                validateValidPet.LastName = string.Empty;
//                validateValidPet.SSN = string.Empty;
//                validateValidPet.Email = string.Empty;
//                validateValidPet.Street = string.Empty;
//                validateValidPet.City = string.Empty;
//                validateValidPet.Postal = string.Empty;
//                validateValidPet.Age = 0;
//                validateValidPet.Height = 0;
//                validateValidPet.Weight = 0;
//                validateValidPet.Insurance = string.Empty;
//                validateValidPet.Gender = string.Empty;
//                Func<int> validPet = () => { provider.ValidatePetInputFields(validateValidPet); return 0; };
//                validPet.Should().Throw<BadRequestException>();
//            }
//        }
//    }
//}
