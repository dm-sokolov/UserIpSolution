using System.Net;
using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;

namespace UserIpService.Core.Services
{
    /// <summary>
    /// Бизнес-логика для работы с пользователями и IP.
    /// </summary>
    public class UserConnectionService : IUserConnectionService
    {
        private readonly IUserIpRepository _repository;

        public UserConnectionService(IUserIpRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Обработка события подключения.
        /// </summary>
        public async Task ProcessConnectionAsync(long userId, string ipText, DateTimeOffset? dateTime = null, CancellationToken cancellationToken = default)
        {
            var ipAddress = IPAddress.Parse(ipText);
            var now = dateTime ?? DateTimeOffset.UtcNow;

            var entry = new UserIp
            {
                UserId = userId,
                IpText = ipText,
                IpAddress = ipAddress,
                FirstSeen = now,
                LastSeen = now,
                Count = 1
            };

            await _repository.UpsertAsync(entry, cancellationToken);
        }

        /// <summary>
        /// Обработка запроса на получение списка пользователей по IP-префиксу.
        /// </summary>
        /// <param name="ipPrefix">IP-префикс</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<IReadOnlyCollection<long>> FindUsersByIpPrefixAsync(string ipPrefix, CancellationToken cancellationToken = default)
            => _repository.FindUsersByIpPrefixAsync(ipPrefix, cancellationToken);

        /// <summary>
        /// Обработка запроса получения списка IP-адресов пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<IReadOnlyCollection<UserIp>> GetUserIpsByUserIdAsync(long userId, CancellationToken cancellationToken = default)
            => _repository.GetUserIpsByUserIdAsync(userId, cancellationToken);

        /// <summary>
        /// Обработка запроса получения последнего подключения пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<UserIp?> GetUserLastConnectionByUserIdAsync(long userId, CancellationToken cancellationToken = default)
            => _repository.GetUserLastConnectionByUserIdAsync(userId, cancellationToken);

        /// <summary> 
        /// Обработка запроса получения времени последнего подключения по IP.
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<DateTimeOffset?> GetLastConnectionDateTimeByIpAsync(string ip, CancellationToken cancellationToken = default)
            => _repository.GetLastConnectionDateTimeByIpAsync(ip, cancellationToken);
    }
}
