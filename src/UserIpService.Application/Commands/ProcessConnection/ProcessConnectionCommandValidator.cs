using FluentValidation;
using System.Net;

namespace UserIpService.Application.Commands.ProcessConnection
{
    /// <summary>
    /// Валидатор для команды на регистрацию подключения пользователя
    /// </summary>
    public class ProcessConnectionCommandValidator : AbstractValidator<ProcessConnectionCommand>
    {
        public ProcessConnectionCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId должен быть больше 0.");

            RuleFor(x => x.IpText)
                .Must(BeValidIp).WithMessage("Некорректный IPv4 или IPv6 адрес.");
        }

        private bool BeValidIp(string ip)
            => IPAddress.TryParse(ip, out _);
    }
}
