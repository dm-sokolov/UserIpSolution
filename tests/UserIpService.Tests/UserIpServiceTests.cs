using NSubstitute;
using NUnit.Framework;
using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;
using UserIpService.Core.Services;

namespace UserIpService.Tests
{
    [TestFixture]
    public class UserIpServiceTests
    {
        [Test]
        public async Task ProcessConnectionAsync_CallsUpsertAsync()
        {
            // Arrange
            var repo = Substitute.For<IUserIpRepository>();
            var service = new UserConnectionService(repo);

            // Act
            await service.ProcessConnectionAsync(100001, "127.0.0.1");

            // Assert
            await repo.Received(1).UpsertAsync(
                Arg.Is<UserIp>(u => u.UserId == 100001 && u.IpText == "127.0.0.1"));
        }
    }
}
