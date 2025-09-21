using System.Net;

namespace UserIpService.Core.Entities
{
    /// <summary>
    /// Хранит информацию о подключении пользователя с конкретного IP.
    /// </summary>
    public class UserIp
    {
        public long UserId { get; set; }
        public string IpText { get; set; } = string.Empty;
        public IPAddress IpAddress { get; set; } = IPAddress.None;
        public DateTimeOffset FirstSeen { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public long Count { get; set; }
    }
}
