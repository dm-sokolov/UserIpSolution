using MediatR;
using UserIpService.Core.Interfaces;

namespace UserIpService.Application.Queries.FindUsersByIpPrefix
{
    /// <summary>
    /// Обработчик для запроса на получение списка пользователей по IP-префиксу.
    /// </summary>
    public class FindUsersByIpPrefixQueryHandler : IRequestHandler<FindUsersByIpPrefixQuery, IReadOnlyCollection<long>>
    {
        private readonly IUserIpRepository _repository;

        public FindUsersByIpPrefixQueryHandler(IUserIpRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<long>> Handle(FindUsersByIpPrefixQuery request, CancellationToken cancellationToken)
            => await _repository.FindUsersByIpPrefixAsync(request.Prefix, cancellationToken);
    }
}
