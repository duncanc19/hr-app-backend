using System.Runtime.ExceptionServices;
using System;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using HRApp.API.Controllers;
using HRApp.API.Models;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;   

namespace tests
{
    public class VisitorAcceptanceTests
    {
     
        [Fact]
        public async Task CreateANewVisit()
        {
            // Arrange
            var apiClient = new HttpClient();
            var newVisit = JToken.FromObject(new { FirstName = "John", Surname = "Doe", Company = "Disneyland", Role =  "Marketing", Telephone = "07716354633", Email = "johnd@disneyland.com", EmployeeEmail = "jfawl@skillsforcare.org", Appointment = new DateTime(2020,12,01,12,30,00)});

            // Serialize our concrete class into a JSON String
            var stringVisit = await Task.Run(() => JsonConvert.SerializeObject(newVisit));

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringVisit, Encoding.UTF8, "application/json");
            var expectedResponse = new Dictionary<string, string>(){
                {"firstName", "John"},
                {"surname", "Doe"},
                {"company", "Disneyland"},
                {"role", "Marketing"},
                {"telephone", "07716354633"},
                {"email", "johnd@disneyland.com"},
                {"employeeEmail", "jfawl@skillsforcare.org"}
                // {"appointment", $"{new DateTime(2020,12,01,12,30,00).ToString("dd/MM/yyyy HH:mm:ss")}"}
            };

            // Act
            var apiResponse = await apiClient.PostAsync($"http://localhost:5003/api/visitor", httpContent);
            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());
            
            // Assert
            foreach (var field in expectedResponse)
            {
                Assert.Equal(expectedResponse[field.Key], jsonResponse["visit"][field.Key]);
            }
        }

        [Fact]
          public async Task GetVisitEndpointWithId()
        {
            // Arrange
            var apiClient = new HttpClient();

            
            var visitId = "0aa862e4-68ee-45ed-873e-ee635f0b47e5";
            var expectedResponse = new Dictionary<string, string>(){
                {"firstName", "John"},
                {"surname", "Doe"},
                {"company", "Disneyland"},
                {"role", "Marketing"},
                {"telephone", "07716354633"},
                {"email", "johnd@disneyland.com"},
                {"employeeEmail", "jfawl@skillsforcare.org"}
                // {"appointment", $"{new DateTime(2020,12,01,12,30,00).ToString("dd/MM/yyyy HH:mm:ss")}"}
            };

            // Act
            
            var apiResponse = await apiClient.GetAsync($"http://localhost:5003/api/visitor/{visitId}" );
            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());
            
            // Assert
            foreach (var field in expectedResponse)
            {
                Assert.Equal(expectedResponse[field.Key], jsonResponse["visit"][field.Key]);
            }
        }
    }
}
        