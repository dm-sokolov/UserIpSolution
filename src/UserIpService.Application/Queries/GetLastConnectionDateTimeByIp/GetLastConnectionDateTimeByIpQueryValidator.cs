using FluentValidation;
using System.Net;

namespace UserIpService.Application.Queries.GetLastConnectionByIp
{
    /// <summary>
    /// Валидатор для запроса получения времени последнего подключения по IP.
    /// </summary>
    public class GetLastConnectionDateTimeByIpQueryValidator : AbstractValidator<GetLastConnectionDateTimeByIpQuery>
    {
        public GetLastConnectionDateTimeByIpQueryValidator()
        {
            RuleFor(x => x.Ip)
                .NotEmpty()
                .WithMessage("IP-адрес не может быть пустым.")
                .Must(BeValidIp)
                .WithMessage("Некорректный формат IP-адреса (поддерживаются IPv4 и IPv6).");
        }

        private bool BeValidIp(string ip) 
            => IPAddress.TryParse(ip, out _);
    }
}
