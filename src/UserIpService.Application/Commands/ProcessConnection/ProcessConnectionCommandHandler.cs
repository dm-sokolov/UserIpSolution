using MediatR;
using UserIpService.Core.Interfaces;

namespace UserIpService.Application.Commands.ProcessConnection
{
    /// <summary>
    /// Обработчик для команды на регистрацию подключения пользователя
    /// </summary>
    public class ProcessConnectionCommandHandler : IRequestHandler<ProcessConnectionCommand>
    {
        private readonly IUserConnectionService _service;

        public ProcessConnectionCommandHandler(IUserConnectionService service)
        {
            _service = service;
        }

        public async Task Handle(ProcessConnectionCommand request, CancellationToken cancellationToken)
            => await _service.ProcessConnectionAsync(request.UserId, request.IpText, null, cancellationToken);
    }
}
