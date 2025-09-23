using MediatR;
using UserIpService.Core.Interfaces;

namespace UserIpService.Application.Queries.GetLastConnectionByIp
{
    /// <summary>
    /// Обработчик для запроса получения времени последнего подключения по IP.
    /// </summary>
    public class GetLastConnectionDateTimeByIpQueryHandler : IRequestHandler<GetLastConnectionDateTimeByIpQuery, DateTimeOffset?>
    {
        private readonly IUserIpRepository _repository;

        public GetLastConnectionDateTimeByIpQueryHandler(IUserIpRepository repository)
        {
            _repository = repository;
        }

        public async Task<DateTimeOffset?> Handle(GetLastConnectionDateTimeByIpQuery request, CancellationToken cancellationToken)
            => await _repository.GetLastConnectionDateTimeByIpAsync(request.Ip, cancellationToken);
    }
}
