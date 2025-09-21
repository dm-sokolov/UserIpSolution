using UserIpService.Core.Entities;

namespace UserIpService.Core.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с данными UserIp.
    /// </summary>
    public interface IUserIpRepository
    {
        Task UpsertAsync(UserIp entry, CancellationToken ct = default);
        Task UpsertBatchAsync(IEnumerable<UserIp> entries, CancellationToken ct = default);

        Task<IReadOnlyCollection<long>> FindUsersByIpPrefixAsync(string ipPrefix, CancellationToken ct = default);
        Task<IReadOnlyCollection<UserIp>> GetUserIpsAsync(long userId, CancellationToken ct = default);
        Task<UserIp?> GetUserLastConnectionAsync(long userId, CancellationToken ct = default);
        Task<DateTimeOffset?> GetLastConnectionByIpAsync(string ip, CancellationToken ct = default);
    }
}
