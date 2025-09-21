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
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [HttpGet("find-by-ip")]
        [Description("Получить данные фотокарточек")]
        public async Task<IActionResult> FindUsers([FromQuery] string prefix)
        {
            var result = await _service.FindUsersByIpPrefixAsync(prefix);
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/ips")]
        [Produces(typeof(UserIp))]
        [Description("Получить данные фотокарточек")]
        public async Task<IActionResult> GetUserIps(long userId)
        {
            var ips = await _service.GetUserIpsAsync(userId);
            return Ok(ips);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/last-connection")]
        [Produces(typeof(UserIp))]
        [Description("Получить данные фотокарточек")]
        public async Task<IActionResult> GetUserLast(long userId)
        {
            var last = await _service.GetUserLastConnectionAsync(userId);
            return Ok(last);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("last-connection-by-ip")]
        [Description("Получить данные фотокарточек")]
        public async Task<IActionResult> GetLastByIp([FromQuery] string ip)
        {
            var time = await _service.GetLastConnectionByIpAsync(ip);
            return Ok(time);
        }
    }
}
