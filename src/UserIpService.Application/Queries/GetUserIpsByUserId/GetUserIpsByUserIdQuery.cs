using MediatR;
using UserIpService.Core.Entities;

namespace UserIpService.Application.Queries.GetUserIps
{
    /// <summary>
    /// Запрос для получения списка IP-адресов пользователя.
    /// </summary>
    public record GetUserIpsByUserIdQuery(long UserId) : IRequest<IReadOnlyCollection<UserIp>>;
}
