using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http.Json;
using Testcontainers.PostgreSql;
using UserIpService.Api;
using UserIpService.Infrastructure.Data;

namespace UserIpService.Tests
{
    [TestFixture]
    public class ApiIntegrationTests
    {
        private PostgreSqlContainer _dbContainer = default!;
        private WebApplicationFactory<Program> _factory = default!;

        [OneTimeSetUp]
        public async Task Setup()
        {
            // Создаём PostgreSQL контейнер без Dotnet.Testcontainers.* пространств имён
            _dbContainer = new PostgreSqlBuilder()
                .WithDatabase("useripdb")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            await _dbContainer.StartAsync();

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Удаляем старый DbContext
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<UserIpContext>));
                        if (descriptor != null)
                            services.Remove(descriptor);

                        // Настраиваем DbContext на контейнер PostgreSQL
                        services.AddDbContext<UserIpContext>(options =>
                            options.UseNpgsql(_dbContainer.GetConnectionString()));
                    });
                });
        }

        [Test]
        public async Task ConnectAndQuery_ShouldReturnUser()
        {
            var client = _factory.CreateClient();

            // Отправляем событие подключения
            await client.PostAsJsonAsync("/api/events/connect", new { UserId = 42L, Ip = "10.0.0.5" });

            // Проверяем поиск пользователя по IP-префиксу
            var response = await client.GetAsync("/api/users/find-by-ip?prefix=10.0");
            var users = await response.Content.ReadFromJsonAsync<List<long>>();

            Assert.That(users, Does.Contain(42L));
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            if (_factory != null)
            {
                _factory.Dispose(); // ✅ Освобождаем фабрику
            }

            if (_dbContainer != null)
            {
                await _dbContainer.DisposeAsync(); // ✅ Останавливаем контейнер
            }
        }
    }
}
