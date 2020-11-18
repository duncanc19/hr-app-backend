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
    public class AcceptanceTests
    {
        [Fact]
        public async Task GetHomeEndpoint()
        {
            var apiClient = new HttpClient();

            var apiResponse = await apiClient.GetAsync($"http://localhost:5003/api/home");

            Assert.True(apiResponse.IsSuccessStatusCode);

            var stringResponse = await apiResponse.Content.ReadAsStringAsync();

            Assert.Equal("This is the home page! Testing, testing...", stringResponse);
        }

        [Fact]
        public async Task LogsInWithCorrectEmailAndPassword()
        {
            var apiClient = new HttpClient();
            var user = new Login
            {
                Email = "rpentecost@skillsforcare.org",
                Password = "password" 
            };
            var stringUser = await Task.Run(() => JsonConvert.SerializeObject(user));
            var httpContent = new StringContent(stringUser, Encoding.UTF8, "application/json");
            var apiResponse = await apiClient.PostAsync($"http://localhost:5003/api/login/authenticate", httpContent);
            var token = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());

            Assert.True(apiResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task LoginFailsWithIncorrectEmailAndPassword()
        {
            var apiClient = new HttpClient();
            var user = new Login
            {
                Email = "rpentecost@skillsforcare.org",
                Password = "passwrd" 
            };
            var stringUser = await Task.Run(() => JsonConvert.SerializeObject(user));
            var httpContent = new StringContent(stringUser, Encoding.UTF8, "application/json");
            var apiResponse = await apiClient.PostAsync($"http://localhost:5003/api/login/authenticate", httpContent);
            
            Assert.False(apiResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetUserEndpointWithId()
        {
            // Arrange
            var apiClient = new HttpClient();
            var userId = "e1170b4c-7084-4f29-9368-1d7b5f6d77df";
            var expectedResponse = new Dictionary<string, string>(){
                {"firstName", "Joanna"},
                {"surname", "Fawl"},
                {"role", "Academy Engineer"},
                {"adminLevel", "Employee"},
                {"telephone", "07987654321"},
                {"email", "jfawl@skillsforcare.org"},
                {"location", "Manchester"},
                {"nextOfKin", "Mum"},
                {"address", "Deansgate"},
                {"password", "1234"},
                {"managerEmail", "csudbery@skillsforcare.org"},
                {"salary", "£30000"}
            };

            // Act
            var apiResponse = await apiClient.GetAsync($"http://localhost:5003/api/user/{userId}");
            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());
            
            // Assert
            foreach (var field in expectedResponse)
            {
                Assert.Equal(expectedResponse[field.Key], jsonResponse["user"][field.Key]);
            }
        }
        
        [Fact]
        public async Task GetUserEndpointWithInvalidId()
        {
            // Arrange
            var apiClient = new HttpClient();
            var userId = "e1170b4c-7084-4f29-9368-1d7b5f6d77dd";
            var expectedResponse = JToken.FromObject(new {message = "ID does not exist" });

            // Act
            var apiResponse = await apiClient.GetAsync($"http://localhost:5003/api/user/{userId}");
            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());
            
            // Assert
            Assert.False(apiResponse.IsSuccessStatusCode);
            Assert.Equal(expectedResponse, jsonResponse);
        }

        // [Fact]
        public async Task PutUserEndpointWithValidFields()
        {
            // Arrange
            var apiClient = new HttpClient();
            var change = JToken.FromObject(new { UserId = "e1170b4c-7084-4f29-9368-1d7b5f6d77df", FirstName = "Harry", Address = "Disneyland", Telephone = "07716354633"});

            // Serialize our concrete class into a JSON String
            var stringChange = await Task.Run(() => JsonConvert.SerializeObject(change));
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringChange, Encoding.UTF8, "application/json");
            // var userId = "18712a4f-744e-4e7c-a191-395fa832518b";


            var userId = "e1170b4c-7084-4f29-9368-1d7b5f6d77df";
            var expectedResponse = new Dictionary<string, string>(){
                {"firstName", "Harry"},
                {"surname", "Fawl"},
                {"role", "Academy Engineer"},
                {"adminLevel", "Employee"},
                {"telephone", "07716354633"},
                {"email", "jfawl@skillsforcare.org"},
                {"location", "Manchester"},
                {"nextOfKin", "Mum"},
                {"address", "Disneyland"},
                {"password", "1234"},
                {"managerEmail", "csudbery@skillsforcare.org"},
                {"salary", "£30000"}
            };
            // var expectedResponse = JToken.FromObject(new {user = new { FirstName = "Harry", Surname = "Yeo", Role = "Employee", PermissionLevel = "Default",
            //             Telephone = "0771635463dd3", Email = "azlina@happy.com", Location = "Singapore", NextOfKin = "Father", Address = "Disneyland",
            //             Salary = "£29000", DoB = new DateTime(1979,01,01) }});

            var apiResponse = await apiClient.PutAsync($"http://localhost:5003/api/user/{userId}", httpContent);
            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());
            
            // Assert
            Assert.True(apiResponse.IsSuccessStatusCode);
            foreach (var field in expectedResponse)
            {
                // Console.WriteLine(expectedResponse[field.Key]);
                // Console.WriteLine(jsonResponse["user"][field.Key]);
                Assert.Equal(expectedResponse[field.Key], jsonResponse["user"][field.Key]);
            }
        }

        // [Fact]
        public async Task PutUserEndpointWithInvalidID()
        {
            // Arrange
            var apiClient = new HttpClient();
            var change = JToken.FromObject(new { FirstName = "Harry", Address = "Disneyland", Telephone = "0771635463dd3"});

            // Serialize our concrete class into a JSON String
            var stringChange = await Task.Run(() => JsonConvert.SerializeObject(change));
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringChange, Encoding.UTF8, "application/json");
            var userId = "18712a4f-744e-4e7c-a191-395fa832519b";

            var expectedResponse = JToken.FromObject(new { message = "ID does not exist" });

            var apiResponse = await apiClient.PutAsync($"http://localhost:5003/api/user/{userId}", httpContent);
            // Assert
            Assert.False(apiResponse.IsSuccessStatusCode);

            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());

            Assert.Equal(expectedResponse, jsonResponse);
        }

        // [Fact]
        public async Task GetAllUsersFromUserEndpoint()
        {
            // Arrange
            var apiClient = new HttpClient();

            var expectedResponse = JToken.FromObject(new {users = new ArrayList() { 
                        new { FirstName = "Duncan", Surname = "Carter", Role = "Employee", PermissionLevel = "Default",
                        Telephone = "0771333333333", Email = "duncan@dc.com", Location = "Paris", NextOfKin = "Mother", Address = "Champs Elysee",
                        Salary = "£56000", DoB = new DateTime(1912,01,01) }, 
                        new { FirstName = "Azlina", Surname = "Yeo", Role = "Employee", PermissionLevel = "Default",
                        Telephone = "0771333546433", Email = "azlina@happy.com", Location = "Singapore", NextOfKin = "Father", Address = "Bedok Reservoir Road",
                        Salary = "£29000", DoB = new DateTime(1979,01,01) }, 
                        new { FirstName = "Joanna", Surname = "Fawl", Role = "Employee", PermissionLevel = "Default",
                        Telephone = "07713344333", Email = "joanna@jf.com", Location = "Seattle", NextOfKin = "Brother", Address = "The Tower",
                        Salary = "£75000", DoB = new DateTime(1917,05,08) } 
                        } });

            var apiResponse = await apiClient.GetAsync($"http://localhost:5003/api/user/all");
            // Assert
            Assert.True(apiResponse.IsSuccessStatusCode);

            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());

            Assert.Equal(expectedResponse, jsonResponse);
        }

        // [Fact]
        public async Task PostUserEndpointWithValidID()
        {
            // Arrange
            var apiClient = new HttpClient();
            var newUser = JToken.FromObject(new { FirstName = "Richard", Surname = "Pentecost", Role = "Employee", PermissionLevel = "Default",
                        Telephone = "07713343333", Email = "richard@rp.com", Location = "Manchester", NextOfKin = "Mother", Address = "Deansgate",
                        Salary = "£56000", DoB = new DateTime(1912,01,01) });

            // Serialize our concrete class into a JSON String
            var stringUser = await Task.Run(() => JsonConvert.SerializeObject(newUser));
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringUser, Encoding.UTF8, "application/json");

            var apiResponse = await apiClient.PostAsync($"http://localhost:5003/api/user", httpContent);
            // Assert
            // Assert.True(apiResponse.IsSuccessStatusCode);

            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());
            

            // var userObject = JsonConvert.DeserializeObject(jsonResponse.ToString(), User); 
            // Console.WriteLine(userObject);
            // jsonResponse.ToObject<User>();
            //Assert.Equal("Richard", userObject.UserInfo.FirstName);
            // Assert.Equal("Pentecost", jsonResponse.UserInfo.Surname);
            // Assert.Equal("Employee", jsonResponse.UserInfo.Role);
            // Assert.Equal("Default", jsonResponse.UserInfo.PermissionLevel);
            // Assert.Equal("07713343333", jsonResponse.UserInfo.Telephone);
            // Assert.Equal("richard@rp.com", jsonResponse.UserInfo.Email);
            // Assert.Equal("Manchester", jsonResponse.UserInfo.Location);
            // Assert.Equal("Mother", jsonResponse.UserInfo.NextOfKin);
            // Assert.Equal("Deansgate", jsonResponse.UserInfo.Address);
            // Assert.Equal("£56000", jsonResponse.UserInfo.Salary);
            // Assert.Equal(new DateTime(1912,01,01), jsonResponse.UserInfo.DoB);
        }

        // [Fact]
        public async Task DeleteUserEndpointWithValidID()
        {
            // Arrange
            var apiClient = new HttpClient();
            var userId = "c0a68046-617e-4927-bd9b-c14ce8f497e1";
            var expectedResponse = JToken.FromObject(new { message = "User has been deleted successfully" });

            // Act
            var apiResponse = await apiClient.DeleteAsync($"http://localhost:5003/api/user/{userId}");
            // Assert
            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());
            Assert.True(apiResponse.IsSuccessStatusCode);

            Assert.Equal(expectedResponse, jsonResponse);
            
        }
    }
}
