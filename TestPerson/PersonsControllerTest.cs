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



        [Fact]
        public async Task GetPerons_PersonExists() {


            using (var scope = _factory.Services.CreateScope()) 
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<PersonAPIContext>();

                db.Database.EnsureCreated();
                db.Database.Migrate();
                Seeding.InitializeTestDB(db);
            }

            var response = await _httpClient.GetAsync("/api/persons");
            var result = await response.Content.ReadFromJsonAsync<List<Person>>();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().HaveCount(4);

        
        }
    }
}
