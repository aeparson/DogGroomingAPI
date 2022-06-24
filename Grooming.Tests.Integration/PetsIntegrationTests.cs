//using Grooming.DTOs;
//using Grooming.Tests.Integration.Utilities;
//using Microsoft.AspNetCore.Mvc.Testing;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using Xunit;

//namespace Grooming.Test.Integration
//{
//    [Collection("Sequential")]
//    public class ProductIntegrationTests : IClassFixture<CustomWebApplicationFactory>
//    {
//        private readonly HttpClient _client;//Postman in code

//        public ProductIntegrationTests(CustomWebApplicationFactory factory)
//        {
//            _client = factory.CreateClient(new WebApplicationFactoryClientOptions//initialize the above
//            {
//                AllowAutoRedirect = false
//            });
//        }

//        /// <summary>
//        /// Tests that when a get request to the pets endpoint is initiated, a list of all pets returns.
//        /// </summary>
//        /// <returns>product from database</returns>
//        [Fact]
//        public async Task GetPets_Returns200()
//        {
//            var response = await _client.GetAsync("/pets");
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//            var content = await response.Content.ReadAsAsync<List<PetDTO>>();
//            Assert.Equal(21, content.Count);
//        }

//        /// <summary>
//        /// Tests that when given a valid pet id, a pet is returned.
//        /// </summary>
//        /// <returns>product from database</returns>
//        [Fact]
//        public async Task GetPetById_GivenByExistingId_Returns200()
//        {
//            var response = await _client.GetAsync("/pets/1");
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//            var content = await response.Content.ReadAsAsync<PetDTO>();
//            Assert.Equal(1, content.Id);
//        }


//        /// <summary>
//        /// Tests that when given a valid pet, a created and saved pet is returned.
//        /// </summary>
//        /// <returns>Saved pet from database</returns>
//        [Fact]
//        public async Task CreatePetAsync__WithPet_ReturnsCreatedPet()
//        {
//            var testPet = new PetDTO
//            {
//                Id = 100,
//                FirstName = "First",
//                LastName = "Last",
//                SSN = "123-45-6789",
//                Email = "test@test.com",
//                Street = "123 Street",
//                City = "City",
//                State = "KS",
//                Postal = "12345",
//                Age = 23,
//                Height = 100,
//                Weight = 400,
//                Insurance = "Tricare",
//                Gender = "Other"

//            };
//            var newPetDTO = JsonContent.Create(testPet);
//            var response = await _client.PostAsync("/pets", newPetDTO);
//            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//        }

//        /// <summary>
//        /// Tests that when given an invalid petId, a 404 response is returned.
//        /// </summary>
//        /// <returns>Saved pet from database</returns>
//        [Fact]
//        public async Task DeletePetAsync_WithNonexistantId_Returns404()
//        {
//            var response = await _client.DeleteAsync("/products/999789");
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//        }

//        [Fact]
//        public async Task DeletePetAsync_WithDeleteablePet_Returns200()
//        {
//            var testPet = new PetDTO
//            {
//                Id = 100,
//                FirstName = "First",
//                LastName = "Last",
//                SSN = "123-45-6789",
//                Email = "test@test.com",
//                Street = "123 Street",
//                City = "City",
//                State = "KS",
//                Postal = "12345",
//                Age = 23,
//                Height = 100,
//                Weight = 400,
//                Insurance = "Tricare",
//                Gender = "Other"

//            };
//            var newPetDTO = JsonContent.Create(testPet);
//            await _client.PostAsync("/pets", newPetDTO);
//            // Make sure pet was created
//            var createdPet = await (await _client.GetAsync("/pets/100")).Content.ReadAsAsync<PetDTO>();
//            Assert.Equal("First", createdPet.FirstName);
//            // Delete pet
//            var response = await _client.DeleteAsync("/pets/100");
//            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//            // Make sure pet was deleted
//            var petDeleted = await _client.GetAsync("/pets/100");
//            Assert.Equal(HttpStatusCode.NotFound, petDeleted.StatusCode);
//        }

//        /// <summary>
//        /// Tests that when given a valid pet, a created and saved pet is returned.
//        /// </summary>
//        /// <returns>Saved pet from database</returns>
//        //[Fact]
//        //public async Task UpdatePetAsync__WithPet_ReturnsUpdatedPet()
//        //{
//        //    var testPet = new PetDTO
//        //    {
//        //        Id = 100,
//        //        FirstName = "First",
//        //        LastName = "Last",
//        //        SSN = "123-45-6789",
//        //        Email = "test@test.com",
//        //        Street = "123 Street",
//        //        City = "City",
//        //        State = "KS",
//        //        Postal = "12345",
//        //        Age = 23,
//        //        Height = 100,
//        //        Weight = 400,
//        //        Insurance = "Tricare",
//        //        Gender = "Other"

//        //    };

//        //    var updatePet = new PetDTO
//        //    {
//        //        Id = 100,
//        //        FirstName = "Update",
//        //        LastName = "Name",
//        //        SSN = "123-45-6789",
//        //        Email = "update@test.com",
//        //        Street = "123 Street",
//        //        City = "City",
//        //        State = "KS",
//        //        Postal = "12345",
//        //        Age = 23,
//        //        Height = 100,
//        //        Weight = 400,
//        //        Insurance = "Tricare",
//        //        Gender = "Other"

//        //    };
//        //    var newPetDTO = JsonContent.Create(testPet);
//        //    var response = await _client.PostAsync("/pets", newPetDTO);
//        //    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//        //}

//    }
//}
