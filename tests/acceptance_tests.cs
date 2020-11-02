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
    public class AcceptanceTests
    {
        [Fact]
        public async Task GetHomeEndpoint()
        {
            var apiClient = new HttpClient();

            var apiResponse = await apiClient.GetAsync($"http://localhost:5003/api/home");

            Assert.True(apiResponse.IsSuccessStatusCode);

            var stringResponse = await apiResponse.Content.ReadAsStringAsync();

            Assert.Equal("This is the home page! Isn't it great!", stringResponse);
        }

        [Fact]
        public async Task PostLoginEndpointWithValidUsernameAndPassword()
        {
            var apiClient = new HttpClient();
            var user = new Login
            {
                Username = "Duncan",
                Password = "abc" 
            };
            // Serialize our concrete class into a JSON String
            var stringUser = await Task.Run(() => JsonConvert.SerializeObject(user));
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringUser, Encoding.UTF8, "application/json");

            var apiResponse = await apiClient.PostAsync($"http://localhost:5003/api/login", httpContent);

            Console.WriteLine(apiResponse);
            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());

            // returns userid, name, role and permissions
            //{ userid: 98909808, name: "Duncan", role: "admin", permissions: 1}
            Assert.Equal(JToken.FromObject(new { Username="Duncan", Password="abc"}), jsonResponse);
        }

        [Fact]
        public async Task PostLoginEndpointWithInvalidUsernameAndPassword()
        {
            var apiClient = new HttpClient();
            var user = new Login
            {
                Username = "Joanna",
                Password = "1234" 
            };
            // Serialize our concrete class into a JSON String
            var stringUser = await Task.Run(() => JsonConvert.SerializeObject(user));
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringUser, Encoding.UTF8, "application/json");

            var apiResponse = await apiClient.PostAsync($"http://localhost:5003/api/login", httpContent);

            var jsonResponse = JToken.Parse(await apiResponse.Content.ReadAsStringAsync());

            // returns userid, name, role and permissions
            //{ userid: 98909808, name: "Duncan", role: "admin", permissions: 1}
            Assert.Equal(JToken.FromObject(new {message = "Username and password is incorrect"}), jsonResponse);
        }

    }
}
