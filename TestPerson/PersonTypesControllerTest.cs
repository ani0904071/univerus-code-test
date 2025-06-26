using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonApi.Data;
using PersonApi.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestPerson.CustomWebFactory;
using TestPerson.TestOrder;
using Xunit;


namespace TestPerson
{   
    //[assembly: TestCaseOrderer("TestPerson.TestOrder.PriorityOrderer", "TestPerson")]
    //[assembly: CollectionBehavior(DisableTestParallelization = true)]
    public class PersonTypesControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public PersonTypesControllerTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        }

        [Fact, TestPriority(2)]
        public async Task GetAllPersonTypes_ShouldReturnList()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<PersonAPIContext>();

                db.Database.EnsureCreated();
                db.Database.Migrate();
                Seeding.InitializeTestDB(db);
                
            }

            // Act
            var response = await _httpClient.GetAsync("/api/v1/persontypes");
            var result = await response.Content.ReadFromJsonAsync<List<PersonType>>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Should().HaveCount(4);

        }

        [Fact, TestPriority(1)]
        public async Task GetPersonTypeById_ShouldReturnCorrectPersonType()
        {
            // Arrange
            var response = await _httpClient.GetAsync("/api/v1/persontypes/2");

            // Act
            var personType = await response.Content.ReadFromJsonAsync<PersonType>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            personType.Should().NotBeNull();
            personType.Id.Should().Be(2);
            personType.Description.Should().Be("Teacher");

        }

        //[Fact]
        //public async Task CreatePersonType_ShouldReturnCreated()
        //{
        //    var newPersonType = new PersonType { Description = "Operator" };

        //    var response = await _httpClient.PostAsJsonAsync("/api/persontypes", newPersonType);
        //    var created = await response.Content.ReadFromJsonAsync<PersonType>();

        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        //    created.Should().NotBeNull();
        //    created.Description.Should().Be("Operator");
        //}
    }
}
