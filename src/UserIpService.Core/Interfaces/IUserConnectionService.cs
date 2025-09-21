using UserIpService.Core.Entities;

namespace UserIpService.Core.Interfaces
{
    /// <summary>
    /// Интерфейс бизнес-сервиса для работы с пользователями и их IP.
    /// </summary>
    public interface IUserConnectionService
    {
        Task ProcessConnectionAsync(long userId, string ipText, DateTimeOffset? ts = null, CancellationToken ct = default);
        Task<IReadOnlyCollection<long>> FindUsersByIpPrefixAsync(string ipPrefix, CancellationToken ct = default);
        Task<IReadOnlyCollection<UserIp>> GetUserIpsAsync(long userId, CancellationToken ct = default);
        Task<UserIp?> GetUserLastConnectionAsync(long userId, CancellationToken ct = default);
        Task<DateTimeOffset?> GetLastConnectionByIpAsync(string ip, CancellationToken ct = default);
    }
}
