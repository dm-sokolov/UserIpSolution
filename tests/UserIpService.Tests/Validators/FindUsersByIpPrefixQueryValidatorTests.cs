using NUnit.Framework;
using UserIpService.Application.Queries.FindUsersByIpPrefix;

namespace UserIpService.Tests.Validators
{
    [TestFixture]
    public class FindUsersByIpPrefixQueryValidatorTests
    {
        [TestCase("192.168", true)]
        [TestCase("2001:db8::", true)]
        [TestCase("", false)]
        [TestCase("%%%%", false)]
        public void Validate_ShouldReturnExpectedResult(string prefix, bool expected)
        {
            var validator = new FindUsersByIpPrefixQueryValidator();
            var result = validator.Validate(new FindUsersByIpPrefixQuery(prefix));
            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
