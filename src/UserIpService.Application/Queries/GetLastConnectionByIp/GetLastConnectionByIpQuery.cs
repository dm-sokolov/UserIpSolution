using MediatR;

namespace UserIpService.Application.Queries.GetLastConnectionByIp
{
    public record GetLastConnectionByIpQuery(string Ip) : IRequest<DateTimeOffset?>;
}
