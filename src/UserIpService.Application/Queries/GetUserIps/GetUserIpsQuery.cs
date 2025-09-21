using MediatR;
using UserIpService.Core.Entities;

namespace UserIpService.Application.Queries.GetUserIps
{
    public record GetUserIpsQuery(long UserId) : IRequest<IReadOnlyCollection<UserIp>>;
}
