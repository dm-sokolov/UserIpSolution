using MediatR;
using UserIpService.Core.Interfaces;

namespace UserIpService.Application.Queries.FindUsersByIpPrefix
{
    public class FindUsersByIpPrefixQueryHandler : IRequestHandler<FindUsersByIpPrefixQuery, IReadOnlyCollection<long>>
    {
        private readonly IUserIpRepository _repository;

        public FindUsersByIpPrefixQueryHandler(IUserIpRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<long>> Handle(FindUsersByIpPrefixQuery request, CancellationToken cancellationToken)
        {
            return await _repository.FindUsersByIpPrefixAsync(request.Prefix, cancellationToken);
        }
    }
}
