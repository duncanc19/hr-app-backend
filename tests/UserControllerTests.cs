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

namespace tests 
{
    public class UserControllerTests
    {
        [Fact]
        public async Task PutUserEndpointWithValidTelephoneField()
        {
            // Arrange
            var apiClient = new HttpClient();
            var change = JToken.FromObject(new { Telephone = "07716354633" });

            // Serialize our concrete class into a JSON String
            var stringChange = await Task.Run(() => JsonConvert.SerializeObject(change));
            
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringChange, Encoding.UTF8, "application/json");
            var userId = "18712a4f-744e-4e7c-a191-395fa832518b";

            var expectedResponse = JToken.FromObject(new { FirstName = "Azlina", Surname = "Yeo", Role = "Employee", PermissionLevel = "Default",
                        Telephone = "07716354633", Email = "azlina@happy.com", Location = "Singapore", NextOfKin = "Father", Address = "Bedok Reservoir Road",
                        Salary = "£29000", DoB = new DateTime(1979,01,01) });

            var apiResponse = await apiClient.PutAsync($"http://localhost:5003/api/user/{userId}", httpContent);
            // Assert
            Assert.True(apiResponse.IsSuccessStatusCode);

            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());

            Assert.Equal(expectedResponse, jsonResponse);
        }

        [Fact]
        public async Task PutUserEndpointWithValidFirstNameField()
        {
            // Arrange
            var apiClient = new HttpClient();
            var change = JToken.FromObject(new { FirstName = "Bob" });

            // Serialize our concrete class into a JSON String
            var stringChange = await Task.Run(() => JsonConvert.SerializeObject(change));
            
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringChange, Encoding.UTF8, "application/json");
            var userId = "18712a4f-744e-4e7c-a191-395fa832518b";

            var expectedResponse = JToken.FromObject(new { FirstName = "Bob", Surname = "Yeo", Role = "Employee", PermissionLevel = "Default",
                        Telephone = "0771333546433", Email = "azlina@happy.com", Location = "Singapore", NextOfKin = "Father", Address = "Bedok Reservoir Road",
                        Salary = "£29000", DoB = new DateTime(1979,01,01) });

            var apiResponse = await apiClient.PutAsync($"http://localhost:5003/api/user/{userId}", httpContent);
            // Assert
            Assert.True(apiResponse.IsSuccessStatusCode);

            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());

            Assert.Equal(expectedResponse, jsonResponse);
        }
    }
}
    
