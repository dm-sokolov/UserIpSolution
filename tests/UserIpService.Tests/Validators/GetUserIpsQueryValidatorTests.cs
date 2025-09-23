using NUnit.Framework;
using UserIpService.Application.Queries.GetUserIps;

namespace UserIpService.Tests.Validators
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
            var validator = new GetUserIpsByUserIdQueryValidator();
            var result = validator.Validate(new GetUserIpsByUserIdQuery(userId));
            Assert.That(result.IsValid, Is.EqualTo(expected));
        }
    }
}
