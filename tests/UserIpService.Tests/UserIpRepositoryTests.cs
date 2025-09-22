using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Net;
using Testcontainers.PostgreSql;
using UserIpService.Core.Entities;
using UserIpService.Infrastructure.Data;
using UserIpService.Infrastructure.Repositories;

namespace UserIpService.Tests
{
    [TestFixture]
    public class UserIpRepositoryTests
    {
        private PostgreSqlContainer _dbContainer = default!;
        private UserIpContext _context = default!;
        private UserIpRepository _repository = default!;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {            
            _dbContainer = new PostgreSqlBuilder()
                .WithDatabase("useripdb")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            await _dbContainer.StartAsync();
            
            var options = new DbContextOptionsBuilder<UserIpContext>()
                .UseNpgsql(_dbContainer.GetConnectionString())
                .Options;

            _context = new UserIpContext(options);
            await _context.Database.EnsureCreatedAsync();

            _repository = new UserIpRepository(_context);
        }

        [Test]
        public async Task UpsertAsync_ShouldInsertAndUpdate()
        {
            // Arrange
            var entry = new UserIp
            {
                UserId = 1,
                IpText = "192.168.0.1",
                IpAddress = IPAddress.Parse("192.168.0.1"),
                FirstSeen = DateTimeOffset.UtcNow,
                LastSeen = DateTimeOffset.UtcNow,
                Count = 1
            };

            // Act
            await _repository.UpsertAsync(entry);

            // Assert
            var inserted = await _context.UserIps.FirstOrDefaultAsync();
            Assert.That(inserted, Is.Not.Null);
            Assert.That(inserted!.Count, Is.EqualTo(1));

            // Act
            await _repository.UpsertAsync(entry);

            // Assert
            var updated = await _context.UserIps.FirstOrDefaultAsync();
            Assert.That(updated!.Count, Is.EqualTo(2));
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }

            if (_dbContainer != null)
            {
                await _dbContainer.DisposeAsync();
            }
        }
    }
}
