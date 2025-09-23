using NUnit.Framework;
using System.Net.Http.Json;
using UserIpService.Tests.Utils;

namespace UserIpService.Tests
{
    [TestFixture]
    public class ApiIntegrationTests
    {
        private CustomWebApplicationFactory _factory = default!;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _factory = new CustomWebApplicationFactory();
            await _factory.StartContainerAsync();
        }

        [Test]
        public async Task ConnectAndQuery_ShouldReturnUser()
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("/api/events/connect", new { UserId = 42L, Ip = "10.0.0.5" });

            var response = await client.GetAsync("/api/users/find-by-ip?prefix=10.0");
            var users = await response.Content.ReadFromJsonAsync<List<long>>();

            Assert.That(users, Does.Contain(42L));
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            if (_factory != null)
                await _factory.DisposeAsync();
        }
    }
}
