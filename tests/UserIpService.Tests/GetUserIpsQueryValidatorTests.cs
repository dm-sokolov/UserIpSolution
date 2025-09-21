using NUnit.Framework;
using UserIpService.Application.Queries.GetUserIps;

namespace UserIpService.Tests
{
    [TestFixture]
    public class GetUserIpsQueryValidatorTests
    {
        [TestCase(1, true)]
        [TestCase(42, true)]
        [TestCase(0, false)]
        [TestCase(-5, false)]
        public void Validate_ShouldReturnExpectedResult(long userId, bool expected)
        {
            var validator = new GetUserIpsQueryValidator();
            var result = validator.Validate(new GetUserIpsQuery(userId));
            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
