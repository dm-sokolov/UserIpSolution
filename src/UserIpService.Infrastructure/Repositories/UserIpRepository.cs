using Microsoft.EntityFrameworkCore;
using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;
using UserIpService.Infrastructure.Data;

namespace UserIpService.Infrastructure.Repositories
{
    /// <summary>
    /// Класс репозитория для БД UserIp
    /// </summary>
    public class UserIpRepository : IUserIpRepository
    {
        private readonly UserIpContext _context;

        public UserIpRepository(UserIpContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавление или обновление одной записи пользователя и IP
        /// </summary>
        /// <param name="entry">Модель пользователя и IP</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task UpsertAsync(UserIp entry, CancellationToken cancellationToken)
        {
            var existing = await _context.UserIps
                .FirstOrDefaultAsync(x => x.UserId == entry.UserId && x.IpText == entry.IpText, cancellationToken);

            if (existing == null)
            {
                entry.FirstSeen = entry.LastSeen;
                _context.UserIps.Add(entry);
            }
            else
            {
                existing.LastSeen = entry.LastSeen;
                existing.Count++;
                _context.UserIps.Update(existing);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Добавление или обновление группы записей пользователей и IP
        /// </summary>
        /// <param name="entries">Группа записей пользователей и IP</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task UpsertBatchAsync(IEnumerable<UserIp> entries, CancellationToken cancellationToken)
        {           
            var keys = entries.Select(e => new { e.UserId, e.IpText }).ToList();

            var existing = await _context.UserIps
                .Where(x => keys.Contains(new { x.UserId, x.IpText }))
                .ToListAsync(cancellationToken);

            var existingMap = existing.ToDictionary(
                x => (x.UserId, x.IpText),
                x => x);

            var newEntries = new List<UserIp>();

            foreach (var entry in entries)
            {
                var key = (entry.UserId, entry.IpText);

                if (existingMap.TryGetValue(key, out var existingEntry))
                {                    
                    existingEntry.LastSeen = entry.LastSeen;
                    existingEntry.Count++;
                }
                else
                {                    
                    entry.FirstSeen = entry.LastSeen;
                    newEntries.Add(entry);
                }
            }

            if (newEntries.Count > 0)
            {
                await _context.UserIps.AddRangeAsync(newEntries, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Получение логинов пользователей по полному или начальной части IP адреса
        /// </summary>
        /// <param name="ipPrefix">полная или начальная часть IP адреса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<long>> FindUsersByIpPrefixAsync(string ipPrefix, CancellationToken cancellationToken)
        {
            var result = await _context.UserIps
                .Where(x => x.IpText.StartsWith(ipPrefix))
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync(cancellationToken);
            
            return result;
        }

        /// <summary>
        /// Получение списка всех подключений по пользователю
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<UserIp>> GetUserIpsByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            var result = await _context.UserIps
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
            
            return result;
        }

        /// <summary>
        /// Получение данных последнего подключению по пользователю
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<UserIp?> GetUserLastConnectionByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            var result = await _context.UserIps
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LastSeen)
                .FirstOrDefaultAsync(cancellationToken);
            
            return result;
        }

        /// <summary>
        /// Получение времени последнего времени подключения по IP
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task<DateTimeOffset?> GetLastConnectionDateTimeByIpAsync(string ip, CancellationToken cancellationToken)
        {
            var result = await _context.UserIps
                .Where(x => x.IpText == ip)
                .OrderByDescending(x => x.LastSeen)
                .Select(x => (DateTimeOffset?)x.LastSeen)
                .FirstOrDefaultAsync(cancellationToken);
            
            return result;
        }
    }
}
