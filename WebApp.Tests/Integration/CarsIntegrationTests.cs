using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApp.Tests.Integration
{
    [TestFixture]
    public class CarsIntegrationTests
    {
        private CustomWebApplicationFactory _factory=null!;
        private HttpClient _client = null!;

        [SetUp]
        public void SetUp()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [Test]
        public async Task Get_Cars_Without_Authentication_Should_Return_401()
        {
            // Act

            var response = await _client.GetAsync("/api/cars");

            // Assert 
            Assert.That(response.StatusCode,
                Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Login_Should_Return_AccessToken()
        {
            // Arrange

            var loginRequest = new
            {
                email = "admin1@test.com",
                password = "Admin_123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Auth/login",
                loginRequest);

            //Assert

            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.OK));

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            var accessToken =
                json.GetProperty("accessToken")
                .GetString();

            Assert.That(accessToken, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Get_Should_Return_Ok_When_User_Has_Valid_Token()
        {
            // Arrange

            var loginRequest = new
            {
                email = "admin1@test.com",
                password = "Admin_123"
            };

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                loginRequest);

            Assert.That(loginResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var json = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();

            var accessToken = json
                .GetProperty("accessToken")
                .GetString();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            // Act

            var response = await _client.GetAsync("/api/Cars");

            // Assert

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
