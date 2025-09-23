using FluentValidation;
using System.Net;

namespace UserIpService.Application.Queries.FindUsersByIpPrefix
{
    /// <summary>
    /// Валидатор для запроса на получение списка пользователей по IP-префиксу.
    /// </summary>
    public class FindUsersByIpPrefixQueryValidator : AbstractValidator<FindUsersByIpPrefixQuery>
    {
        public FindUsersByIpPrefixQueryValidator()
        {
            RuleFor(x => x.Prefix)
                .NotEmpty().WithMessage("IP-префикс не может быть пустым.")
                .Must(BeValidPrefix).WithMessage("Некорректный формат IP-префикса.");
        }

        private bool BeValidPrefix(string prefix)
        {
            var result = IPAddress.TryParse(prefix, out _) || prefix.Any(char.IsDigit) || prefix.Contains(':');

            return result;
        }
    }
}
