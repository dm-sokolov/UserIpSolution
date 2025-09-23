using MediatR;

namespace UserIpService.Application.Queries.GetLastConnectionByIp
{
    /// <summary>
    /// Запрос для получения времени последнего подключения по IP.
    /// </summary>
    public record GetLastConnectionDateTimeByIpQuery(string Ip) : IRequest<DateTimeOffset?>;
}
