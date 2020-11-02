using System;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;

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
        public async Task PostUsernameAndPasswordEndpoint()
        {
            var apiClient = new HttpClient();
            var user = new User
            {
                Username = "Duncan",
                Password = "abc" 
            };
            // Serialize our concrete class into a JSON String
            var stringUser = await Task.Run(() => JsonConvert.SerializeObject(user));
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringUser, Encoding.UTF8, "application/json");

            var apiResponse = await apiClient.PostAsync($"http://localhost:5003/api/login", httpContent);

            var stringResponse = await apiResponse.Content.ReadAsStringAsync();

            // returns userid, name, role and permissions
            Assert.Equal("true", stringResponse);
        }

    }
}
