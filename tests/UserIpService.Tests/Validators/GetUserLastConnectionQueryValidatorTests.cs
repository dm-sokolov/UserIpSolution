using NUnit.Framework;
using UserIpService.Application.Queries.GetUserLastConnection;

namespace UserIpService.Tests.Validators
{
    [TestFixture]
    public class GetUserLastConnectionQueryValidatorTests
    {
        [TestCase(1, true)]
        [TestCase(999, true)]
        [TestCase(0, false)]
        [TestCase(-10, false)]
        public void Validate_ShouldReturnExpectedResult(long userId, bool expectedValid)
        {
            var validator = new GetUserLastConnectionByUserIdQueryValidator();
            var result = validator.Validate(new GetUserLastConnectionByUserIdQuery(userId));
            Assert.That(result.IsValid, Is.EqualTo(expectedValid));
        }
    }
}
