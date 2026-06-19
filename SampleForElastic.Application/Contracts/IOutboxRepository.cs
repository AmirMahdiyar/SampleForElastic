using SampleForElastic.Domain.Models.Outbox;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SampleForElastic.Application.Contracts
{
    public interface IOutboxRepository
    {
        Task<List<OutboxMessage>> FetchPendingMessagesWithLockAsync(int batchSize, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
