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
        [TestCase(100001L, "127.0.0.1", TestName = "IPv4 127.0.0.1 — должен вызвать UpsertAsync")]
        [TestCase(42L, "10.0.0.5", TestName = "IPv4 10.0.0.5 — должен вызвать UpsertAsync")]
        [TestCase(7L, "::1", TestName = "IPv6 loopback ::1 — должен вызвать UpsertAsync")]
        [TestCase(123456L, "2001:0db8:85a3::8a2e:0370:7334", TestName = "Полный IPv6 — должен вызвать UpsertAsync")]
        public async Task ProcessConnectionAsync_CallsUpsertAsync(long userId, string ip)
        {
            // Arrange
            var repo = Substitute.For<IUserIpRepository>();
            var service = new UserConnectionService(repo);

            // Act
            await service.ProcessConnectionAsync(userId, ip);

            // Assert
            await repo.Received(1).UpsertAsync(
                Arg.Is<UserIp>(u => u.UserId == userId && u.IpText == ip));
        }
    }
}
