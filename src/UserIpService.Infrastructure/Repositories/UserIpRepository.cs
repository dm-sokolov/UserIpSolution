using Microsoft.EntityFrameworkCore;
using UserIpService.Core.Entities;
using UserIpService.Core.Interfaces;

namespace UserIpService.Infrastructure.Repositories
{
    public class UserIpRepository : IUserIpRepository
    {
        private readonly UserIpContext _ctx;

        public UserIpRepository(UserIpContext ctx)
        {
            _ctx = ctx;
        }

        public async Task UpsertAsync(UserIp entry, CancellationToken ct = default)
        {
            var existing = await _ctx.UserIps
                .FirstOrDefaultAsync(x => x.UserId == entry.UserId && x.IpText == entry.IpText, ct);

            if (existing == null)
            {
                entry.FirstSeen = entry.LastSeen;
                _ctx.UserIps.Add(entry);
            }
            else
            {
                existing.LastSeen = entry.LastSeen;
                existing.Count++;
                _ctx.UserIps.Update(existing);
            }

            await _ctx.SaveChangesAsync(ct);
        }

        public async Task UpsertBatchAsync(IEnumerable<UserIp> entries, CancellationToken ct = default)
        {
            foreach (var entry in entries)
                await UpsertAsync(entry, ct);
        }

        public async Task<IReadOnlyCollection<long>> FindUsersByIpPrefixAsync(string ipPrefix, CancellationToken ct = default)
        {
            return await _ctx.UserIps
                .Where(x => x.IpText.StartsWith(ipPrefix))
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyCollection<UserIp>> GetUserIpsAsync(long userId, CancellationToken ct = default)
        {
            return await _ctx.UserIps
                .Where(x => x.UserId == userId)
                .ToListAsync(ct);
        }

        public async Task<UserIp?> GetUserLastConnectionAsync(long userId, CancellationToken ct = default)
        {
            return await _ctx.UserIps
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LastSeen)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<DateTimeOffset?> GetLastConnectionByIpAsync(string ip, CancellationToken ct = default)
        {
            return await _ctx.UserIps
                .Where(x => x.IpText == ip)
                .OrderByDescending(x => x.LastSeen)
                .Select(x => (DateTimeOffset?)x.LastSeen)
                .FirstOrDefaultAsync(ct);
        }
    }
}
