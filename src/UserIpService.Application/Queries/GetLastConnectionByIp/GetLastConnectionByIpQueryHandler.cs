using MediatR;
using UserIpService.Core.Interfaces;

namespace UserIpService.Application.Queries.GetLastConnectionByIp
{
    public class GetLastConnectionByIpQueryHandler : IRequestHandler<GetLastConnectionByIpQuery, DateTimeOffset?>
    {
        private readonly IUserIpRepository _repository;

        public GetLastConnectionByIpQueryHandler(IUserIpRepository repository)
        {
            _repository = repository;
        }

        public async Task<DateTimeOffset?> Handle(GetLastConnectionByIpQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetLastConnectionByIpAsync(request.Ip, cancellationToken);
        }
    }
}
