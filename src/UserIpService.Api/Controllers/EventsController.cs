using Microsoft.AspNetCore.Mvc;
using UserIpService.Core.Interfaces;

namespace UserIpService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IUserConnectionService _service;

        public EventsController(IUserConnectionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Принимает событие подключения.
        /// </summary>
        [HttpPost("connect")]
        public async Task<IActionResult> Connect([FromBody] ConnectEventDto dto)
        {
            await _service.ProcessConnectionAsync(dto.UserId, dto.Ip);
            return Ok();
        }

        public record ConnectEventDto(long UserId, string Ip);
    }
}
