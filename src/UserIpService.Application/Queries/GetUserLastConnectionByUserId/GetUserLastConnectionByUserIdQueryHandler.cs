using MediatR;
using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;

namespace UserIpService.Application.Queries.GetUserLastConnection
{
    /// <summary>
    /// Обработчик для запроса получения последнего подключения пользователя.
    /// </summary>
    public class GetUserLastConnectionByUserIdQueryHandler : IRequestHandler<GetUserLastConnectionByUserIdQuery, UserIp?>
    {
        private readonly IUserIpRepository _repository;

        public GetUserLastConnectionByUserIdQueryHandler(IUserIpRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserIp?> Handle(GetUserLastConnectionByUserIdQuery request, CancellationToken cancellationToken)
            => await _repository.GetUserLastConnectionByUserIdAsync(request.UserId, cancellationToken);
    }
}
