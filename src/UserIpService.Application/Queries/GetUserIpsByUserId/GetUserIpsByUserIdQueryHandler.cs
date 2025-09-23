using MediatR;
using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;

namespace UserIpService.Application.Queries.GetUserIps
{
    /// <summary>
    /// Обработчик для запроса получения списка IP-адресов пользователя.
    /// </summary>
    public class GetUserIpsByUserIdQueryHandler : IRequestHandler<GetUserIpsByUserIdQuery, IReadOnlyCollection<UserIp>>
    {
        private readonly IUserIpRepository _repository;

        public GetUserIpsByUserIdQueryHandler(IUserIpRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<UserIp>> Handle(GetUserIpsByUserIdQuery request, CancellationToken cancellationToken)
            => await _repository.GetUserIpsByUserIdAsync(request.UserId, cancellationToken);
    }
}
