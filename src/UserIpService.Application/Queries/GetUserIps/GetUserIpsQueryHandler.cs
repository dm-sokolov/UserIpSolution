using MediatR;
using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;

namespace UserIpService.Application.Queries.GetUserIps
{
    public class GetUserIpsQueryHandler : IRequestHandler<GetUserIpsQuery, IReadOnlyCollection<UserIp>>
    {
        private readonly IUserIpRepository _repository;

        public GetUserIpsQueryHandler(IUserIpRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<UserIp>> Handle(GetUserIpsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetUserIpsAsync(request.UserId, cancellationToken);
        }
    }
}
