using MediatR;
using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;

namespace UserIpService.Application.Queries.GetUserLastConnection
{
    public class GetUserLastConnectionQueryHandler : IRequestHandler<GetUserLastConnectionQuery, UserIp?>
    {
        private readonly IUserIpRepository _repository;

        public GetUserLastConnectionQueryHandler(IUserIpRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserIp?> Handle(GetUserLastConnectionQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetUserLastConnectionAsync(request.UserId, cancellationToken);
        }
    }
}
