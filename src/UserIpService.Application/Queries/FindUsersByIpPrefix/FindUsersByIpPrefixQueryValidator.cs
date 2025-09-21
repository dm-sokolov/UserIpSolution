using FluentValidation;
using System.Net;

namespace UserIpService.Application.Queries.FindUsersByIpPrefix
{
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
            // Простейшая проверка: префикс должен быть началом корректного IPv4/IPv6
            return IPAddress.TryParse(prefix, out _)
                   || prefix.Any(char.IsDigit)
                   || prefix.Contains(':'); // простая эвристика для IPv6
        }
    }
}
