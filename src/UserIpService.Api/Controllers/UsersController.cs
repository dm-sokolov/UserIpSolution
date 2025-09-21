using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;

namespace UserIpService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserConnectionService _service;

        public UsersController(IUserConnectionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получить данные пользователя по IP
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [HttpGet("find-by-ip")]
        [Description("Получить Id пользователя по IP")]
        [ProducesResponseType(typeof(IReadOnlyCollection<long>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyCollection<long>>> FindUsers([FromQuery] string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return BadRequest("Параметр prefix обязателен.");

            var result = await _service.FindUsersByIpPrefixAsync(prefix);

            if (result == null || result.Count == 0)
                return NotFound($"Пользователи с IP, начинающимся на {prefix}, не найдены.");

            return Ok(result);
        }

        /// <summary>
        /// Получить данные о всех IP для конкретного пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/ips")]
        [Description("Получить данные о всех IP для конкретного пользователя")]
        [ProducesResponseType(typeof(IReadOnlyCollection<UserIp>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyCollection<UserIp>>> GetUserIps(long userId)
        {
            var ips = await _service.GetUserIpsAsync(userId);
            if (ips == null || ips.Count == 0)
            {
                return NotFound($"IP-адреса для пользователя {userId} не найдены.");
            }
            return Ok(ips);
        }

        /// <summary>
        /// Получить данные последнего подключения по пользователю
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/last-connection")]
        [Description("Получить данные последнего подключения по пользователю")]
        [ProducesResponseType(typeof(UserIp), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserLast(long userId)
        {
            var last = await _service.GetUserLastConnectionAsync(userId);

            if (last == null)
            {
                return NotFound($"Данные о последнем подключении пользователя {userId} не найдены.");
            }

            return Ok(last);
        }

        /// <summary>
        /// Получить время последнего подключения пользователя по IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("last-connection-by-ip")]
        [Description("Получить время последнего подключения пользователя по IP")]
        [ProducesResponseType(typeof(DateTime), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastByIp([FromQuery] string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return BadRequest("Параметр ip обязателен.");
            }

            var time = await _service.GetLastConnectionByIpAsync(ip);

            if (time == null)
            {
                return NotFound($"Для IP {ip} данные о последнем подключении не найдены.");
            }

            return Ok(time);
        }
    }
}
