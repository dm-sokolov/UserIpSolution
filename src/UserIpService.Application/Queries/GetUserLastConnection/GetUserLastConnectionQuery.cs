using MediatR;
using UserIpService.Core.Entities;

namespace UserIpService.Application.Queries.GetUserLastConnection
{
    public record GetUserLastConnectionQuery(long UserId) : IRequest<UserIp?>;
}
