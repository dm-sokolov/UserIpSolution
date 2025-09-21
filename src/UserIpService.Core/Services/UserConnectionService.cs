using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;

namespace UserIpService.Core.Services
{
    /// <summary>
    /// Бизнес-логика для работы с пользователями и IP.
    /// </summary>
    public class UserConnectionService : IUserConnectionService
    {
        private readonly IUserIpRepository _repo;

        public UserConnectionService(IUserIpRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Обработка события подключения.
        /// </summary>
        public async Task ProcessConnectionAsync(long userId, string ipText, DateTimeOffset? ts = null, CancellationToken ct = default)
        {
            var ipAddress = System.Net.IPAddress.Parse(ipText);
            var now = ts ?? DateTimeOffset.UtcNow;

            var entry = new UserIp
            {
                UserId = userId,
                IpText = ipText,
                IpAddress = ipAddress,
                FirstSeen = now,
                LastSeen = now,
                Count = 1
            };

            await _repo.UpsertAsync(entry, ct);
        }

        public Task<IReadOnlyCollection<long>> FindUsersByIpPrefixAsync(string ipPrefix, CancellationToken ct = default)
            => _repo.FindUsersByIpPrefixAsync(ipPrefix, ct);

        public Task<IReadOnlyCollection<UserIp>> GetUserIpsAsync(long userId, CancellationToken ct = default)
            => _repo.GetUserIpsAsync(userId, ct);

        public Task<UserIp?> GetUserLastConnectionAsync(long userId, CancellationToken ct = default)
            => _repo.GetUserLastConnectionAsync(userId, ct);

        public Task<DateTimeOffset?> GetLastConnectionByIpAsync(string ip, CancellationToken ct = default)
            => _repo.GetLastConnectionByIpAsync(ip, ct);
    }
}
