using MediatR;

namespace UserIpService.Application.Commands.ProcessConnection
{
    /// <summary>
    /// Команда для обработки события подключения пользователя.
    /// </summary>
    public record ProcessConnectionCommand(long UserId, string IpText) : IRequest;
}
