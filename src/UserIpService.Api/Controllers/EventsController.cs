using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using UserIpService.Application.Commands.ProcessConnection;

namespace UserIpService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("connect")]
        [Description("Регистрация подключения пользователя")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Connect([FromBody] ConnectEventDto dto, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ProcessConnectionCommand(dto.UserId, dto.Ip), cancellationToken);

            return Ok();
        }

        public record ConnectEventDto(long UserId, string Ip);
    }
}
