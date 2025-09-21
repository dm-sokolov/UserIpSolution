using MediatR;

namespace UserIpService.Application.Queries.FindUsersByIpPrefix
{
    /// <summary>
    /// Запрос на получение списка пользователей по IP-префиксу.
    /// </summary>
    public record FindUsersByIpPrefixQuery(string Prefix) : IRequest<IReadOnlyCollection<long>>;
}
