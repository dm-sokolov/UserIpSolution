using NUnit.Framework;
using UserIpService.Application.Queries.GetLastConnectionByIp;

namespace UserIpService.Tests.Validators
{
    [TestFixture]
    public class GetLastConnectionByIpQueryValidatorTests
    {
        [TestCase("192.168.1.1", true)]
        [TestCase("2001:db8::1", true)]
        [TestCase("", false)]
        [TestCase("not-an-ip", false)]
        public void Validate_ShouldReturnExpectedResult(string ip, bool expected)
        {
            var validator = new GetLastConnectionDateTimeByIpQueryValidator();
            var result = validator.Validate(new GetLastConnectionDateTimeByIpQuery(ip));
            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
