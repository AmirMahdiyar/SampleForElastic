using Microsoft.EntityFrameworkCore;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Domain.Models.Outbox;

namespace SampleForElastic.Infraustructure.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly SampleDbContext _context;

        public OutboxRepository(SampleDbContext context)
        {
            _context = context;
        }

        public async Task<List<OutboxMessage>> FetchPendingMessagesWithLockAsync(int batchSize, CancellationToken ct)
        {
            await _context.Database.BeginTransactionAsync(ct);
            return await _context.OutboxMessages
                .FromSqlRaw("SELECT TOP ({0}) * FROM dbo.OutboxMessages WITH (UPDLOCK, READPAST) WHERE Status = 0 ORDER BY Id", batchSize)
                .ToListAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CurrentTransaction.CommitAsync(ct);
            }
        }
    }
}
