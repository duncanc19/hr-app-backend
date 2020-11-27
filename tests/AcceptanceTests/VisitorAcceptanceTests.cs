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
     
        //[Fact]
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

        //[Fact]
        public async Task GetVisitEndpointWithId()
        {
            // Arrange
            var apiClient = new HttpClient();

            
            var visitId = "cfe0d661-e890-4ab5-93c4-6f13eb769f74";
            var expectedResponse = new Dictionary<string, string>(){
                {"firstName", "Bob"},
                {"surname", "Ross"},
                {"company", "NHS"},
                {"role", "Doctor"},
                {"telephone", "01234567890"},
                {"email", "bobross@nhs.com"},
                {"employeeEmail", "dcarter@skillsforcare.org"}
                // {"appointment", $"{new DateTime(2020,12,18,15,30,00).ToString("dd/MM/yyyy HH:mm:ss")}"}
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

        //[Fact]
        public async Task DeleteAVisit()
        {
            // Arrange
            var apiClient = new HttpClient();
            var newVisit = JToken.FromObject(new { FirstName = "John", Surname = "Doe", Company = "Disneyland", Role =  "Marketing", Telephone = "07716354633", Email = "johnd@disneyland.com", EmployeeEmail = "jfawl@skillsforcare.org", Appointment = new DateTime(2020,12,01,12,30,00)});

            // Serialize our concrete class into a JSON String
            var stringVisit = await Task.Run(() => JsonConvert.SerializeObject(newVisit));

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringVisit, Encoding.UTF8, "application/json");

            // Create a new visitor ready to delete
            var postApiResponse = await apiClient.PostAsync($"http://localhost:5003/api/visitor", httpContent);
            var postJsonResponse = JToken.Parse(await postApiResponse.Content.ReadAsStringAsync());
            var visitId = postJsonResponse["visit"]["visitorId"];

            var expectedResponse = JToken.FromObject(new { message = "Visit has been deleted successfully" });

            // Act
            var apiResponse = await apiClient.DeleteAsync($"http://localhost:5003/api/visitor/{visitId}");
            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());
            
            // Assert
            Assert.Equal(expectedResponse, jsonResponse);
        }

        
        //[Fact]
        public async Task EditAVisit()
            {
            // Test first edit for visit
            // Arrange
            var apiClient = new HttpClient();
            var editVisit = JToken.FromObject(new { VisitorId = "c6f19c35-666f-4faa-a9ef-349b85f775c5", FirstName = "Annie", Email="annier@madetech.com"});
            var visitId = "c6f19c35-666f-4faa-a9ef-349b85f775c5";
            // Serialize our concrete class into a JSON String
            var stringEditVisit = await Task.Run(() => JsonConvert.SerializeObject(editVisit));

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringEditVisit, Encoding.UTF8, "application/json");

            var putExpectedResponse = new Dictionary<string, string>(){
                {"firstName", "Annie"},
                {"surname", "Richards"},
                {"company", "Made Tech"},
                {"role", "Engineer"},
                {"telephone", "07716354633"},
                {"email", "annier@madetech.com"},
                {"employeeEmail", "rpentecost@skillsforcare.org"}
                // {"appointment", $"{new DateTime(2020,12,01,12,30,00).ToString("dd/MM/yyyy HH:mm:ss")}"}
            };

            // Act
            var putApiResponse = await apiClient.PutAsync($"http://localhost:5003/api/visitor/{visitId}", httpContent);
            var putJsonResponse = JToken.Parse(await putApiResponse.Content.ReadAsStringAsync());
          
            // Assert
            foreach (var field in putExpectedResponse)
            {
                Assert.Equal(putExpectedResponse[field.Key], putJsonResponse["visit"][field.Key]);
            }

            // Change edits back
            // Arrange
            editVisit = JToken.FromObject(new { VisitorId = "c6f19c35-666f-4faa-a9ef-349b85f775c5", FirstName = "Charlotte", Email="charlotter@madetech.com"});
            // Serialize our concrete class into a JSON String
            stringEditVisit = await Task.Run(() => JsonConvert.SerializeObject(editVisit));

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            httpContent = new StringContent(stringEditVisit, Encoding.UTF8, "application/json");

            var expectedResponse = new Dictionary<string, string>(){
                {"firstName", "Charlotte"},
                {"surname", "Richards"},
                {"company", "Made Tech"},
                {"role", "Engineer"},
                {"telephone", "07716354633"},
                {"email", "charlotter@madetech.com"},
                {"employeeEmail", "rpentecost@skillsforcare.org"}
                // {"appointment", $"{new DateTime(2020,12,01,12,30,00).ToString("dd/MM/yyyy HH:mm:ss")}"}
            };

            // Act
            var apiResponse = await apiClient.PutAsync($"http://localhost:5003/api/visitor/{visitId}", httpContent);
            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());
          
            // Assert
            foreach (var field in putExpectedResponse)
            {
                Assert.Equal(expectedResponse[field.Key], jsonResponse["visit"][field.Key]);
            }
        }
    }
}
        