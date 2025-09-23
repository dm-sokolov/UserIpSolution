using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using UserIpService.Api;
using UserIpService.Infrastructure.Data;

namespace UserIpService.Tests.Utils
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncDisposable
    {
        private readonly PostgreSqlContainer _dbContainer;

        public CustomWebApplicationFactory()
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithDatabase("useripdb")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
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
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
            Dispose();
        }

        public async Task StartContainerAsync() 
            => await _dbContainer.StartAsync();
    }
}
