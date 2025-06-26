using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonApi.Data;
using PersonApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TestPerson.CustomWebFactory;
using TestPerson.TestOrder;

[assembly: TestCaseOrderer("TestPerson.TestOrder.PriorityOrderer", "TestPerson")]
[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace TestPerson
{
    public class PersonsControllerTest : IClassFixture<CustomWebApplicationFactory<Program>> 
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private HttpClient _httpClient;

        public PersonsControllerTest(CustomWebApplicationFactory<Program> factory) { 
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        }

        [Fact, TestPriority(1)]
        public async Task GetAllPersons_ShouldReturnAllPersons() {


            using (var scope = _factory.Services.CreateScope()) 
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<PersonAPIContext>();

                db.Database.EnsureCreated();
                db.Database.Migrate();
                Seeding.InitializeTestDB(db);
            }

            var response = await _httpClient.GetAsync("/api/v1/persons");
            var result = await response.Content.ReadFromJsonAsync<List<Person>>();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().HaveCount(4);

        }

        [Fact, TestPriority(2)]
        public async Task GetPersonById_ShouldReturnCorrectPerson()
        {
            // Arrange
            var response = await _httpClient.GetAsync("/api/v1/persons/1");

            // Act
            var person = await response.Content.ReadFromJsonAsync<Person>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            person.Should().NotBeNull();
            person.Id.Should().Be(1);
            person.Name.Should().Be("Alice");
            person.PersonTypeId.Should().Be(1);
            person.Age.Should().Be(22);
            person.PersonType!.Description.Should().Be("Student");
        }

        [Fact, TestPriority(3)]
        public async Task UpdatePerson_ShouldUpdateSuccessfully()
        {
            // Arrange
            var personToUpdate = new Person
            {
                Id = 2,
                Name = "Updated Name",
                Age = 40,
                PersonTypeId = 1
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync("/api/v1/persons/2", personToUpdate);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var updatedPerson = await _httpClient.GetFromJsonAsync<Person>("/api/v1/persons/2");
            // Assert ID remains the same
            updatedPerson.Should().NotBeNull();
            updatedPerson.Id.Should().Be(personToUpdate.Id);
            updatedPerson.Name.Should().Be(personToUpdate.Name);
            updatedPerson.Age.Should().Be(personToUpdate.Age);
            updatedPerson.PersonTypeId.Should().Be(personToUpdate.PersonTypeId);

        }

        [Fact, TestPriority(4)]
        public async Task DeletePerson_ShouldRemovePerson()
        {
            // Arrange
            var idToDelete = 2;

            // Act
            var deleteResponse = await _httpClient.DeleteAsync($"/api/v1/persons/{idToDelete}");

            // Assert
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            var getResponse = await _httpClient.GetAsync($"/api/v1/persons/{idToDelete}");
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        


    }
}
