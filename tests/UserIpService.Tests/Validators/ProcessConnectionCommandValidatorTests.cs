using NUnit.Framework;
using UserIpService.Application.Commands.ProcessConnection;

namespace UserIpService.Tests.Validators
{
    [TestFixture]
    public class ProcessConnectionCommandValidatorTests
    {
        [TestCase("192.168.1.1", true, TestName = "Допустимый IPv4 — должен пройти проверку")]
        [TestCase("2001:0db8:85a3::8a2e:0370:7334", true, TestName = "Допустимый IPv6 — должен пройти проверку")]
        [TestCase("10.0.0.5", true, TestName = "Ещё один допустимый IPv4 — должен пройти проверку")]
        [TestCase("::1", true, TestName = "IPv6 Loopback — должен пройти проверку")]
        [TestCase("invalid-ip", false, TestName = "Недопустимый IP — должен не пройти проверку")]
        [TestCase("", false, TestName = "Пустая строка — должна не пройти проверку")]
        [TestCase("1234", false, TestName = "Не является IP — должен не пройти проверку")]
        public void Validate_IP_Address(string ip, bool expectedIsValid)
        {
            // Arrange
            var validator = new ProcessConnectionCommandValidator();

            // Act
            var result = validator.Validate(new ProcessConnectionCommand(1, ip));

            // Assert
            Assert.That(result.IsValid, Is.EqualTo(expectedIsValid));
        }
    }
}
