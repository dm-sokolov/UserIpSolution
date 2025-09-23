using UserIpService.Core.Entities;

namespace UserIpService.Core.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с данными UserIp.
    /// </summary>
    public interface IUserIpRepository
    {
        /// <summary>
        /// Добавление или обновление одной записи пользователя и IP
        /// </summary>
        /// <param name="entry">Модель пользователя и IP</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task UpsertAsync(UserIp entry, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавление или обновление группы записей пользователей и IP
        /// </summary>
        /// <param name="entries">Группа записей пользователей и IP</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task UpsertBatchAsync(IEnumerable<UserIp> entries, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение логинов пользователей по полному или начальной части IP адреса
        /// </summary>
        /// <param name="ipPrefix">полная или начальная часть IP адреса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<IReadOnlyCollection<long>> FindUsersByIpPrefixAsync(string ipPrefix, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение списка всех подключений по пользователю
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<IReadOnlyCollection<UserIp>> GetUserIpsByUserIdAsync(long userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение данных последнего подключению по пользователю
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<UserIp?> GetUserLastConnectionByUserIdAsync(long userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение времени последнего времени подключения по IP
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<DateTimeOffset?> GetLastConnectionDateTimeByIpAsync(string ip, CancellationToken cancellationToken = default);
    }
}
