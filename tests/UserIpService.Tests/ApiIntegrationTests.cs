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
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<UserIpContext>));
                        if (descriptor != null)
                            services.Remove(descriptor);

                        services.AddDbContext<UserIpContext>(options =>
                            options.UseNpgsql(_dbContainer.GetConnectionString()));
                        
                        using var sp = services.BuildServiceProvider();
                        using var scope = sp.CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<UserIpContext>();
                        context.Database.Migrate();
                    });
                });
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
            {
                _factory.Dispose(); 
            }

            if (_dbContainer != null)
            {
                await _dbContainer.DisposeAsync(); 
            }
        }
    }
}
