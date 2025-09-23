using MediatR;

namespace UserIpService.Application.Commands.ProcessConnection
{
    /// <summary>
    /// Команда на регистрацию подключения пользователя
    /// </summary>
    public record ProcessConnectionCommand(long UserId, string IpText) : IRequest;
}
