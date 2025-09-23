using NUnit.Framework;
using System.Net.Http.Json;
using UserIpService.Tests.Utils;

namespace UserIpService.Tests.Load
{
    [TestFixture]
    public class UserConnectionLoadTests
    {
        private CustomWebApplicationFactory _factory = default!;
        private HttpClient _client = default!;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _factory = new CustomWebApplicationFactory();
            await _factory.StartContainerAsync();
            _client = _factory.CreateClient();
        }

        [Test]
        [Category("Load")]
        public async Task Should_Handle_50000_Connections_In_30_Seconds()
        {
            const int totalUsers = 50000;
            const int durationSeconds = 30;

            var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(durationSeconds + 5));
            var rnd = new Random();
            var startTime = DateTime.UtcNow;

            await Parallel.ForEachAsync(Enumerable.Range(1, totalUsers), cancellation.Token,
                async (userId, token) =>
                {
                    int repeat = rnd.Next(1, 4); // каждый пользователь 1–3 раза
                    for (int i = 0; i < repeat; i++)
                    {
                        var dto = new { UserId = userId, Ip = $"10.0.{userId % 255}.{rnd.Next(1, 255)}" };
                        var response = await _client.PostAsJsonAsync("/api/events/connect", dto, token);
                        Assert.That(response.IsSuccessStatusCode, Is.True, $"Ошибка для пользователя {userId}");
                    }
                });

            var elapsed = DateTime.UtcNow - startTime;
            TestContext.WriteLine($"Завершено за {elapsed.TotalSeconds:F2} сек");

            Assert.That(elapsed.TotalSeconds, Is.LessThanOrEqualTo(durationSeconds + 5),
                "Обработка подключений превысила допустимое время.");
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            if (_client != null)
                _client.Dispose();

            if (_factory != null)
                await _factory.DisposeAsync();
        }
    }
}
