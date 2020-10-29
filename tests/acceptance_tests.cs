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
    }
}
