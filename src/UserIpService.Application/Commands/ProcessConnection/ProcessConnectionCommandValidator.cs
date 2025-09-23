using FluentValidation;
using System.Net;
using System.Net.Sockets;

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
        {
            if (string.IsNullOrWhiteSpace(ip))
                return false;

            if (IPAddress.TryParse(ip, out var addr))
            {
                return addr.AddressFamily == AddressFamily.InterNetwork
                    || addr.AddressFamily == AddressFamily.InterNetworkV6;
            }

            return false;
        }
    }
}
