using MediatR;
using UserIpService.Core.Entities;

namespace UserIpService.Application.Queries.GetUserLastConnection
{
    /// <summary>
    /// Запрос для получения последнего подключения пользователя.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetUserLastConnectionByUserIdQuery(long UserId) : IRequest<UserIp?>;
}
