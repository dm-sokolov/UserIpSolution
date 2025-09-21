using NUnit.Framework;
using UserIpService.Application.Commands.ProcessConnection;

namespace UserIpService.Tests
{
    [TestFixture]
    public class ProcessConnectionCommandValidatorTests
    {
        [Test]
        public void Should_Pass_When_Valid_IPv6()
        {
            var validator = new ProcessConnectionCommandValidator();
            var result = validator.Validate(new ProcessConnectionCommand(1, "2001:0db8:85a3::8a2e:0370:7334"));

            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void Should_Fail_When_Invalid_IP()
        {
            var validator = new ProcessConnectionCommandValidator();
            var result = validator.Validate(new ProcessConnectionCommand(1, "invalid-ip"));

            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void Should_Pass_When_Valid_IPv4()
        {
            var validator = new ProcessConnectionCommandValidator();
            var result = validator.Validate(new ProcessConnectionCommand(1, "192.168.1.1"));

            Assert.That(result.IsValid, Is.True);
        }
    }
}
