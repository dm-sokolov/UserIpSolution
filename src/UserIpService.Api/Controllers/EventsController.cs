using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Connect([FromBody] ConnectEventDto dto)
        {
            await _mediator.Send(new ProcessConnectionCommand(dto.UserId, dto.Ip));
            return Ok();
        }

        public record ConnectEventDto(long UserId, string Ip);
    }
}
