using UserIpService.Core.Entities;

namespace UserIpService.Core.Interfaces
{
    /// <summary>
    /// Интерфейс бизнес-сервиса для работы с пользователями и их IP.
    /// </summary>
    public interface IUserConnectionService
    {
        /// <summary>
        /// Обработка события подключения.
        /// </summary>
        Task ProcessConnectionAsync(long userId, string ipText, DateTimeOffset? dateTime = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обработка запроса на получение списка пользователей по IP-префиксу.
        /// </summary>
        /// <param name="ipPrefix">IP-префикс</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<IReadOnlyCollection<long>> FindUsersByIpPrefixAsync(string ipPrefix, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обработка запроса получения списка IP-адресов пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<IReadOnlyCollection<UserIp>> GetUserIpsByUserIdAsync(long userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обработка запроса получения последнего подключения пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<UserIp?> GetUserLastConnectionByUserIdAsync(long userId, CancellationToken cancellationToken = default);

        /// <summary> 
        /// Обработка запроса получения времени последнего подключения по IP.
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<DateTimeOffset?> GetLastConnectionDateTimeByIpAsync(string ip, CancellationToken cancellationToken = default);
    }
}
